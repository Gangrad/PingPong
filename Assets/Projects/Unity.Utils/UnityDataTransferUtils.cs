using DataHelper;
using UnityEngine;

namespace Unity.Utils {
    public static class UnityDataTransferUtils {
        public static ByteWriter WriteVector2(this ByteWriter writer, Vector2 vector) {
            return writer.WriteFloat(vector.x)
                .WriteFloat(vector.y);
        }

        public static Vector2 ReadVector2(this ByteReader reader) {
            return new Vector2(reader.ReadFloat(), reader.ReadFloat());
        }
        
        public static ByteWriter WriteVector3(this ByteWriter writer, Vector3 vector) {
            return writer.WriteFloat(vector.x)
                .WriteFloat(vector.y)
                .WriteFloat(vector.z);
        }

        public static Vector3 ReadVector3(this ByteReader reader) {
            return new Vector3(reader.ReadFloat(), reader.ReadFloat(), reader.ReadFloat());
        }
        
        public static ByteWriter WriteColor(this ByteWriter writer, Color color) {
            return writer.WriteFloat(color.r)
                .WriteFloat(color.g)
                .WriteFloat(color.b)
                .WriteFloat(color.a);
        }

        public static Color ReadColor(this ByteReader reader) {
            return new Color(
                reader.ReadFloat(),
                reader.ReadFloat(),
                reader.ReadFloat(),
                reader.ReadFloat());
        }
    }
}