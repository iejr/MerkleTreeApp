using MerkleTree;
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

public class GetBalanceResponse {
    public int balance { get; set; }
    public MerkleProofNode[] proofs { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class UserBalanceController : ControllerBase
{
    private readonly IUserStateService _user_state_service;

    public UserBalanceController(IUserStateService user_state_service)
    {
        _user_state_service = user_state_service;
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] CreateRequest request) {
        this._user_state_service.user_state = request.user_data.ToDictionary(e => e.id, e => e);
        // Console.WriteLine("Show user id {0}", request.user_data[0].id);

        string[] payloads = request.user_data.ConvertAll(e => e.Serialization()).ToArray(); 
        string result = _user_state_service.merkle_tree.Build(payloads, request.leaf_tag!, request.branch_tag!);
        return Ok(new { Status = "Ok", Details = result });;
    }

    [HttpPost("getbalance")]
    public IActionResult GetBalance([FromBody] GetBalanceRequest request) {
        // Console.WriteLine("Show user id {0}", request.user_id);
        Console.WriteLine("Show state count {0}", this._user_state_service.user_state.Count);
        if (!this._user_state_service.user_state.ContainsKey(request.user_id)) {
            return NotFound(new { Message = "User Id not found" });
        }

        User user = this._user_state_service.user_state[request.user_id];
        string query = user.Serialization();
        var result = this._user_state_service.merkle_tree.Proof(query);

        var response = new GetBalanceResponse {
            balance = user.balance,
            proofs = result,
        };
        return Ok(response);
    }
}
