using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/test")]
public class TaskController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Controller works");
    }
}