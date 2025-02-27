namespace MerkleTreeApi.Models;

public class CreateRequest {
    public List<User> user_data { get; set; } = new();
    public string? leaf_tag { get; set; }
    public string? branch_tag { get; set; }
}