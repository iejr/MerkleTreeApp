namespace MerkleTreeApi.Models;

public class GetBalanceResponse {
    // Return user's balance if user presented, otherwise will be error response
    public int balance { get; set; }
    // Return merkle proof nodes if user presented, which are a list of hashes from
    // sibling nodes when goes up from the matched leaf node to root
    // root is not included
    public MerkleTree.MerkleProofNode[] proofs { get; set; }
}
