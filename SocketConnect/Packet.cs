using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SocketConnect
{
    // Make abstract
    [Serializable]
    public class Packet : IEquatable<Packet>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Packet() {}

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Serialize

        public static T FromBytes<T>(byte[] data) where T : Packet
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(data);

            return (T)formatter.Deserialize(stream);
        }

        public byte[] ToBytes()
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();

            formatter.Serialize(stream, this);
            byte[] data = stream.ToArray();
            stream.Flush();

            return data;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Equals

        public bool Equals(Packet? other)
        {
            if (other is null) return false;
            if (!ReferenceEquals(this, other)) return false;
            if (Id != other.Id) return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Packet);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

}
