using System;
using System.Net;
using CsExtensions;
using DataHelper;
using Game.Gameplay;
using Game.Utils;
using Unity.Utils;
using UnityEngine;

namespace Game.MultiPlayer {
    public class MultiPlayerNetService {
        private const int PortNum = 10308;
        private NetReceiver _receiver;
        private NetSender _sender;
        private bool _connected;
        private readonly DisposableCollection _disposableCollection = new DisposableCollection();
        private readonly ActionsQueue _mainThreadActionsQueue;

        public readonly Signal<bool> OnStartGame = new Signal<bool>();
        public readonly Signal<float> OnPlatformMoved = new Signal<float>();
        public readonly Signal<Vector2> OnBallMoved = new Signal<Vector2>();
        public readonly Signal<GameParams> OnGameParamsReceived = new Signal<GameParams>();
        public readonly Signal<Side> OnGameOverReceived = new Signal<Side>();
        
        public MultiPlayerNetService(ActionsQueue mainThreadActionsQueue) {
            _mainThreadActionsQueue = mainThreadActionsQueue;
        } 

        public void StartHost() {
            _connected = false;
            StartReceiver(OnHostReceiveData, PortNum);
            Debug.Log("Host started");
        }

        private void OnHostReceiveData(byte[] data, int length, IPAddress address) {
            var reader = new ByteReader(data);
            var msgType = (MsgType) reader.ReadByte();
            switch (msgType) {
                case MsgType.Handshake:
                    OnHostReceivedHandshake(address);
                    return;
                case MsgType.PlatformMoved:
                    OnReceivePlatformPosition(reader);
                    return;
                case MsgType.BallMoved:
                    OnReceiveBallPositions(reader);
                    return;
                case MsgType.StartParams:
                case MsgType.GameOver:
                    Debug.LogErrorFormat("Unexpected data type received by host: {0}", msgType.ToString());
                    return;
                default:
                    throw new ArgumentException();
            }
        }

        private void OnHostReceivedHandshake(IPAddress address) {
            CreateSenderAndSendHandshake(address);
            _connected = true;
            Debug.LogFormat("Connected");
            _mainThreadActionsQueue.Enqueue(() => OnStartGame.Dispatch(true));
        }

        public void StartConnected(IPAddress address) {
            Debug.LogFormat("Connect to {0}", address);
            _connected = false;
            StartReceiver(OnConnectedReceiveData, PortNum);
            CreateSenderAndSendHandshake(address);
        }

        private void CreateSenderAndSendHandshake(IPAddress address) {
            _sender = new NetSender(address, PortNum);
            var msg = new []{(byte) MsgType.Handshake};
            _sender.Send(msg);
        }

        private void StartReceiver(Action<byte[], int, IPAddress> handle, int portNum) {
            if (_receiver != null)
                _receiver.Stop();
            _receiver = new NetReceiver(portNum);
            _receiver.OnReceiveData.Subscribe(handle).DisposeBy(_disposableCollection);
            _receiver.Run(NetUtils.AnyAddress);
        }

        private void OnConnectedReceiveData(byte[] data, int length, IPAddress address) {
            var reader = new ByteReader(data);
            var msgType = (MsgType) reader.ReadByte();
            switch (msgType) {
                case MsgType.Handshake:
                    OnConnectedReceivedHandshake();
                    return;
                case MsgType.StartParams:
                    OnReceiveGameParams(reader);
                    return;
                case MsgType.PlatformMoved:
                    OnReceivePlatformPosition(reader);
                    return;
                case MsgType.BallMoved:
                    OnReceiveBallPositions(reader);
                    return;
                case MsgType.GameOver:
                    OnReceiveGameOver(reader);
                    return;
                default:
                    throw new ArgumentException();
            }
        }

        private void OnConnectedReceivedHandshake() {
            _connected = true;
            Debug.LogFormat("Connected");
            OnStartGame.Dispatch(false);
        }

        public void SendGameParams(GameParams gameParams) {
            if (!_connected)
                return;
            var bw = new ByteWriter(25)
                .WriteByte((byte) MsgType.StartParams)
                .WriteFloat(gameParams.BallSize)
                .WriteColor(gameParams.BallColor)
                .WriteVector2(gameParams.StartForce);
            _sender.Send(bw.Data);
        }

        private void OnReceiveGameParams(ByteReader reader) {
            var gameParams = new GameParams {
                BallSize = reader.ReadFloat(),
                BallColor = reader.ReadColor(),
                StartForce = reader.ReadVector2()
            };
            _mainThreadActionsQueue.Enqueue(() => OnGameParamsReceived.Dispatch(gameParams));
        }

        public void SendPlatformPosition(float xPos) {
            if (!_connected)
                return;
            var bw = new ByteWriter(5);
            bw.WriteByte((byte) MsgType.PlatformMoved);
            bw.WriteFloat(xPos);
            _sender.Send(bw.Data);
        }

        private void OnReceivePlatformPosition(ByteReader byteReader) {
            var pos = byteReader.ReadFloat();
            OnPlatformMoved.Dispatch(pos);
        }

        public void SendBallPositions(Vector2 pos) {
            if (!_connected)
                return;
            var bw = new ByteWriter(9);
            bw.WriteByte((byte) MsgType.BallMoved);
            bw.WriteFloat(pos.x);
            bw.WriteFloat(pos.y);
            _sender.Send(bw.Data);
        }

        private void OnReceiveBallPositions(ByteReader reader) {
            var x = reader.ReadFloat();
            var y = reader.ReadFloat();
            OnBallMoved.Dispatch(new Vector2(x, y));
        }

        public void SendGameOver(Side winner) {
            var data = new[] {(byte) MsgType.GameOver, (byte) winner};
            _sender.Send(data);
        }

        private void OnReceiveGameOver(ByteReader reader) {
            var winner = (Side) reader.ReadByte();
            _mainThreadActionsQueue.Enqueue(() => OnGameOverReceived.Dispatch(winner));
        }

        public void Close() {
            if (_receiver != null)
                _receiver.Stop();
        }
    }

    public enum MsgType : byte {
        Handshake = 18,
        StartParams,
        PlatformMoved,
        BallMoved,
        GameOver
    }
}