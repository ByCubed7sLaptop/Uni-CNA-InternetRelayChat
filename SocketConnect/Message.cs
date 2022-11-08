using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketConnect
{
    public class Message
    {
        public string Header { get; set; }
        public List<string> Args { get; set; }

        public Message()
        {
            Header = "HEADER";
            Args = new List<string>();
        }

        virtual public void FromString(string data)
        {
            List<string> datalist = new List<string>(data.Split(Connection.Split));

            Header = datalist[0];
            datalist.RemoveAt(0);

            Args = datalist;
        }

        public void FromBytes(byte[] data)
        {
            FromString(Encoding.UTF8.GetString(data));
        }

        public override string ToString()
        {
            string datastring = Header + Connection.Split;
            foreach (string arg in Args)
                datastring += arg + Connection.Split;
            datastring.Trim();
            datastring += Connection.EOF;
            return datastring;
        }

        public byte[] ToBytes()
        {
            return Encoding.UTF8.GetBytes(ToString());
        }


    }
}
