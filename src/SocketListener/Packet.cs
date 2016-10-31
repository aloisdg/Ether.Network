using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketListener
{
    internal enum PacketState
    {
        Read = 0,
        Write,
    }

    public class Packet : IDisposable
    {
        private MemoryStream memoryStream;
        private BinaryReader memoryReader;
        private BinaryWriter memoryWriter;
        private PacketState state;

        private readonly Dictionary<Type, Func<BinaryReader, object>> readMethods = new Dictionary<Type, Func<BinaryReader, object>>()
        {
            { typeof(char), (reader) => { return reader.ReadChar(); } },
            { typeof(byte), (reader) => { return reader.ReadByte(); } },
            { typeof(bool), (reader) => { return reader.ReadBoolean(); } },
            { typeof(ushort), (reader) => { return reader.ReadUInt16(); } },
            { typeof(short), (reader) => { return reader.ReadInt16(); } },
            { typeof(uint), (reader) => { return reader.ReadUInt32(); } },
            { typeof(int), (reader) => { return reader.ReadInt32(); } },
            { typeof(ulong), (reader) => { return reader.ReadUInt64(); } },
            { typeof(long), (reader) => { return reader.ReadInt64(); } },
            { typeof(byte[]), (reader) => { return reader.ReadBytes(count: reader.ReadUInt16()); } },
            { typeof(string), (reader) => new string(reader.ReadChars(count: reader.ReadUInt16())) },
        };

        private readonly Dictionary<Type, Action<BinaryWriter, object>> writeMethods = new Dictionary<Type, Action<BinaryWriter, object>>()
        {
            { typeof(char), (writer, value) => writer.Write((char)value) },
            { typeof(byte), (writer, value) => writer.Write((byte)value) },
            { typeof(bool), (writer, value) => writer.Write((bool)value) },
            { typeof(ushort), (writer, value) => writer.Write((ushort)value) },
            { typeof(short), (writer, value) => writer.Write((short)value) },
            { typeof(uint), (writer, value) => writer.Write((uint)value) },
            { typeof(int), (writer, value) => writer.Write((int)value) },
            { typeof(ulong), (writer, value) => writer.Write((ulong)value) },
            { typeof(long), (writer, value) => writer.Write((long)value) },
            { typeof(byte[]), (writer, value) => writer.Write((byte[])value) },
            { typeof(string),
                (writer, value) =>
                {
                    writer.Write((ushort)value.ToString().Length);
                    if (value.ToString().Length > 0)
                        writer.Write(Encoding.ASCII.GetBytes(value.ToString()));
                }
            }
        };

        /// <summary>
        /// Gets the Packet buffer
        /// </summary>
        public Byte[] Buffer
        {
            get
            {
                int size = Convert.ToInt32(this.memoryStream.Length);

                if (this.state == PacketState.Write)
                {
                    this.memoryWriter.Seek(0, SeekOrigin.Begin);
                    this.Write(size);
                    this.memoryWriter.Seek(size, SeekOrigin.Begin);
                }

                return this.memoryStream.ToArray();
            }
        }

        public Packet()
        {
            this.state = PacketState.Write;

            this.memoryStream = new MemoryStream();
            this.memoryWriter = new BinaryWriter(this.memoryStream);
        }

        public Packet(byte[] buffer)
        {
            this.state = PacketState.Read;

            this.memoryStream = new MemoryStream(buffer);
            this.memoryReader = new BinaryReader(this.memoryStream);
        }

        public void Write<T>(T value)
        {
            if (this.state != PacketState.Write)
                throw new InvalidOperationException("Packet is in read-only mode.");

            Type type = typeof(T);

            if (this.writeMethods.ContainsKey(type))
                this.writeMethods[type](this.memoryWriter, value);
        }

        public T Read<T>()
        {
            if (this.state != PacketState.Read)
                throw new InvalidOperationException("Packet is in write-only mode.");

            Type type = typeof(T);

            if (this.readMethods.ContainsKey(type))
                return (T)this.readMethods[type](this.memoryReader);

            return default(T);
        }

        public void Dispose()
        {
            if (this.memoryReader != null)
            {
                this.memoryReader.Dispose();
                this.memoryReader = null;
            }

            if (this.memoryWriter != null)
            {
                this.memoryWriter.Dispose();
                this.memoryWriter = null;
            }

            if (this.memoryStream != null)
            {
                this.memoryStream.Dispose();
                this.memoryStream = null;
            }
        }
    }
}
