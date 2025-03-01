using Microsoft.AspNetCore.Mvc;

namespace MerkleTreeApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserBalanceController : ControllerBase
{
    private readonly Services.IUserStateService _user_state_service;

    public UserBalanceController(Services.IUserStateService user_state_service)
    {
        _user_state_service = user_state_service;
    }

    [HttpPost("create")]
    public IActionResult Create([FromBody] Models.CreateRequest request) {
        this._user_state_service.user_state = request.user_data.ToDictionary(e => e.id, e => e);

        string[] payloads = request.user_data.ConvertAll(e => e.Serialization()).ToArray(); 
        string leaf_tag = request.leaf_tag == null ? Models.Default.leaf_tag : request.leaf_tag;
        string branch_tag = request.branch_tag == null ? Models.Default.branch_tag : request.branch_tag;
        string result = _user_state_service.merkle_tree.Build(payloads, leaf_tag, branch_tag);
        return Ok(new Models.CreateResponse { root_hash = result });;
    }

    [HttpPost("getbalance")]
    public IActionResult GetBalance([FromBody] Models.GetBalanceRequest request) {
        if (!this._user_state_service.user_state.ContainsKey(request.user_id)) {
            return NotFound("User Id not found");
        }

        Models.User user = this._user_state_service.user_state[request.user_id];
        string query = user.Serialization();
        var result = this._user_state_service.merkle_tree.Proof(query);

        var response = new Models.GetBalanceResponse {
            balance = user.balance,
            proofs = result,
        };
        return Ok(response);
    }
}
