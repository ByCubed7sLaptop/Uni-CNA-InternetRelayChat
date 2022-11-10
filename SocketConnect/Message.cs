using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SocketConnect
{
    public class Message
    {

        //

        public string Header { get; set; }
        public List<string> Args { get; set; }

        public Guid Id { get; set; } = Guid.NewGuid();

        //
        static public Message CreateRecieved(Message message) 
        {
            Message respond = new("Recieved");
            respond.Id = message.Id;
            return respond;
        }
        static public Message CreateDisconnect() => new("Disconnect");
        static public Message CreateShutdown() => new("Shutdown");

        //

        public Message()
        {
            Header = "HEADER";
            Args = new List<string>();
        }
        protected Message(string header) : this()
        {
            Header = header;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        public Message Titled(string header)
        {
            Header = header;
            return this;
        }
        public Message Add(string arg)
        {
            Args.Add(arg);
            return this;
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -

        virtual public Message FromString(string data)
        {
            //Console.WriteLine(data);
            data = data.Replace(Connector.EOF.ToString(), "");
            List<string> datalist = new List<string>(data.Split(Connector.Split));

            Header = datalist[0];
            //Console.WriteLine("Parsing: " + datalist[1]);
            //Id = Guid.ParseExact(datalist[1], "N");
            // See: https://stackoverflow.com/questions/28106574/try-parse-guid-issues-with-a-valid-guid
            string guid = Regex.Replace(datalist[1], "[^A-Fa-f0-9]", string.Empty);
            Id = Guid.ParseExact(guid, "N");

            if (datalist.Count > 2)
                Args = datalist.GetRange(2, datalist.Count-1);
            
            return this;
        }

        public override string ToString()
        {
            string datastring = Header;
            datastring += Connector.Split + Id.ToString("N");
            foreach (string arg in Args)
                datastring += Connector.Split + arg;
            datastring += Connector.EOF;
            return datastring;
        }


        public Message FromBytes(byte[] data)
        {
            return FromString(Encoding.UTF8.GetString(data));
        }

        public byte[] ToBytes()
        {
            return Encoding.UTF8.GetBytes(ToString());
        }


    }
}
