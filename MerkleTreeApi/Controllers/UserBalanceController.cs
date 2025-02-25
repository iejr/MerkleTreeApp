using Microsoft.AspNetCore.Mvc;

namespace MerkleTreeApi.Controllers;

public class CreateRequest {
    public List<User> user_data { get; set; } = new();
    public string? leaf_tag { get; set; }
    public string? branch_tag { get; set; }
}

public class User {
    public int id { get; set;}
    public int balance { get; set; }

    public string Serialization() {
        return string.Format("({0},{1})", this.id, this.balance);
    }
}

[ApiController]
[Route("api/[controller]")]
public class UserBalanceController : ControllerBase
{
    private readonly ILogger<UserBalanceController> _logger;

    private  MerkleTree.MerkleTree _merkle_tree;

    public UserBalanceController(ILogger<UserBalanceController> logger)
    {
        _logger = logger;
        _merkle_tree = new MerkleTree.MerkleTree();
    }

    [HttpPost("create")]
    // public ActionResult<string> Create() {
    public IActionResult Create([FromBody] CreateRequest request) {
        string[] payloads = request.user_data.ConvertAll(e => e.Serialization()).ToArray(); 
        string result = _merkle_tree.Build(payloads, request.leaf_tag!, request.branch_tag!);
        return Ok(new { Status = "Ok", Details = result });;
    }
}
