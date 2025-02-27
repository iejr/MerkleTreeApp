namespace MerkleTreeApi.Models;

public class GetBalanceResponse {
    public int balance { get; set; }
    public MerkleTree.MerkleProofNode[] proofs { get; set; }
}
