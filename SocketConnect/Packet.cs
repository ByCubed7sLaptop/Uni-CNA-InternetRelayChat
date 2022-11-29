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
    /// <summary>
    /// A serializable abstract chunk of data that can be send over a socket.
    /// </summary>
    [Serializable]
    public abstract class Packet : IEquatable<Packet>
    {
        /// <summary>
        /// The generated guid of the packet
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Serialize

        /// <summary>
        /// Deserailizes an array of bytes into a packet.
        /// </summary>
        /// <typeparam name="T">The type of packet to cast it to.</typeparam>
        /// <param name="data">The serailized packet.</param>
        /// <returns>The deserailized packet.</returns>
        public static T FromBytes<T>(byte[] data) where T : Packet
        {
            // Create the formatter and memory stream.
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(data);

            // Deserialize
            T packet = (T) formatter.Deserialize(stream);

            // Return the deserialized data
            return packet;
        }

        /// <summary>
        /// Serailizes a packet into an array of bytes.
        /// </summary>
        /// <returns>The serailized packet.</returns>
        public byte[] ToBytes()
        {
            // Create the formatter and memory stream.
            IFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();

            // Serialize
            formatter.Serialize(stream, this);
            byte[] data = stream.ToArray();
            stream.Flush();

            // Return the serialized data
            return data;
        }


        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Equals

        public bool Equals(Packet? other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (other is null) return false;
            if (Id != other.Id) return false;

            return true;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Packet);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

}
