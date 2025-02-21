namespace MerkleTree;

class Hash {
    public static string TagHash(string data, string tag) {
        string hash_val = tag + tag + data;
        return hash_val;
    }
}

public class MerkleTreeNode {
    public string hash;
    public MerkleTreeNode? parent;
    public MerkleTreeNode? sibling;

    public MerkleTreeNode (string hash) {
        this.hash = hash;
        this.parent = null;
        this.sibling = null;
    }
}

public class MerkleTree {
    public MerkleTreeNode? root = null;
    Dictionary<string, MerkleTreeNode> entrances = new Dictionary<string, MerkleTreeNode>();

    public void Build(string[] payloads, string leaf_tag, string branch_tag) {
        Queue<MerkleTreeNode> order = new Queue<MerkleTreeNode>();
        string leaf_hash = "";
        foreach (string payload in payloads) {
            leaf_hash = Hash.TagHash(payload, leaf_tag);
            MerkleTreeNode node = new MerkleTreeNode(leaf_hash);
            order.Enqueue(node);
            this.entrances.Add(payload, node);
        }
        if (order.Count > 1 && (order.Count & 1) == 1) {
            MerkleTreeNode node = new MerkleTreeNode(leaf_hash);
            order.Enqueue(node);
        }

        while (order.Count > 1) {
            int level_size = order.Count;
            while (level_size > 1) {
                MerkleTreeNode left_child = order.Dequeue();
                MerkleTreeNode right_child = order.Dequeue();

                string branch_hash = Hash.TagHash(left_child.hash + right_child.hash, branch_tag);
                MerkleTreeNode node = new MerkleTreeNode(branch_hash);

                left_child.parent = node;
                left_child.sibling = right_child;

                right_child.parent = node;
                right_child.sibling = left_child;

                order.Enqueue(node);

                level_size -= 2;
            }

            if (level_size == 1) {
                order.Enqueue(order.Dequeue());
            }
        }

        if (order.Count == 1) {
            this.root = order.Dequeue();
        }
    }

    public void Show() {
        Console.WriteLine("============================================================");
        foreach (KeyValuePair<string, MerkleTreeNode> kv in this.entrances) {
            MerkleTreeNode node = kv.Value;
            Console.Write("{0}", node.hash);
            while (node.parent != null) {
                Console.Write(" -> {0}", node.parent.hash);
                node = node.parent;
            }
            Console.WriteLine();
        }
        Console.WriteLine("============================================================");
    }
}