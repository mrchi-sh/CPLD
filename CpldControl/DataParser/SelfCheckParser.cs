using System.Collections.Generic;
using System.Linq;
using CpldBase.DataStructure;


namespace CpldControl.DataParser
{
    public class SelfCheckParser:CommonParser
    {
        public static void SelfCheckParse(byte[] resultData, out List<List<string>> shortCircuitInfoResult, out int pointNo, out int circuitNo)
        {
            TagCommandResult commandResult = null;
            TagShortCircuitInfo shortCircuitInfo = null;
            shortCircuitInfoResult = new List<List<string>>();

            ParseCommandResult(resultData.Take(8).ToArray(), out commandResult);

            ParseShortCircuitInfo(resultData.Skip(8).ToArray(), out shortCircuitInfo);

            circuitNo = U8ToU16(shortCircuitInfo.u16_short_circuit_group_count[1], shortCircuitInfo.u16_short_circuit_group_count[0]);          
            pointNo = U8ToU16(shortCircuitInfo.u16_short_lines_count[1], shortCircuitInfo.u16_short_lines_count[0]);

            FormatShortCircuitInfo(shortCircuitInfo, out shortCircuitInfoResult);
        }
    }
}
