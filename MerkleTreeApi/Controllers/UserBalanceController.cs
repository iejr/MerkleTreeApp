using Microsoft.AspNetCore.Mvc;

namespace MerkleTreeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserBalanceController : ControllerBase
{
    private readonly ILogger<UserBalanceController> _logger;

    public UserBalanceController(ILogger<UserBalanceController> logger)
    {
        _logger = logger;
    }

    [HttpGet("create")]
    public ActionResult<string> Create() {
        return "Hello from Controller!";
    }
}
