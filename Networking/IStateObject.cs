using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Windows.Networking.Sockets;
using System.Threading.Tasks;

namespace Demo.Net
{
    public interface IStateObject
    {
        int BufferSize { get; }

        int Id { get; }

        bool Close { get; set; }

        byte[] Buffer { get; }

        StreamSocket Listener { get; }

        string Text { get; }

        void Append(string text);

        void Reset();
    }
}