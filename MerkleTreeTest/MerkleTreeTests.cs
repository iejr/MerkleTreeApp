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
        Assert.Equal("df353dc99624bd3a86e61ff712ae4dfcf7341312207acf89220febccc6088981", root_hash);
    }

    [Fact]
    public void SingleNodeWithSameTags_ReturnsCorrectRootHash() {
        string[] payloads = ["a"];
        const string leaf_tag = "SomeUnitTest_Tag";
        const string branch_tag = "SomeUnitTest_Tag";

        var merkle_tree = new MerkleTree.MerkleTree();
        string root_hash = merkle_tree.Build(payloads, leaf_tag, branch_tag);
        Assert.Equal("ce1dea40688951931880427e03f4fc89741a1b727424ee3349671858e95a9227", root_hash);
    }

    [Fact]
    public void SingleNodeWithDifferentTags_ReturnsCorrectRootHash() {
        string[] payloads = ["a"];
        const string leaf_tag = "SomeUnitTest_Tag";
        const string branch_tag = "AnotherUnitTest_Tag";

        var merkle_tree = new MerkleTree.MerkleTree();
        string root_hash = merkle_tree.Build(payloads, leaf_tag, branch_tag);
        Assert.Equal("ce1dea40688951931880427e03f4fc89741a1b727424ee3349671858e95a9227", root_hash);
    }

    [Fact]
    public void PerfectNodesWithSameTags_ReturnsCorrectRootHash() {
        string[] payloads = ["111", "222", "333", "444"];
        const string leaf_tag = "Bitcoin_Transaction";
        const string branch_tag = "Bitcoin_Transaction";

        var merkle_tree = new MerkleTree.MerkleTree();
        string root_hash = merkle_tree.Build(payloads, leaf_tag, branch_tag);
        Assert.Equal("ac948e99d8b9cba7696cc914cbf1d300b457d94bdefa37fb85a4b15d43978866", root_hash);
    }

    [Fact]
    public void OddNodesWithSameTags_ReturnsCorrectRootHash() {
        string[] payloads = ["aaa", "bbb", "ccc", "ddd", "eee"];
        const string leaf_tag = "Bitcoin_Transaction";
        const string branch_tag = "Bitcoin_Transaction";

        var merkle_tree = new MerkleTree.MerkleTree();
        string root_hash = merkle_tree.Build(payloads, leaf_tag, branch_tag);
        Assert.Equal("ea5c29ddb781d6a00344c0194dde7eaf062852a89950543503a095b004951a13", root_hash);
    }

    [Fact]
    public void OddNodesWithDifferentTags_ReturnsCorrectRootHash() {
        string[] payloads = ["aaa", "bbb", "ccc", "ddd", "eee"];
        const string leaf_tag = "Leaf_Tag";
        const string branch_tag = "Branch_Tag";

        var merkle_tree = new MerkleTree.MerkleTree();
        string root_hash = merkle_tree.Build(payloads, leaf_tag, branch_tag);
        Assert.Equal("a6299a061ce5cc402f6117b6f68124ca1f6c157314c3406d73e3252636da6ae2", root_hash);
    }
}