using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using CpldDB;
using Colors = System.Windows.Media.Colors;

namespace CpldUI.Check.Base
{
    public class Circuit
    {
        public CircuitInfo Info { get; set; }
        public Canvas CircuitCanvas { get; set; }
        public ObservableCollection<Dot> CircuitDots { get; set; }  //回路点列表
        public ObservableCollection<LineInfo> CircuitLines { get; set; }  //回路线列表
        
        public Circuit(CircuitInfo circuit)
        {
            if (circuit == null)
            {
                CpldLog.LogControl.LogError("创建回路失败，无效的回路信息");
                return;
            }

            CircuitCanvas = new Canvas();
            
            Info = circuit;
            if (Info.LineStyle == null)
            {
                Info.LineStyle = new CpldDB.Style
                {
                    Type = 1,
                    Size = 1,
                    ForegroundColor = Colors.Black,
                    BackgroundColor = Colors.Transparent
                };
            }           
            if (circuit.DotStyle == null)
            {
                Info.DotStyle = new CpldDB.Style
                {
                    Type = 1,
                    Size = 5,
                    ForegroundColor = Colors.White,
                    BackgroundColor = Colors.White
                };
            }         
            

            CircuitDots = new ObservableCollection<Dot>();
            CircuitLines = new ObservableCollection<LineInfo>();
        }

        public void MoveDotToCircuit(Circuit srcCircuit, Dot curDot, double scale)
        {
            CircuitDots.Add(curDot);
            GetAllLines(scale);
          
            curDot.InCircuit = this;
            curDot.Info.ParentCircuitId = Info.CircuitId;

            srcCircuit.CircuitDots.Remove(curDot);
            srcCircuit.GetAllLines(scale);
        }

        public void ScaleChange(double scale)
        {
            CircuitCanvas.Children.Clear();
            foreach (var line in CircuitLines)
            {
                var lineCanvas = new Line
                {
                    X1 = line.PositionStart.X * scale,
                    Y1 = line.PositionStart.Y * scale,
                    X2 = line.PositionEnd.X * scale,
                    Y2 = line.PositionEnd.Y * scale,
                    StrokeThickness = Info.LineStyle.Size,
                    Stroke = new SolidColorBrush(Info.LineStyle.ForegroundColor)
                };

                switch (Info.LineStyle.Type)
                {
                    case 1:
                        lineCanvas.StrokeDashArray = new DoubleCollection() { 5, 5 };
                        break;
                    case 2:
                        lineCanvas.StrokeDashArray = new DoubleCollection() { 1, 0 };
                        break;
                    case 3:
                        lineCanvas.StrokeDashArray = new DoubleCollection() { 5, 2, 1, 2 };
                        break;
                    default:
                        lineCanvas.StrokeDashArray = new DoubleCollection() { 5, 5 };
                        break;
                }
                CircuitCanvas.Children.Add(lineCanvas);
            }
        }

        public void GetAllLines(double scale)
        {
            if (CircuitDots.Count <= 1 || Info.LineStyle.ForegroundColor == Colors.Transparent || Info.CircuitId == 0)
            {
                CircuitCanvas.Children.Clear();
                return;
            }

            //生成所有无向边
            var allLines = new List<LineInfo>();
            for (var i = 0; i < CircuitDots.Count - 1; i++)
            {
                for (var j = i + 1; j < CircuitDots.Count; j++)
                {
                    var line = new LineInfo
                    {
                        PositionStart = new Point(CircuitDots[i].Info.Position.X, CircuitDots[i].Info.Position.Y),
                        PositionEnd = new Point(CircuitDots[j].Info.Position.X, CircuitDots[j].Info.Position.Y),
                      
                        StartPointIndex = i,
                        EndPointIndex = j
                    };
                    line.LineLength = Math.Sqrt((line.PositionEnd.X - line.PositionStart.X) * (line.PositionEnd.X - line.PositionStart.X) +
                                                (line.PositionEnd.Y - line.PositionStart.Y) * (line.PositionEnd.Y - line.PositionStart.Y));
                    allLines.Add(line);
                }
            }
            //排序
            allLines.Sort((a, b) => (a.LineLength >= b.LineLength) ? 1 : -1);

            //克鲁斯卡尔最小生成树
            CircuitLines.Clear();
            var labels = new int[CircuitDots.Count];
            foreach (var line in allLines)
            {
                var start = FindCircle(labels, line.StartPointIndex);       //找到这个起点最远连通到哪个点
                var end = FindCircle(labels, line.EndPointIndex);           //找到这个终点最远连通到哪个顶点
                if (start == end) continue;
                labels[start] = end;
                CircuitLines.Add(line);               
            }
            allLines.Clear();
            ScaleChange(scale);
        }

        public void FlushCircuitStyle(double scale)
        {
            foreach (var dot in CircuitDots)
            {
                dot.FlushDotStyle(Info.DotStyle, scale);
            }
            ScaleChange(scale);
        }

        public void FlushCircuitStyle(CircuitInfo info, double scale)
        {
            Info.LineStyle.Size = info.LineStyle.Size;
            Info.LineStyle.Type = info.LineStyle.Type;
            Info.LineStyle.ForegroundColor = info.LineStyle.ForegroundColor;
            Info.LineStyle.BackgroundColor = info.LineStyle.BackgroundColor;

            Info.DotStyle.Size = info.DotStyle.Size;
            Info.DotStyle.Type = info.DotStyle.Type;
            Info.DotStyle.ForegroundColor = info.DotStyle.ForegroundColor;
            Info.DotStyle.BackgroundColor = info.DotStyle.BackgroundColor;
            foreach (var dot in CircuitDots)
            {
                dot.FlushDotStyle(Info.DotStyle, scale);
            }
            ScaleChange(scale);
        }

        public void DestroyCircuit()
        {


        }

        public void RemoveDot(Dot dot)
        {
            CircuitDots.Remove(dot);
        }

        private static int FindCircle(IList<int> m, int a)
        {
            while (m[a] > 0)
            {
                a = m[a];
            }
            return a;
        }

    }

    
}
