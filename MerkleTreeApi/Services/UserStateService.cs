namespace MerkleTreeApi.Services;

// Define a Singleton service running for storing stateful vars
public interface IUserStateService {
    // A map from user id to user object, used by GetBalance api
    Dictionary<int, Models.User> user_state { get; set; }
    // Built merkle tree
    MerkleTree.MerkleTree merkle_tree { get; set; }
}

public class UserStateService : IUserStateService {
    public Dictionary<int, Models.User> user_state { get; set; } = new Dictionary<int, Models.User>();
    public MerkleTree.MerkleTree merkle_tree { get; set; } = new MerkleTree.MerkleTree();
}
