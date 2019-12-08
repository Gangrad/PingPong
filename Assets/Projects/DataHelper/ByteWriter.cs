using System;
using System.Text;

namespace DataHelper {
    public class ByteWriter {
        private byte[] _data;
        private int _pos;

        public byte[] Data {
            get {
                var res = new byte[_pos];
                Array.Copy(_data, res, _pos);
                return res;
            }
        }

        public ByteWriter(int capacity) {
            _data = new byte[capacity];
            _pos = 0;
        }

        private void EnsureCapacity(int size) {
            if (_pos + size <= _data.Length) {
                int newSize = size >= _data.Length ? size*2 : _data.Length*2;
                var data = new byte[newSize];
                Array.Copy(_data, data, _pos);
                _data = data;
            }
        }

        private void Write(byte[] bytes) {
            int size = bytes.Length;
            EnsureCapacity(size);
            Buffer.BlockCopy(bytes, 0, _data, _pos, size);
            _pos += size;
        }

        public ByteWriter WriteBool(bool value) {
            Write(BitConverter.GetBytes(value));
            return this;
        }

        public ByteWriter WriteChar(char value) {
            Write(BitConverter.GetBytes(value));
            return this;
        }

        public ByteWriter WriteByte(byte value) {
            EnsureCapacity(1);
            _data[_pos++] = value;
            return this;
        }

        public ByteWriter WriteShort(short value) {
            Write(BitConverter.GetBytes(value));
            return this;
        }

        public ByteWriter WriteInt(int value) {
            Write(BitConverter.GetBytes(value));
            return this;
        }

        public ByteWriter WriteLong(long value) {
            Write(BitConverter.GetBytes(value));
            return this;
        }

        public ByteWriter WriteFloat(float value) {
            Write(BitConverter.GetBytes(value));
            return this;
        }

        public ByteWriter WriteDouble(double value) {
            Write(BitConverter.GetBytes(value));
            return this;
        }

        private ByteWriter WriteString(string s, Encoding enc) {
            var bytes = enc.GetBytes(s);
            WriteInt(bytes.Length);
            Write(bytes);
            return this;
        }

        public ByteWriter WriteAsciiString(string value) {
            WriteString(value, Encoding.ASCII);
            return this;
        }

        public ByteWriter WriteUtf8String(string value) {
            WriteString(value, Encoding.UTF8);
            return this;
        }

        public ByteWriter WriteUtf32String(string value) {
            WriteString(value, Encoding.UTF32);
            return this;
        }
    }
}
