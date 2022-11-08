using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketConnect
{
    public class Message
    {
        string header;
        List<string> args;

        public void FromBytes(byte[] data)
        {
            string datastring = Encoding.UTF8.GetString(data);
            List<string> datalist = new List<string>(datastring.Split(Connection.Split));

            header = datalist[0];
            datalist.RemoveAt(0);
            
            args = datalist;
        }

        public byte[] ToBytes()
        {
            string datastring = header + Connection.Split;
            foreach (string arg in args) 
                datastring += arg + Connection.Split;
            datastring.Trim();
            datastring += Connection.EOF;
            return Encoding.UTF8.GetBytes(datastring);
        }


    }
}
