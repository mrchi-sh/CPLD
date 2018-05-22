using System;
using System.Collections.Generic;
using CpldLog;

namespace CpldControl.Check
{
    public class SampleCheck:CommonControl
    {
        public static bool BeginSampleCheck(out bool isNoShortCircuit, out  List<List<string>> shortCircuitInfoResult, out int pointNo, out int circuitNo)
        {
            ResultData = null;
            shortCircuitInfoResult = null;

            isNoShortCircuit = false;
            pointNo = 0;
            circuitNo = 0;

            var sendData = CommandData.GetCommandData(CommandType.SampleCheck);
            if (!SendCommand(sendData)) 
            { 
                LogControl.LogError("Send fail");
                return false;
            }
            if (!ReceiveResult())
            {
                LogControl.LogError("Receive Fail");
                return false;
            }

            if (ResultData == null || ResultData.Length < 8 )
            {
                LogControl.LogError("result null or resultdata length < 8");
                return false;
            }
            if (ResultData[0] != 0)
            {
                isNoShortCircuit = true;
                return true;
            }
          
            try
            {
                DataParser.SelfCheckParser.SelfCheckParse(ResultData, out shortCircuitInfoResult, out pointNo, out circuitNo);
                return true;
            }
            catch (Exception ex)
            {
                LogControl.LogError(ex);
                return false;
            }
        }

        public static bool ManualCheck(string sampleCircuitLoop, out int pointNo, out int circuitNo, out List<string[]> shortCircuitList, out List<string[]> openCircuitList)
        {
            bool isNoShortCircuit;
            List<List<string>> shortCircuitInfoResult;                               
            List<List<string>> sampleShortCircuitInfoResult;      

            pointNo = 0;
            circuitNo = 0;
            shortCircuitList = new List<string[]>();
            openCircuitList = new List<string[]>();

            if (!BeginSampleCheck(out isNoShortCircuit, out shortCircuitInfoResult, out pointNo, out circuitNo))
                return false;


            if (!isNoShortCircuit)
            {
                List<string[]> shortCircuitPointCombList;
                var arraySampleCircuitLoop = sampleCircuitLoop.Split(';');

                DataParser.CommonParser.CircuitToPointCombination(shortCircuitInfoResult, out shortCircuitPointCombList);
                ShortCircuitCheck(arraySampleCircuitLoop, shortCircuitPointCombList, out shortCircuitList);
            }

            DataParser.CommonParser.CircuitStringToList(sampleCircuitLoop, out sampleShortCircuitInfoResult);

            if (sampleShortCircuitInfoResult.Count == 0) return true;
            
            List<string[]> openCircuitPointCombList = null;
            DataParser.CommonParser.CircuitToPointCombination(sampleShortCircuitInfoResult, out openCircuitPointCombList);
            OpenCircuitCheck(shortCircuitInfoResult, openCircuitPointCombList, out openCircuitList);

            return true;
        }

        public static bool BeginPointCheck(out List<string> pointList)
        {
            ResultData = null;
            pointList = null;

            var sendData = CommandData.GetCommandData(CommandType.PointCheck);
            SendCommand(sendData);
            ReceiveResult();

            if (ResultData == null || ResultData.Length < 8)
            {
                LogControl.LogError("result null or resultdata length < 8");
                return false;
            }

            if (ResultData[0] != 0)
            {
                return false;
            }

            try
            {
                DataParser.PointCircuitParser.SearchPointParse(ResultData, out pointList);
                return true;
            }
            catch (Exception ex)
            {
                LogControl.LogError(ex);
                return false;
            }
        }
    }

}
