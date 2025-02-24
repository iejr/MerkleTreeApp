namespace MerkleTreeTest;

public class MerkleTreeTests
{
    [Fact]
    public void SingleNodeNoTag_ReturnsCorrectRootHash() {
        string[] payloads = {"a"};
        const string leaf_tag = "";
        const string branch_tag = "";

        var merkle_tree = new MerkleTree.MerkleTree();
        string root_hash = merkle_tree.Build(payloads, leaf_tag, branch_tag);
        Assert.Equal("fc599dda7553c203a3ff32187bf1529b4ef53943ff425d6569f11b418e019bc1", root_hash);
    }

    [Fact]
    public void MultipleNodesWithBothTags_ReturnsCorrectRootHash() {
        string[] payloads = {"aaa", "bbb", "ccc", "ddd", "eee"};
        const string leaf_tag = "ProofOfReserve_Leaf";
        const string branch_tag = "ProofOfReserve_Branch";

        var merkle_tree = new MerkleTree.MerkleTree();
        string root_hash = merkle_tree.Build(payloads, leaf_tag, branch_tag);
        Assert.Equal("f59501f722a1e9cf3d97afb72876644ed4d2c40c471c92c2463f9a260a14bf90", root_hash);
    }
}