using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using CpldLog;

namespace CpldDB
{
    public class CableInfo
    {
        public int CableId { get; set; }
        public string CableName { get; set; }
        public string Remark { get; set; }
        public string ModifyDate { get; set; }
        public string CreateUser { get; set; }
        public string BgImgPath { get; set; }
        public CheckSetting Settings { get; set; }

        public CableInfo()
        {
            CableId = CacheGraph.GetMaxCableId() + 1;
            CableName = "";
            Remark = "";
            ModifyDate = "";
            CreateUser = "";
            BgImgPath = "";
            Settings = new CheckSetting
            {
                AutoStart = false,
                OkAutoRelease = false,
                NgAutoRelease = false,
                OkTime = 0,
                NgTime = 0
            };
        }
        public CableInfo(CableInfo cable)
        {
            CableId = CacheGraph.GetMaxCableId() + 1;
            CableName = cable.CableName;
            Remark = cable.Remark;
            ModifyDate = cable.ModifyDate;
            CreateUser = cable.CreateUser;
            BgImgPath = cable.BgImgPath;
            Settings = new CheckSetting
            {
                AutoStart = cable.Settings.AutoStart,
                OkAutoRelease = cable.Settings.OkAutoRelease,
                NgAutoRelease = cable.Settings.NgAutoRelease,
                OkTime = cable.Settings.OkTime,
                NgTime = cable.Settings.NgTime
            };
        }
    }

    public class CheckSetting
    {
        public int OkTime { get; set; }
        public int NgTime { get; set; }
        public bool OkAutoRelease { get; set; }
        public bool NgAutoRelease { get; set; }
        public bool AutoStart { get; set; }
    }

    public class Style
    {
        public int Type { get; set; }                       //类型
        public double Size { get; set; }                    //尺寸

        public Color ForegroundColor { get; set; }          //前景色
        public Color BackgroundColor { get; set; }          //背景色
        public Style() 
		{
		 Type = 1;
                Size = 5;
                ForegroundColor = Colors.White;
                BackgroundColor = Colors.White;
		 }
        public Style(Style info) 
        {
            if (info == null)
            {
                Type = 1;
                Size = 5;
                ForegroundColor = Colors.White;
                BackgroundColor = Colors.White;
            }
            else 
            {
                Type = info.Type;
                Size = info.Size;
                ForegroundColor = info.ForegroundColor;
                BackgroundColor = info.BackgroundColor;
            }
            
        }
    }

    public class DotInfo
    {
        public int ParentCableId { get; set; }              //所属样线ID
        public int ParentPlugId { get; set; }               //所属组件ID
        public int ParentCircuitId { get; set; }            //所属回路ID
        public int DotId { get; set; }                      //点ID

        public string PhyAddr { get; set; }                 //物理地址（机器地址）
        public string Name { get; set; }                    //别名（用户使用）
        public Style DotStyle { get; set; }                 //样式

        public Point Position { get; set; }

        public DotInfo()
        {
           
            PhyAddr = string.Empty;
            Name = string.Empty;
            DotStyle = new Style
            {
                Type = 1,
                Size = 5,
                ForegroundColor = Colors.White,
                BackgroundColor = Colors.White
            };
            Position = new Point(0, 0);
        }
        public DotInfo(DotInfo info)
        {
            ParentCableId = info.ParentCableId;
            ParentCircuitId = info.ParentCircuitId;
            ParentPlugId = info.ParentPlugId;
            DotId = info.DotId;
            PhyAddr = info.PhyAddr;
            Name = info.Name;
            DotStyle = new Style
            {
                Type = info.DotStyle.Type,
                Size = info.DotStyle.Size,
                ForegroundColor = info.DotStyle.ForegroundColor,
                BackgroundColor = info.DotStyle.BackgroundColor
            };
            Position = new Point(info.Position.X,info.Position.Y);
        }
    }

    public class LineInfo
    {
        public Point PositionStart { get; set; }
        public Point PositionEnd { get; set; }
        public int StartPointIndex;
        public int EndPointIndex;
        public double LineLength;
    }

    public class CircuitInfo
    {
        public int ParentCableId { get; set; }
        public int CircuitId { get; set; }
        public string Name { get; set; }
        public string Remark { get; set; }
        public Style LineStyle { get; set; }
        public Style DotStyle { get; set; }

        public CircuitInfo()
        {
            Remark = string.Empty;
            LineStyle = new Style
            {
                Type = 1,
                Size = 1,
                ForegroundColor = Colors.Black,
                BackgroundColor = Colors.Transparent
            };
            DotStyle = new Style
            {
                Type = 1,
                Size = 5,
                ForegroundColor = Colors.White,
                BackgroundColor = Colors.White
            };
        }
        public CircuitInfo(CircuitInfo info)
        {
            ParentCableId = info.ParentCableId;
            CircuitId = info.CircuitId;
            Name = info.Name;
            Remark = info.Remark;
            LineStyle = new Style
            {
                Type = info.LineStyle.Type,
                Size = info.LineStyle.Size,
                ForegroundColor = info.LineStyle.ForegroundColor,
                BackgroundColor = info.LineStyle.BackgroundColor
            };
            DotStyle = new Style
            {
                Type = info.DotStyle.Type,
                Size = info.DotStyle.Size,
                ForegroundColor = info.DotStyle.ForegroundColor,
                BackgroundColor = info.DotStyle.BackgroundColor
            };
        }

    }


    public class DbGraph : DbCommon
    {
        #region Cable
        public static bool SelectCables(out List<CableInfo> cablesInfos)
        {
            const string querySql = "SELECT id,product_name,remark,create_date,create_user,graph_path,auto_start,ok_auto_release,ng_auto_release,ng_time,ok_time FROM sample_info";
            DataTable dt;
            cablesInfos = new List<CableInfo>();

            if (!QueryData(querySql, out dt))
                return false;

            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var tmpCableInfo = new CableInfo
                {
                    CableId = Convert.ToInt32(dt.Rows[i][0].ToString()),
                    CableName = dt.Rows[i][1].ToString(),
                    Remark = dt.Rows[i][2].ToString(),
                    ModifyDate = dt.Rows[i][3].ToString(),
                    CreateUser = dt.Rows[i][4].ToString(),
                    BgImgPath = dt.Rows[i][5].ToString(),
                    Settings = new CheckSetting
                    {
                        AutoStart = Convert.ToBoolean(dt.Rows[i][6]),
                        OkAutoRelease = Convert.ToBoolean(dt.Rows[i][7]),
                        NgAutoRelease = Convert.ToBoolean(dt.Rows[i][8]),

                        NgTime = Convert.ToInt32(dt.Rows[i][9].ToString()),
                        OkTime = Convert.ToInt32(dt.Rows[i][10].ToString())
                    }
                };

                cablesInfos.Add(tmpCableInfo);
            }
            return true;
        }

        public static bool InsertCables(List<CableInfo> cableInfos)
        {
            var querySql = cableInfos.Aggregate("", (current, cable) =>
                current + "INSERT INTO sample_info " +
                "(id, product_name, remark, create_date, create_user, graph_path,auto_start,ok_auto_release,ng_auto_release,ng_time,ok_time)" +
                " VALUES ("
                + cable.CableId + ","
                + "'" + cable.CableName + "'" + ","
                + "'" + cable.Remark + "'" + ","
                + "'" + cable.ModifyDate + "'" + ","
                + "'" + cable.CreateUser + "'" + ","
                + "'" + cable.BgImgPath + "'" + ","
                + cable.Settings.AutoStart + ","
                + cable.Settings.OkAutoRelease + ","
                + cable.Settings.NgAutoRelease + ","
                + cable.Settings.NgTime + ","
                + cable.Settings.OkTime + ");");
            
            return NonQueryData(querySql);
        }

        public static bool DelCables(List<CableInfo> cableInfos)
        {
            var querySql = cableInfos.Aggregate("", (current, cable) =>
                current + "DELETE FROM sample_info WHERE id=" + cable.CableId + ";");
            return NonQueryData(querySql);
        }

        public static bool UpdateCables(List<CableInfo> cablesInfos)
        {
            var querySql = cablesInfos.Aggregate("", (current, cable) => 
                current + "UPDATE sample_info SET "
                +"product_name='"+cable.CableName+"',"
                +"remark='"+cable.Remark+"',"
                +"create_date='"+cable.ModifyDate+"',"
                +"create_user='"+cable.CreateUser+"',"
                +"graph_path='"+@cable.BgImgPath+"',"
                + "auto_start=" + cable.Settings.AutoStart + ","
                +"ok_auto_release="+cable.Settings.OkAutoRelease+","
                +"ng_auto_release="+cable.Settings.NgAutoRelease+","
                +"ng_time="+cable.Settings.NgTime+","
                +"ok_time="+cable.Settings.OkTime+" "
                +"WHERE id="+cable.CableId+";");

            return NonQueryData(querySql);
        }
        #endregion
        
        #region Circuit
        public static bool SelectCircuits(out List<CircuitInfo> circuits)
        {
            const string querySql = "SELECT * FROM circuits";
            DataTable dt;
            circuits = new List<CircuitInfo>();

            if (!QueryData(querySql, out dt))
                return false;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var tmpCircuitInfo = new CircuitInfo
                {
                    ParentCableId = Convert.ToInt32(dt.Rows[i][0].ToString()),
                    CircuitId = Convert.ToInt32(dt.Rows[i][1].ToString()),
                    Name = dt.Rows[i][2].ToString(),
                    Remark = dt.Rows[i][3].ToString(),
                    LineStyle = new Style
                    {
                        Type = Convert.ToInt32(dt.Rows[i][4].ToString()),
                        Size = Convert.ToInt32(dt.Rows[i][5].ToString()),
                        ForegroundColor = (Color)ColorConverter.ConvertFromString(dt.Rows[i][6].ToString()),
                        BackgroundColor = Colors.Transparent
                    },
                    DotStyle = new Style
                    {
                        Type = Convert.ToInt32(dt.Rows[i][7].ToString()),
                        Size = Convert.ToInt32(dt.Rows[i][8].ToString()),
                        ForegroundColor = (Color)ColorConverter.ConvertFromString(dt.Rows[i][9].ToString()),
                        BackgroundColor = (Color)ColorConverter.ConvertFromString(dt.Rows[i][10].ToString())
                    }
                };
                circuits.Add(tmpCircuitInfo);
            }

            return true;
        }
        
        public static bool InsertCircuits(List<CircuitInfo> circuits)
        {
            var querySql = circuits.Aggregate("", (current, circuit) => 
                current + ("INSERT INTO circuits (cable_id, circuit_id, name, remark, line_type, line_width, line_color,point_type,point_size,point_fg_color,point_bg_color) VALUES (" 
                + circuit.ParentCableId + "," 
                + circuit.CircuitId + "," 
                + "'" + circuit.Name + "'" + "," 
                + "'" + circuit.Remark + "'" + "," 
                + circuit.LineStyle.Type + "," 
                + circuit.LineStyle.Size + "," 
                + "'" + circuit.LineStyle.ForegroundColor + "'" 
                + "," + circuit.DotStyle.Type + "," 
                + circuit.DotStyle.Size + "," 
                + "'" + circuit.DotStyle.ForegroundColor + "'" 
                + "," + "'" + circuit.DotStyle.BackgroundColor + "'" + ");"));

            return NonQueryData(querySql);
        }
        
        public static bool DelCircuits(List<CircuitInfo> circuits)
        {
            var querySql = circuits.Aggregate("", (current, circuit) => 
                current + ("DELETE FROM circuits WHERE cable_id=" + circuit.ParentCableId + " AND circuit_id=" + circuit.CircuitId + ";"));
            return NonQueryData(querySql); 
        }

        public static bool UpdataCircuits(List<CircuitInfo> circuits)
        {
            var querySql = circuits.Aggregate("", (current, circuit) =>
                current + "UPDATE circuits SET "
                + "name='" + circuit.Name + "',"
                + "remark='" + circuit.Remark + "',"
                + "line_type=" + circuit.LineStyle.Type + ","
                + "line_width=" + circuit.LineStyle.Size + ","
                + "line_color='" + circuit.LineStyle.ForegroundColor + "',"
                + "point_type=" + circuit.DotStyle.Type + ","
                + "point_size=" + circuit.DotStyle.Size + ","
                + "point_fg_color='" + circuit.DotStyle.ForegroundColor + "',"
                + "point_bg_color='" + circuit.DotStyle.BackgroundColor + "' "
                + "WHERE cable_id=" + circuit.ParentCableId + " AND circuit_id=" + circuit.CircuitId + ";");
            return NonQueryData(querySql);
        }

        #endregion

        #region Dot
        public static bool SelectDots(out List<DotInfo> dots)
        {
            const string querySql = "SELECT * FROM points";
            DataTable dt;
            dots = new List<DotInfo>();

            if (!QueryData(querySql, out dt))
                return false;
            try
            {
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var tmpDotInfo = new DotInfo
                    {
                        ParentCableId = Convert.ToInt32(dt.Rows[i][0].ToString()),
                        DotId = Convert.ToInt32(dt.Rows[i][1].ToString()),
                        ParentPlugId= Convert.ToInt32(dt.Rows[i][2].ToString()),
                        ParentCircuitId = Convert.ToInt32(dt.Rows[i][3].ToString()),
                        PhyAddr = dt.Rows[i][4].ToString(),
                        Name = dt.Rows[i][5].ToString(),
                        DotStyle = new Style
                        {
                            Type = Convert.ToInt32(dt.Rows[i][6].ToString()),
                            Size = Convert.ToInt32(dt.Rows[i][7].ToString()),
                            ForegroundColor = (Color)ColorConverter.ConvertFromString(dt.Rows[i][8].ToString()),
                            BackgroundColor = (Color)ColorConverter.ConvertFromString(dt.Rows[i][9].ToString())
                        },
                        Position = new Point(Convert.ToInt32(dt.Rows[i][10].ToString()), Convert.ToInt32(dt.Rows[i][11].ToString()))
                    };
                    dots.Add(tmpDotInfo);
                }
            }
            catch (Exception e)
            {
                LogControl.LogError(e);
            }
            return true;
        }
        
        public static bool InsertDots(List<DotInfo> dots)
        {
            var querySql = dots.Aggregate("", (current, dot) => current + 
                ("INSERT INTO points (cable_id, point_id, plug_id, circuit_id, phy_addr, name, type, size, fg_color, bg_color, zoomX, zoomY) VALUES (" 
                + dot.ParentCableId + "," 
                + dot.DotId + "," 
                + dot.ParentPlugId + "," 
                + dot.ParentCircuitId + "," + "'" 
                + dot.PhyAddr + "'" + "," + "'" 
                + dot.Name + "'" + "," 
                + dot.DotStyle.Type + "," 
                + dot.DotStyle.Size + "," + "'" 
                + dot.DotStyle.ForegroundColor + "'" + "," + "'" 
                + dot.DotStyle.BackgroundColor + "'" + "," 
                + dot.Position.X + "," 
                + dot.Position.Y + ");"));

            return NonQueryData(querySql);
        }
        
        public static bool DelDots(List<DotInfo> dots)
        {
            var querySql = dots.Aggregate("", (current, dot) =>
                current + "DELETE FROM points WHERE cable_id=" + dot.ParentCableId + " AND point_id=" + dot.DotId +";");
            return NonQueryData(querySql);
        }
        
        public static bool UpdataDots(List<DotInfo> dots)
        {
            var querySql = dots.Aggregate("", (current, dot) =>
                current + "UPDATE points SET " +
                "plug_id=" + dot.ParentPlugId + "," +
                "circuit_id=" + dot.ParentCircuitId + "," +
                "phy_addr=" + "'" + dot.PhyAddr + "'" + "," +
                "name=" + "'" + dot.Name + "'" + "," +
                "type=" + dot.DotStyle.Type + "," +
                "size=" + dot.DotStyle.Size + "," +
                "fg_color=" + "'" + dot.DotStyle.ForegroundColor + "'" + "," +
                "bg_color=" + "'" + dot.DotStyle.BackgroundColor + "'" + "," +
                "zoomX=" + dot.Position.X + "," +
                "zoomY=" + dot.Position.Y +
                " WHERE " +
                "point_id=" + dot.DotId +
                " AND " +
                "cable_id=" + dot.ParentCableId + ";");
            return NonQueryData(querySql);
        }
        #endregion



    }
}
