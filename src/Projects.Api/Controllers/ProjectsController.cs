using AutoBogus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Projects.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProjectsController : ControllerBase
{
    [HttpGet]
    public IEnumerable<Project> GetProjects()
        => new AutoFaker<Project>().Generate(20).Select(p=> {
            p.Owner = User.Identity?.Name;
            return p;
        });
}
