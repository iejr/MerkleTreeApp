using System.Security.Cryptography;

namespace MerkleTree;

class Hash {

    public static byte[] Utf8Decoding(string data) {
        return System.Text.Encoding.UTF8.GetBytes(data);
    }

    public static byte[] Sha256Hash(byte[] data) {
        SHA256 manager = SHA256.Create();
        return manager.ComputeHash(data);
    }

    public static string HexEncoding(byte[] data) {
        return BitConverter.ToString(data).Replace("-", string.Empty).ToLower();
    }

    public static byte[] ConcatBytes(byte[] array1, byte[] array2) {
        byte[] buffer = new byte[array1.Length + array2.Length];
        System.Buffer.BlockCopy(array1, 0, buffer, 0, array1.Length);
        System.Buffer.BlockCopy(array2, 0, buffer, array1.Length, array2.Length);
        return buffer;
    }

    public static byte[] Concat3Bytes(byte[] array1, byte[] array2, byte[] array3) {
        byte[] buffer = new byte[array1.Length + array2.Length + array3.Length];
        System.Buffer.BlockCopy(array1, 0, buffer, 0, array1.Length);
        System.Buffer.BlockCopy(array2, 0, buffer, array1.Length, array2.Length);
        System.Buffer.BlockCopy(array3, 0, buffer, array1.Length + array2.Length, array3.Length);
        return buffer;
    }

    public static byte[] TagHash(byte[] data, byte[] tag) {
        byte[] tag_hash = Sha256Hash(tag);
        byte[] buffer = Concat3Bytes(tag_hash, tag_hash, Sha256Hash(data));
        return Sha256Hash(buffer);
    }
}

public class MerkleTreeNode {
    public byte[] hash;
    public MerkleTreeNode? parent;
    public MerkleTreeNode? sibling;

    public MerkleTreeNode (byte[] hash) {
        this.hash = hash;
        this.parent = null;
        this.sibling = null;
    }
}

public class MerkleTree {
    public MerkleTreeNode? root = null;
    Dictionary<string, MerkleTreeNode> entrances = new Dictionary<string, MerkleTreeNode>();

    public string Build(string[] payloads, string leaf_tag, string branch_tag) {
        Queue<MerkleTreeNode> order = new Queue<MerkleTreeNode>();
        byte[] leaf_hash = Array.Empty<byte>();
        foreach (string payload in payloads) {
            leaf_hash = Hash.TagHash(Hash.Utf8Decoding(payload), Hash.Utf8Decoding(leaf_tag));
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

                byte[] branch_hash = Hash.TagHash(Hash.ConcatBytes(left_child.hash, right_child.hash), Hash.Utf8Decoding(branch_tag));
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
            return Hash.HexEncoding(root.hash);
        }
        return "";
    }

    public void Show() {
        Console.WriteLine("============================================================");
        foreach (KeyValuePair<string, MerkleTreeNode> kv in this.entrances) {
            MerkleTreeNode node = kv.Value;
            Console.Write("{0}", Hash.HexEncoding(node.hash));
            while (node.parent != null) {
                Console.Write(" -> {0}", Hash.HexEncoding(node.parent.hash));
                node = node.parent;
            }
            Console.WriteLine();
        }
        Console.WriteLine("============================================================");
    }
}