namespace MerkleTreeApi;

public interface IUserStateService {
    Dictionary<int, User> user_state { get; set; }
    MerkleTree.MerkleTree merkle_tree { get; set; }
}

public class UserStateService : IUserStateService {
    public Dictionary<int, User> user_state { get; set; } = new Dictionary<int, User>();
    public MerkleTree.MerkleTree merkle_tree { get; set; } = new MerkleTree.MerkleTree();
}

public class User {
    public int id { get; set;}
    public int balance { get; set; }

    public string Serialization() {
        return string.Format("({0},{1})", this.id, this.balance);
    }
}