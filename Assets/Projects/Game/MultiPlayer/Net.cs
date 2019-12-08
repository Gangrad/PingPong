using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using CsExtensions;

namespace Game.MultiPlayer {
    public class NetReceiver {
        private const int BufferSize = 1024;
        private readonly int _portNum;
        private Socket _socket;
        private Thread _thread;
        private bool _active;
        public readonly Signal<byte[], int, IPAddress> OnReceiveData = new Signal<byte[], int, IPAddress>();

        public NetReceiver(int portNum) {
            _portNum = portNum;
        }
        
        public void Run(AddressFamily addressFamily) {
            _socket = new Socket(addressFamily, SocketType.Dgram, ProtocolType.Udp);
            _active = true;
            _thread = new Thread(Listen);
            _thread.Start();
        }

        public void Stop() {
            if (_socket != null) {
                _socket.Close();
                _socket = null;
            }
            _active = false;
        }

        private void Listen() {
            EndPoint endPoint = new IPEndPoint(IPAddress.Any, _portNum);
            _socket.Bind(endPoint);
            var buffer = new byte[BufferSize];
            while (_socket != null && _socket.IsBound && _active) {
                try {
                    var received = _socket.ReceiveFrom(buffer, BufferSize, SocketFlags.None, ref endPoint);
                    OnReceiveData.Dispatch(buffer, received, ((IPEndPoint) endPoint).Address);
                }
                catch (SocketException e) {
                    if (_active) {
                        UnityEngine.Debug.LogErrorFormat(e.ToString());
                        Stop();
                    }
                    break;
                }
                catch (Exception e) {
                    UnityEngine.Debug.LogError(e.ToString());
                    break;
                }
            }
            ThreadUtils.SafeAbortAndRemove(ref _thread);
        }
    }

    public class NetSender {
        private readonly Socket _socket;
        private readonly EndPoint _endPoint;

        public NetSender(IPAddress address, int portNum) {
            _socket = new Socket(address.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            _endPoint = new IPEndPoint(address, portNum);
        }
        
        public void Send(byte[] data) {
            _socket.SendTo(data, _endPoint);
        }
    }
}