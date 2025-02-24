namespace MerkleTreeTest;

public class MerkleTreeTests
{
    [Fact]
    public void EmptyInputListNoTag_ReturnsCorrectHash() {
        string[] payloads = [];
        const string leaf_tag = "";
        const string branch_tag = "";

        var merkle_tree = new MerkleTree.MerkleTree();
        string root_hash = merkle_tree.Build(payloads, leaf_tag, branch_tag);
        Assert.Equal("", root_hash);
    }

    [Fact]
    public void SingleNodeNoTag_ReturnsCorrectRootHash() {
        string[] payloads = ["a"];
        const string leaf_tag = "";
        const string branch_tag = "";

        var merkle_tree = new MerkleTree.MerkleTree();
        string root_hash = merkle_tree.Build(payloads, leaf_tag, branch_tag);
        Assert.Equal("fc599dda7553c203a3ff32187bf1529b4ef53943ff425d6569f11b418e019bc1", root_hash);
    }

    [Fact]
    public void SingleNodeWithSameTags_ReturnsCorrectRootHash() {
        string[] payloads = ["a"];
        const string leaf_tag = "SomeUnitTest_Tag";
        const string branch_tag = "SomeUnitTest_Tag";

        var merkle_tree = new MerkleTree.MerkleTree();
        string root_hash = merkle_tree.Build(payloads, leaf_tag, branch_tag);
        Assert.Equal("589aa42d9b10b2bdc235ce7c7729c1aa727187a0a4d84f3c1cc83e62d594a27f", root_hash);
    }

    [Fact]
    public void SingleNodeWithDifferentTags_ReturnsCorrectRootHash() {
        string[] payloads = ["a"];
        const string leaf_tag = "SomeUnitTest_Tag";
        const string branch_tag = "AnotherUnitTest_Tag";

        var merkle_tree = new MerkleTree.MerkleTree();
        string root_hash = merkle_tree.Build(payloads, leaf_tag, branch_tag);
        Assert.Equal("589aa42d9b10b2bdc235ce7c7729c1aa727187a0a4d84f3c1cc83e62d594a27f", root_hash);
    }

    [Fact]
    public void PerfectNodesWithSameTags_ReturnsCorrectRootHash() {
        string[] payloads = ["111", "222", "333", "444"];
        const string leaf_tag = "Bitcoin_Transaction";
        const string branch_tag = "Bitcoin_Transaction";

        var merkle_tree = new MerkleTree.MerkleTree();
        string root_hash = merkle_tree.Build(payloads, leaf_tag, branch_tag);
        Assert.Equal("13581576d58d7daaab1f1cc6e6812fb54cc6e03ea8cedfeec1713fd60578044e", root_hash);
    }

    [Fact]
    public void OddNodesWithSameTags_ReturnsCorrectRootHash() {
        string[] payloads = ["aaa", "bbb", "ccc", "ddd", "eee"];
        const string leaf_tag = "Bitcoin_Transaction";
        const string branch_tag = "Bitcoin_Transaction";

        var merkle_tree = new MerkleTree.MerkleTree();
        string root_hash = merkle_tree.Build(payloads, leaf_tag, branch_tag);
        Assert.Equal("795ff52ba643da284182adc4bebe68dfcd25a48a41b2c552ebd3b8731b2d1381", root_hash);
    }

    [Fact]
    public void OddNodesWithDifferentTags_ReturnsCorrectRootHash() {
        string[] payloads = ["aaa", "bbb", "ccc", "ddd", "eee"];
        const string leaf_tag = "Leaf_Tag";
        const string branch_tag = "Branch_Tag";

        var merkle_tree = new MerkleTree.MerkleTree();
        string root_hash = merkle_tree.Build(payloads, leaf_tag, branch_tag);
        Assert.Equal("815b52897c579d5f1540f2066223c73e9a5b0c1b7fe326bf31eaae0f68911cca", root_hash);
    }
}