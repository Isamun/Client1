using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Networking.NetworkOperators;
using Windows.Networking.BackgroundTransfer;
using System.IO;
using Windows.Storage.Streams;
using Demo.Protocol;

namespace Networking
{
    public class Buffer : IBuffer
    {
        private uint _capacity;
        private uint _lenght;

        public uint Capacity
        {
            set { _capacity = value; }
            get { return _capacity; }
        }

        public uint Length
        {
            get
            {
                return _lenght;
            }
            set
            {
                _lenght = value;
            }
        }
    }
    public interface IClient
    {
        event DisconnectedHandler Disconnected;
        event MessageReceivedHandler MessageReceived;
        event MessageSubmittedHandler MessageSubmitted;

        public delegate void MessageReceivedHandler(Client N, String Msg);
        public delegate void MessageSubmittedHandler(Client N, String Msg);
        public delegate void DisconnectedHandler(Client N);

        async void StartClient();
        void StartClient(String Hostname, String Port);
        bool IsConnected();
        async void Receive();
        void Send(PDU msg, bool close);
        void Close();
        void returnmessage(IAsyncResult a);

    }

    public class Client : IClient
    {
        private StreamSocket _socket;
        private static HostName _localHost;
        private HostName _remoteHost;
        private String _serviceName;
        private EndpointPair _endpointpair;
        private DataWriter _dataWriter;
        private DataReader _dataReader;


        public Client(String RemoteHost, String LocalHost, String ServiceName)
        {
            _socket = new StreamSocket();
            _localHost = new HostName("192.168.1.113");
            _remoteHost = new HostName("192.168.1.145");
            _serviceName = "1337";
            _endpointpair = new EndpointPair(_localHost, _serviceName, _remoteHost, _serviceName);
            
        }

        public event IClient.DisconnectedHandler Disconnected;

        public event IClient.MessageReceivedHandler MessageReceived;

        public event IClient.MessageSubmittedHandler MessageSubmitted;

        async public void StartClient()
        {
            await _socket.ConnectAsync(_endpointpair);
        }

        public void StartClient(string Hostname, string Port)
        {
            throw new NotImplementedException();
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }

        public async void Receive()
        {
            StreamSocketListener Listener = new StreamSocketListener();
            _dataReader = new DataReader(_socket.InputStream);
            _dataReader.InputStreamOptions = InputStreamOptions.Partial;
            string msg = string.Empty;
            var count = await _dataReader.LoadAsync(8192);

            if (count > 0)
                msg = _dataReader.ReadString(count);

            _dataReader.DetachStream();
            _dataReader.Dispose();
            AsyncCallback retuner = new AsyncCallback(returnmessage);
            
            
        }

        public async void Send(PDU msg, bool close)
        {
            var _dataWriter = new DataWriter(_socket.OutputStream);
            _dataWriter.WriteString(msg.ToJson());
            var ret = await _dataWriter.StoreAsync();

            if (close)
            {
                _dataWriter.DetachStream();
                Close();
            }
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        void returnmessage(IAsyncResult a)
        {
            throw new NotImplementedException();
        }
    }
}
