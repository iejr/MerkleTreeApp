namespace MerkleTreeApi.Models;

public class CreateRequest {
    public List<User> user_data { get; set; } = new();
    // Optional, will use Default.leaf_tag if not presented in the request
    public string? leaf_tag { get; set; }
    // Optional, will use default Default.branch_tag if not presented in the request
    public string? branch_tag { get; set; }
}