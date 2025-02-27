namespace MerkleTreeApi.Services;

public interface IUserStateService {
    Dictionary<int, Models.User> user_state { get; set; }
    MerkleTree.MerkleTree merkle_tree { get; set; }
}

public class UserStateService : IUserStateService {
    public Dictionary<int, Models.User> user_state { get; set; } = new Dictionary<int, Models.User>();
    public MerkleTree.MerkleTree merkle_tree { get; set; } = new MerkleTree.MerkleTree();
}
