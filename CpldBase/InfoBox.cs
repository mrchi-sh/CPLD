using System.Collections;
using System.Collections.Generic;
using System.Windows;


namespace CpldBase
{
    public class InfoBox
    {
        public static void ErrorMsg(string errorMsg)
        {
            MessageBox.Show(errorMsg, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void InfoMsg(string infoMsg)
        {
            MessageBox.Show(infoMsg, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void WarnMsg(string warnMsg)
        {
            MessageBox.Show(warnMsg, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static void RichTextMsg(out string resultString, bool isCheckOk, int pointNo, int circuitNo, List<List<string>> shortCircuitList, List<string[]> openCircuitList, Hashtable aliasHt)
        {
            var shortCircuitIndex = 0;
            var shortCircuit = "";
            var openCircuit = "";
            var openCircuitIndex = 0;

            resultString = "";

            var checkStatus = isCheckOk ? "OK" : "NG";

            foreach (var tmpList in shortCircuitList)
            {
                ++shortCircuitIndex;
                var tmpResult = "";
                foreach (var tmp in tmpList)
                {
                    var aliasTmp = aliasHt[tmp] as string;
                    if (aliasTmp == null || aliasTmp.Equals(""))
                        aliasTmp = tmp;
                    tmpResult += aliasTmp + "-";
                }
                shortCircuit += shortCircuitIndex + ": " + tmpResult.Remove(tmpResult.Length - 1) + "\r\n";
            }

            foreach (var circuit in openCircuitList)
            {
                var aliasTmp0 = aliasHt[circuit[0]] as string;
                var aliasTmp1 = aliasHt[circuit[1]] as string;

                if (aliasTmp0 != null && !aliasTmp0.Equals(""))
                {
                    circuit[0] = aliasTmp0;
                }

                if (aliasTmp1 != null && !aliasTmp1.Equals(""))
                {
                    circuit[1] = aliasTmp1;
                }
            }

            if (openCircuitList.Count > 0)
            {
                var tmpList = new List<string> { openCircuitList[0][0] + "/" + openCircuitList[0][1] };
                for (var i = 1; i < openCircuitList.Count; i++)
                {
                    if (tmpList[openCircuitIndex].Contains(openCircuitList[i][0]) || tmpList[openCircuitIndex].Contains(openCircuitList[i][1]))
                    {
                        tmpList[openCircuitIndex] = openCircuitList[i][0] + "/" + openCircuitList[i][1];
                    }
                    else
                    {
                        tmpList.Add(openCircuitList[i][0] + "/" + openCircuitList[i][1]);
                        openCircuitIndex++;
                    }
                }

                openCircuitIndex = 0;
                foreach (var tmp in tmpList)
                {
                    ++openCircuitIndex;
                    openCircuit += openCircuitIndex + ": " + tmp + "\r\n";
                }
            }

            resultString = "检测结果：" + checkStatus + "\r\n\r\n";
            if (shortCircuit != "")
                resultString += "短路：\r\n" + shortCircuit + "\r\n";
            if (openCircuit != "")
                resultString += "断路：\r\n" + openCircuit;
        }


        public static void PlaySound(bool isOk)
        {
            var soundPath = "";
            soundPath = isOk ? "Sound/ok.wav" : "Sound/ng.wav";
            var player = new System.Media.SoundPlayer(soundPath);
            player.Play();
        }

    }
}
