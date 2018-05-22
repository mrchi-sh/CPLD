using System.Collections.Generic;
using System.Threading;

namespace CpldControl.Check
{
    
    public class CommonControl
    {       
        protected static byte[] ResultData;

        public static void OpenConnection()
        {
            CpldSocket.CpldSocket.Connect();
        }

        public static void CloseConnection()
        {
            CpldSocket.CpldSocket.Disconnected();
        }

        public static bool CheckSocketState()
        {
            return CpldSocket.CpldSocket.CheckConnectionState();
        }

        protected static bool SendCommand(byte[] sendData)
        {
            return CpldSocket.CpldSocket.Send(sendData);
        }

        protected static bool ReceiveResult()
        {
            List<byte> recvList;
            if (!CpldSocket.CpldSocket.Receive(out recvList)) return false;
            ResultData = recvList.ToArray();
            return true;
        }

        protected static void ReceiveDataCallBack(List<byte> recvList)
        {
            ResultData = recvList.ToArray();
        }

        protected static void ShortCircuitCheck(string[] arraySampleCircuitLoop, List<string[]> shortCircuitPointCombList, out List<string[]> shortCircuitPointList)
        {
            shortCircuitPointList = new List<string[]>();
            var isShortCircuit = new bool[shortCircuitPointCombList.Count];
            for (var i = 0; i < isShortCircuit.Length; i++)
                isShortCircuit[i] = true;

            foreach (var tmpSampleCircuitLoop in arraySampleCircuitLoop)
            {
                var i = 0;
                foreach (var arrayShortCircuitPointComb in shortCircuitPointCombList)
                {
                    if (tmpSampleCircuitLoop.Contains(arrayShortCircuitPointComb[0]) && tmpSampleCircuitLoop.Contains(arrayShortCircuitPointComb[1]))
                        isShortCircuit[i] = false;
                    i++;
                }
            }

            for (var i = 0; i < isShortCircuit.Length; i++)
            {
                if(isShortCircuit[i])
                    shortCircuitPointList.Add(shortCircuitPointCombList[i]);
            }
        }

        protected static void OpenCircuitCheck(List<List<string>> circuitLoopList, List<string[]> openCircuitPointCombList, out List<string[]> openCircuitPointList)
        {
            openCircuitPointList = new List<string[]>();
            var isOpenCircuit = new bool[openCircuitPointCombList.Count];

            for (var i = 0; i < isOpenCircuit.Length; i++)
                isOpenCircuit[i] = true;

            if (circuitLoopList != null)
            {
                foreach (var tmpCircuitLoop in circuitLoopList)
                {
                    var i = 0;
                    foreach (var arrayOpenCircuitPointComb in openCircuitPointCombList)
                    {
                        if (tmpCircuitLoop.Contains(arrayOpenCircuitPointComb[0]) && tmpCircuitLoop.Contains(arrayOpenCircuitPointComb[1]))
                            isOpenCircuit[i] = false;
                        i++;
                    }
                }
            }
            for (var i = 0; i < isOpenCircuit.Length; i++)
            {
                if (isOpenCircuit[i])
                    openCircuitPointList.Add(openCircuitPointCombList[i]);
            }
        }

        public static bool LoadBoardCount(out int boardCount)
        {
            ResultData = null;
            boardCount = -1;
            var sendData = CommandData.GetCommandData(CommandType.BoardCount);
            SendCommand(sendData);
            ReceiveResult();

            if (ResultData == null || ResultData.Length < 8 || ResultData[0] != 1)
                return false;
            
            boardCount = ResultData[1];
            return true;
        }

        public static bool ControlRelay(CommandType type)
        {
            ResultData = null;

            var sendData = CommandData.GetCommandData(type);
            SendCommand(sendData);
            ReceiveResult();

            return ResultData != null && ResultData.Length >= 8 && ResultData[0] == 1;
        }
        
        public static bool SendOkRelay(int delay)
        {
            if (!ControlRelay(CommandType.OpenRelay))
                return false;
            Thread.Sleep(delay);
            return ControlRelay(CommandType.CloseRealy);
        }
    }
}
