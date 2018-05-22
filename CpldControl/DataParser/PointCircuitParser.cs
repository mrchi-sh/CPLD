using System.Collections.Generic;
using System.Linq;
using CpldBase.DataStructure;

namespace CpldControl.DataParser
{
    class PointCircuitParser:CommonParser
    {
        public static void SearchPointParse(byte[] resultData, out List<string> pointList)
        {
            TagCommandResult commandResult = null;
            TagShortCircuitInfo shortCircuitInfo = null;

            ParseCommandResult(resultData.Take(8).ToArray(), out commandResult);

            ParseShortCircuitInfo(resultData.Skip(8).ToArray(), out shortCircuitInfo);

            FormatPointCircuitInfo(shortCircuitInfo, out pointList);
        }
    }
}
