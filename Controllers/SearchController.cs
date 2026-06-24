using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeVault.Web.Data;
using SafeVault.Web.Models;
using SafeVault.Web.Services;

namespace SafeVault.Web.Controllers;

[ApiController]
[Route("api/search")]
[Authorize]
public class SearchController : ControllerBase
{
    private readonly UserSearchRepository _repository;

    public SearchController(UserSearchRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult Search([FromQuery] SearchRequest request)
    {
        // 1) Input validation: [RegularExpression] on SearchRequest rejects
        //    anything containing quotes, semicolons, angle brackets, SQL
        //    keywords used as syntax, etc. ModelState catches it first.
        if (!ModelState.IsValid || !InputValidationService.IsValidSearchTerm(request.Term))
        {
            return BadRequest(new { message = "Invalid search term." });
        }

        // 2) SQL injection prevention: the repository always binds the term
        //    as a parameter, never via string concatenation.
        var matches = _repository.SearchUsersByUsername(request.Term);

        // 3) XSS prevention: the term is HTML-encoded before being echoed
        //    back into the response payload, in case a client renders it
        //    directly into a page.
        var safeEchoTerm = InputValidationService.EncodeForHtml(request.Term);

        return Ok(new
        {
            searchedFor = safeEchoTerm,
            results = matches.Select(m => new { m.Id, m.Username, m.Email })
        });
    }
}
