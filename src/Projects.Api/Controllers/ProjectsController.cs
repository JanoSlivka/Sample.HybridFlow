using AutoBogus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

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
