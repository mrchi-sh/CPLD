using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using CpldBase;
using CpldLog;

namespace CpldSocket
{
    public delegate void DelGetReceiveData(List<byte> recvList);

    internal class StateObject
    {
        internal Socket WorkSocket = null;
        internal const int BufferSize = 1024;
        internal byte[] Buffer = new byte[BufferSize];
        internal int RecvLen = 0;
    }

    public class CpldSocket
    {
        private static readonly int RemotePort = Params.ServerPort;
        private static string _remoteHost = Params.ServerHost;
        private static Socket _cpldSocket;
        private static readonly AutoResetEvent ConnectDone = new AutoResetEvent(false);


        public static int ReceiveTimeout = 600000;

        public static bool CheckConnectionState()
        {
            return _cpldSocket != null && _cpldSocket.Connected;
        }

        public static void Connect()
        {
            try
            {
                _remoteHost = Params.ServerHost;
                var ipAddress = IPAddress.Parse(_remoteHost);
                var remoteEp = new IPEndPoint(ipAddress, RemotePort);
                _cpldSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _cpldSocket.BeginConnect(remoteEp, ConnectCallBack, _cpldSocket);
                if (!ConnectDone.WaitOne(1000, false))                              //5 sec timeout
                {
                    LogControl.LogError("connect time out");
                }
            }
            catch (Exception ex)
            {
                LogControl.LogError("connect error " + ex);
            }
        }

        public static void Reconnect()
        {
            try
            {
                if (_cpldSocket.Connected)
                {
                    _cpldSocket.Shutdown(SocketShutdown.Both);
                    _cpldSocket.Disconnect(true);
                    //cpldSocket.Connected = false;
                    _cpldSocket.Close();
                }
                _cpldSocket = null;
            }
            catch (Exception ex)
            {
                LogControl.LogError("reconnect error " + ex);
            }
            Connect();
        }

        private static void ConnectCallBack(IAsyncResult ar)
        {
            try
            {
                if (_cpldSocket == null || !_cpldSocket.Connected)
                    return;

                var client = (Socket)ar.AsyncState;
                client.EndConnect(ar);
                ConnectDone.Set();
            }
            catch (Exception ex)
            {
                LogControl.LogError(ex.ToString());
            }
        }

        public static void Disconnected()
        {
            try
            {
                if (_cpldSocket == null) return;
                _cpldSocket.Shutdown(SocketShutdown.Both);
                Thread.Sleep(10);
                _cpldSocket.Close();

                LogControl.LogError("receive timeout");
            }
            catch (Exception ex)
            {
                LogControl.LogError(ex.ToString());
            }

        }

        public static bool Receive(out List<byte> recvList)
        {
            recvList = new List<byte>();
            if (_cpldSocket == null)
                return false;
            if (_cpldSocket.Connected == false)
                return false;

            try
            {
                _cpldSocket.ReceiveTimeout = ReceiveTimeout;

                var dataTotalLen = -1;
                var recvLoopCount = 0;
                do
                {
                    var recvData = new byte[1024];
                    var recvLen = _cpldSocket.Receive(recvData);
                    recvLoopCount++;

                    var tmp = new byte[recvLen];
                    Array.ConstrainedCopy(recvData, 0, tmp, 0, recvLen);
                    recvList.AddRange(tmp);
                    dataTotalLen = Convert.ToInt32(
                        Convert.ToString(recvList[5], 16).PadLeft(2, '0') +
                        Convert.ToString(recvList[4], 16).PadLeft(2, '0'), 16);

                } while (recvList.Count < dataTotalLen + 8 || recvLoopCount > 100);
                if (recvLoopCount <= 100) return true;
                LogControl.LogError("Loop timeout");
                return false;
            }
            catch (Exception ex)
            {
                LogControl.LogError(ex);
                return false;
            }
        }

        public static bool Send(byte[] data)
        {
            try
            {
                if (_cpldSocket == null)
                {
                    Connect();
                }
                else if (_cpldSocket.Connected == false)
                {
                    Connect();
                }


                if (_cpldSocket == null || !_cpldSocket.Connected) return false;
                _cpldSocket.SendTimeout = 3000;
                _cpldSocket.Send(data);
                return true;
            }
            catch (TimeoutException ex)
            {
                LogControl.LogError(ex);
                Disconnected();
                return false;
            }
            catch (Exception ex)
            {
                LogControl.LogError(ex);
                Disconnected();
                return false;
            }

        }
    }
}
