using Microsoft.AspNetCore.Mvc;

namespace MerkleTreeApi.Controllers;

public class CreateRequest {
    public List<User> user_data { get; set; } = new();
    public string? leaf_tag { get; set; }
    public string? branch_tag { get; set; }
}

public class GetBalanceRequest {
    public int user_id { get; set; }
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
    private Dictionary<int, User> _user_records;

    public UserBalanceController(ILogger<UserBalanceController> logger)
    {
        _logger = logger;
        _merkle_tree = new MerkleTree.MerkleTree();
        _user_records = new Dictionary<int, User>();
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] CreateRequest request) {
        this._user_records = request.user_data.ToDictionary(e => e.id, e => e);
        // Console.WriteLine("Show user id {0}", request.user_data[0].id);

        string[] payloads = request.user_data.ConvertAll(e => e.Serialization()).ToArray(); 
        string result = _merkle_tree.Build(payloads, request.leaf_tag!, request.branch_tag!);
        return Ok(new { Status = "Ok", Details = result });;
    }

    [HttpPost("getbalance")]
    public IActionResult GetBalance([FromBody] GetBalanceRequest request) {
        // Console.WriteLine("Show user id {0}", request.user_id);
        if (!this._user_records.ContainsKey(request.user_id)) {
            return NotFound(new { Message = "User Id not found" });
        }

        string query = this._user_records[request.user_id].Serialization();
        var result = _merkle_tree.Proof(query);
        return Ok(new { Status = "Ok", Details = result });;
    }
}
