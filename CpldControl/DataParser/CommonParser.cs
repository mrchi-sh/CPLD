using System.Collections;
using System.Linq;
using System.Collections.Generic;
using CpldBase.DataStructure;

namespace CpldControl.DataParser
{
    public class CommonParser 
    {
        protected const int CpldBoardCount = 32;
        protected const int CpldSingleLineCount = 128;
        protected const int LinesSampleNameLen = 12;

        protected static int U8ToU16(byte high, byte low)
        {
            return (high << 8) + low;
        }

        public static byte[] StructToByte(object data)
        {
            if (data.GetType() != typeof(TagCommandRequest)) return null;
            var tmpData = (TagCommandRequest)data;
            var tmpArr = new ArrayList {tmpData.u8_cmd_id, tmpData.u8_para1};

            foreach (var tmp in tmpData.u16_para2)
                tmpArr.Add(tmp);
            foreach (var tmp in tmpData.u16_para2)
                tmpArr.Add(tmp);
            foreach(var tmp in tmpData.u32_para3)
                tmpArr.Add(tmp);
            foreach (var tmp in tmpData.s_buffer)
                tmpArr.Add(tmp);

            var retData = (byte[])tmpArr.ToArray(typeof(byte));
            return retData;
        }

        protected static void ParseCommandResult(byte[] resultData, out TagCommandResult commandResult)
        {
            var resultDataQueue = new Queue();

            foreach (var t in resultData)
                resultDataQueue.Enqueue(t);

            //Obtain command result
            var crTmp = new TagCommandResult
            {
                u8_is_ok = (byte) resultDataQueue.Dequeue(),
                u8_result1 = (byte) resultDataQueue.Dequeue(),
                u16_result2 = new byte[2]
            };
            for (var i = 0; i < 2; i++)
                crTmp.u16_result2[i] = (byte)resultDataQueue.Dequeue();
            crTmp.u32_append_data_size = new byte[4];
            for (var i = 0; i < 4; i++)
                crTmp.u32_append_data_size[i] = (byte)resultDataQueue.Dequeue();

            commandResult = crTmp;
        }

        protected static void ParseShortCircuitInfo(byte[] resultData, out TagShortCircuitInfo shortCircuitInfo)
        {
            var resultDataQueue = new Queue();

            foreach (var t in resultData)
                resultDataQueue.Enqueue(t);

            var sciTmp = new TagShortCircuitInfo();
            sciTmp.u8_cpld_count = (byte)resultDataQueue.Dequeue();

            sciTmp.u8_reserved = new byte[3];
            for (var i = 0; i < 3; i++)
                sciTmp.u8_reserved[i] = (byte)resultDataQueue.Dequeue();

            sciTmp.u16_short_circuit_group_count = new byte[2];
            for (var i = 0; i < 2; i++)
                sciTmp.u16_short_circuit_group_count[i] = (byte)resultDataQueue.Dequeue();

            sciTmp.u16_short_lines_count = new byte[2];
            for (var i = 0; i < 2; i++)
                sciTmp.u16_short_lines_count[i] = (byte)resultDataQueue.Dequeue();

            sciTmp.ary_cpld_state = new TagBoardLinesState[32];
          
            for (var i = 0; i < sciTmp.u8_cpld_count; i++)          //using cpld board count
            {
                sciTmp.ary_cpld_state[i] = new TagBoardLinesState();
                sciTmp.ary_cpld_state[i].u8_board_addr = (byte)resultDataQueue.Dequeue();

                sciTmp.ary_cpld_state[i].u8_reserved = new byte[3];
                for (var j = 0; j < 3; j++)
                    sciTmp.ary_cpld_state[i].u8_reserved[j] = (byte)resultDataQueue.Dequeue();

                sciTmp.ary_cpld_state[i].ary_u8_checked_flag = new byte[CpldSingleLineCount / 8];
                for (var j = 0; j < CpldSingleLineCount / 8; j++)
                    sciTmp.ary_cpld_state[i].ary_u8_checked_flag[j] = (byte)resultDataQueue.Dequeue();

                sciTmp.ary_cpld_state[i].ary_short_circuit_info = new TagLineAddr[CpldSingleLineCount];
                for (var j = 0; j < CpldSingleLineCount; j++)
                {
                    sciTmp.ary_cpld_state[i].ary_short_circuit_info[j] =
                        new TagLineAddr
                        {
                            u8_board_addr = (byte) resultDataQueue.Dequeue(),
                            u8_index_on_board = (byte) resultDataQueue.Dequeue()
                        };
                }
            }
            shortCircuitInfo = sciTmp;
        }

        protected static void FormatShortCircuitInfo(TagShortCircuitInfo shortCircuitInfo, out List<List<string>> shortCircuitList)
        {
            var parsedDataList = new List<HashSet<string>>();
            for (var i = 0; i < shortCircuitInfo.u8_cpld_count; i++)
            {
                var srcBoardAddr = shortCircuitInfo.ary_cpld_state[i].u8_board_addr;

                for (var j = 0; j < CpldSingleLineCount; j++)
                {
                    var dstBoardAddr = shortCircuitInfo.ary_cpld_state[i].ary_short_circuit_info[j].u8_board_addr;
                    var dstIndex = shortCircuitInfo.ary_cpld_state[i].ary_short_circuit_info[j].u8_index_on_board;

                    if (dstBoardAddr == 0xff || dstIndex == 0xff) continue;

                    var srcPoint = "";
                    var dstPoint = "";

                    if (j < 64)
                        srcPoint = "A" + ((srcBoardAddr - 1) * 64 + j + 1);
                    else
                        srcPoint = "B" + ((srcBoardAddr - 1) * 64 + j - 63);

                    if (dstIndex < 64)
                        dstPoint = "A" + ((dstBoardAddr - 1) * 64 + dstIndex + 1);
                    else
                        dstPoint = "B" + ((dstBoardAddr - 1) * 64 + dstIndex - 63);

                    //Console.WriteLine(srcPoint + "->" + dstPoint);
                    //Console.WriteLine(srcBoardAddr + "," + j + "," + dstBoardAddr +","+ dstIndex);
                    

                    var isNewShortCircuit = true;

                    foreach (var tmpHs in parsedDataList)
                    {
                        if (!tmpHs.Contains(dstPoint) && !tmpHs.Contains(srcPoint)) continue;
                        tmpHs.Add(srcPoint);
                        tmpHs.Add(dstPoint);
                        isNewShortCircuit = false;
                        break;
                    }
                    if (!isNewShortCircuit) continue;
                    {
                        var tmpHs = new HashSet<string> {srcPoint, dstPoint};
                        parsedDataList.Add(tmpHs);
                    }
                }
            }

            var resultDataList = parsedDataList.Select(tmpHs => tmpHs.OrderBy(x => x).ToList()).ToList();
            shortCircuitList = resultDataList;
        }

        protected static void FormatPointCircuitInfo(TagShortCircuitInfo shortCircuitInfo, out List<string> pointList)//, out List<string> PointList)
        {
            pointList = new List<string>();
            
            for (var i = 0; i < shortCircuitInfo.u8_cpld_count; i++)
            {
                var srcBoardAddr = shortCircuitInfo.ary_cpld_state[i].u8_board_addr;

                for (var j = 0; j < CpldSingleLineCount; j++)
                {
                    var dstBoardAddr = shortCircuitInfo.ary_cpld_state[i].ary_short_circuit_info[j].u8_board_addr;
                    var dstIndex = shortCircuitInfo.ary_cpld_state[i].ary_short_circuit_info[j].u8_index_on_board;

                    if (dstBoardAddr == 0xff || dstIndex == 0xff || srcBoardAddr > dstBoardAddr || j == dstIndex ||
                        dstBoardAddr != 0xfe || dstIndex != 0xfe) continue;
                    
                    var srcPoint = "";
                    if (j < 64)
                        srcPoint = "A" + ((srcBoardAddr - 1) * 64 + j + 1);
                    else
                        srcPoint = "B" + ((srcBoardAddr - 1) * 64 + j - 63);
                    pointList.Add(srcPoint);
                }
            }                   
        }

        public static void PointToPair(List<string[]> openCircuitPointList, out List<List<string>> openCircuitList)
        {
            var openCircuitIndex = 0;
            openCircuitList = new List<List<string>>();

            if (openCircuitPointList.Count <= 0) return;
            var tmpList = new List<string> {openCircuitPointList[0][0] + "/" + openCircuitPointList[0][1]};
            for (var i = 1; i < openCircuitPointList.Count; i++)
            {
                if (tmpList[openCircuitIndex].Contains(openCircuitPointList[i][0]))
                {
                    tmpList[openCircuitIndex] = openCircuitPointList[i][0] + "/" + openCircuitPointList[i][1];
                }
                else
                {
                    tmpList.Add(openCircuitPointList[i][0] + "/" + openCircuitPointList[i][1]);
                    openCircuitIndex++;
                }
            }

            foreach (var tmp in tmpList)
            {
                var tmpResult = new List<string>();
                //  Console.WriteLine(tmp);
                var tmpArr = tmp.Split('/');

                tmpResult.Add(tmpArr[0]);
                tmpResult.Add(tmpArr[1]);

                openCircuitList.Add(tmpResult);
            }
        }

        public static void PointToCircuit(List<string[]> shortCircuitPointList, out List<List<string>> shortCircuitList)
        {
            var parsedDataList = new List<HashSet<string>>();
            var shortCircuitPointListCopy = shortCircuitPointList.ToList();
            while (shortCircuitPointListCopy.Count != 0)
            {
                var tmpHs = new HashSet<string>();
                for (var i = 0; i < shortCircuitPointListCopy.Count; )
                {
                    if (tmpHs.Count == 0 || tmpHs.Contains(shortCircuitPointListCopy[i][0]) || tmpHs.Contains(shortCircuitPointListCopy[i][1]))
                    {
                        tmpHs.Add(shortCircuitPointListCopy[i][0]);
                        tmpHs.Add(shortCircuitPointListCopy[i][1]);
                        shortCircuitPointListCopy.RemoveAt(i);
                        i = 0;
                    }
                    else
                    {
                        i++;
                    }
                }
                parsedDataList.Add(tmpHs);
            }
            shortCircuitList = parsedDataList.Select(tmp => tmp.OrderBy(x => x).ToList()).ToList();
        }

        public static void CircuitToPoint(List<List<string>> shortCircuitList, out List<string[]> shortCircuitPointList)
        {
            shortCircuitPointList = new List<string[]>();

            foreach (List<string> tmpCircuitList in shortCircuitList)
            {
                for (int i = 0; i < tmpCircuitList.Count - 1; i++)
                {
                    string[] tmpStr = new string[2];
                    tmpStr[0] = tmpCircuitList[i];
                    tmpStr[1] = tmpCircuitList[i + 1];
                    shortCircuitPointList.Add(tmpStr);
                }
            }
        }

        public static void CircuitToPointCombination(List<List<string>> shortCircuitList, out List<string[]> shortCircuitPointCombList)
        {
            shortCircuitPointCombList = new List<string[]>();

            foreach (var tmpCircuitList in shortCircuitList)
                for (var i = 0; i < tmpCircuitList.Count - 1; i++)
                    for (var j = i + 1; j < tmpCircuitList.Count; j++)
                    {
                        var tmp = new string[2];
                        tmp[0] = tmpCircuitList[i];
                        tmp[1] = tmpCircuitList[j];
                        shortCircuitPointCombList.Add(tmp);
                    }
        }

        public static void CircuitStringToList(string sampleShortCircuitString, out List<List<string>> sampleShortCircuitList)
        {
            var arrayStringList = sampleShortCircuitString.Split(';');
            sampleShortCircuitList = arrayStringList.Select(tmp1 => tmp1.Split('-')).Select(arrayString => arrayString.ToList()).ToList();
        }

    }
}
