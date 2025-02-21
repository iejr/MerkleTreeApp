// See https://aka.ms/new-console-template for more information

using MerkleTree;

class Program {
    static void Main() {
        string[] payloads = {"aaa", "bbb", "ccc", "ddd", "eee"};
        const string leaf_tag = "";
        const string branch_tag = "";

        var merkle_tree = new MerkleTree.MerkleTree();
        merkle_tree.Build(payloads, leaf_tag, branch_tag);
        merkle_tree.Show();
    }
}