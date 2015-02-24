using System.Text;
using System.Net.Sockets;

namespace Prompter.Server
{
    public class ExchangeObject
    {
        public const int BufferSize = 1024;

        private byte[] buffer = new byte[BufferSize];
        private readonly StringBuilder contentBuilder = new StringBuilder();

        public Socket WorkSocket { get; set; }

        public byte[] Buffer
        {
            get { return buffer;  }
            set { buffer = value; }
        }

        public StringBuilder ContentBuilder
        {
            get { return contentBuilder; }
        }
    }
}