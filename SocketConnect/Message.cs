using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketConnect
{
    public class Message
    {

        //

        public string Header { get; set; }
        public List<string> Args { get; set; }

        //
        static public Message CreateRecieved() => new("Recieved");
        static public Message CreateShutdown() => new("Shutdown");

        //

        public Message()
        {
            Header = "HEADER";
            Args = new List<string>();
        }
        public Message(string header) : this()
        {
            FromString(header);
        }


        virtual public Message FromString(string data)
        {
            data = data.Replace(Connector.EOF, "");
            List<string> datalist = new List<string>(data.Split(Connector.Split));

            Header = datalist[0];
            datalist.RemoveAt(0);

            Args = datalist;
            return this;
        }

        public Message FromBytes(byte[] data)
        {
            return FromString(Encoding.UTF8.GetString(data));
        }

        public override string ToString()
        {
            string datastring = Header + Connector.Split;
            foreach (string arg in Args)
                datastring += arg + Connector.Split;
            datastring = datastring.Trim();
            datastring += Connector.EOF;
            return datastring;
        }

        public byte[] ToBytes()
        {
            return Encoding.UTF8.GetBytes(ToString());
        }


    }
}
