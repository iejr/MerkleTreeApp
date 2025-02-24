namespace Common;

public class Serializer {
    public static byte[] Utf8Decoding(string data) {
        return System.Text.Encoding.UTF8.GetBytes(data);
    }

    public static string HexEncoding(byte[] data) {
        return BitConverter.ToString(data).Replace("-", string.Empty).ToLower();
    }
}