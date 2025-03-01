namespace MerkleTreeApi.Models;

// Class model a user
public class User {
    public int id { get; set;}
    public int balance { get; set; }

    public string Serialization() {
        return string.Format("({0},{1})", this.id, this.balance);
    }
}