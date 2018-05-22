using System;
using System.Collections.Generic;
using System.Linq;

namespace CpldDB
{
    public class CacheGraph : DbCommon
    {
        private static List<CableInfo> DbCables { get; set; }
        private static List<CableInfo> TmpCables { get; set; }

        private static List<CircuitInfo> DbCircuits { get; set; }
        private static List<CircuitInfo> TmpCircuits { get; set; }

        private static List<DotInfo> DbDots { get; set; }
        private static List<DotInfo> TmpDots { get; set; }

        public bool CacheInit()
        {
            List<CableInfo> cableList;
            DbGraph.SelectCables(out cableList);
            DbCables = cableList;
            TmpCables = new List<CableInfo>(cableList);

            List<CircuitInfo> circuitList;
            DbGraph.SelectCircuits(out circuitList);
            DbCircuits = circuitList;
            TmpCircuits = new List<CircuitInfo>(circuitList);

            List<DotInfo> dotList;
            DbGraph.SelectDots(out dotList);
            DbDots = dotList;
            TmpDots = new List<DotInfo>(dotList);

            return true;
        }

        public static bool CacheAddCable()
        {
            var updataCables = TmpCables.Intersect(DbCables).ToList();
            if (updataCables.Count > 0)
            {
                DbGraph.UpdateCables(updataCables);
            }
            var newCables = TmpCables.Except(DbCables).ToList();
            if (newCables.Count > 0)
            {
                DbCables.AddRange(newCables);
                DbGraph.InsertCables(newCables);
            }
            var newCircusts = TmpCircuits.Except(DbCircuits).ToList();
            if (newCircusts.Count > 0)
            {
                DbCircuits.AddRange(newCircusts);
                DbGraph.InsertCircuits(newCircusts);
            }
            return true;
        }

        public static bool CacheDelCable()
        {
            var delCables = DbCables.Except(TmpCables).ToList();
            if (delCables.Count > 0)
            {
                foreach (var cable in delCables)
                {
                    DbCables.Remove(cable);
                }
                DbGraph.DelCables(delCables);
            }
            var delCircuits = DbCircuits.Except(TmpCircuits).ToList();
            if (delCircuits.Count > 0)
            {
                foreach (var circuit in delCircuits)
                {
                    DbCircuits.Remove(circuit);
                }

                DbGraph.DelCircuits(delCircuits);
            }
            var delDots = DbDots.Except(TmpDots).ToList();
            if (delDots.Count > 0)
            {
                foreach (var dot in delDots)
                {
                    DbDots.Remove(dot);
                }
                DbGraph.DelDots(delDots);
            }
            return true;
        }

        public static bool CacheCopyCable()
        {
            var newCables = TmpCables.Except(DbCables).ToList();
            if (newCables.Count > 0)
            {
                DbCables.AddRange(newCables);
                DbGraph.InsertCables(newCables);
            }
            var newCircusts = TmpCircuits.Except(DbCircuits).ToList();
            if (newCircusts.Count > 0)
            {
                DbCircuits.AddRange(newCircusts);
                DbGraph.InsertCircuits(newCircusts);
            }
            var newDots = TmpDots.Except(DbDots).ToList();
            if (newDots.Count > 0)
            {
                DbDots.AddRange(newDots);
                DbGraph.InsertDots(newDots);
            }
            return true;
        }

        //更新所有样线记录
        public static bool CacheSaveCable()
        {
            //cable
            var updataCables = TmpCables.Intersect(DbCables).ToList();
            if (updataCables.Count > 0)
            {
                DbGraph.UpdateCables(updataCables);
            }
            var newCables = TmpCables.Except(DbCables).ToList();
            if (newCables.Count > 0)
            {
                DbCables.AddRange(newCables);
                DbGraph.InsertCables(newCables);
            }
            var delCables = DbCables.Except(TmpCables).ToList();
            if (delCables.Count > 0)
            {
                foreach (var cable in delCables)
                {
                    DbCables.Remove(cable);
                }
                DbGraph.DelCables(delCables);
            }
            //circuit
            var updataCircuits = TmpCircuits.Intersect(DbCircuits).ToList();
            if (updataCircuits.Count > 0)
            {
                DbGraph.UpdataCircuits(updataCircuits);
            }
            var newCircusts = TmpCircuits.Except(DbCircuits).ToList();
            if (newCircusts.Count > 0)
            {
                DbCircuits.AddRange(newCircusts);
                DbGraph.InsertCircuits(newCircusts);
            }
            var delCircuits = DbCircuits.Except(TmpCircuits).ToList();
            if (delCircuits.Count > 0)
            {
                foreach (var circuit in delCircuits)
                {
                    DbCircuits.Remove(circuit);
                }

                DbGraph.DelCircuits(delCircuits);
            }
            //dot
            var updataDots = TmpDots.Intersect(DbDots).ToList();
            if (updataDots.Count > 0)
            {
                DbGraph.UpdataDots(updataDots);
            }
            var newDots = TmpDots.Except(DbDots).ToList();
            if (newDots.Count > 0)
            {
                DbDots.AddRange(newDots);
                DbGraph.InsertDots(newDots);
            }
            var delDots = DbDots.Except(TmpDots).ToList();
            if (delDots.Count > 0)
            {
                foreach (var dot in delDots)
                {
                    DbDots.Remove(dot);
                }
                DbGraph.DelDots(delDots);
            }
            return true;
        }
        
        #region Cable

        public static int GetMaxCableId()
        {
            if (TmpCables == null || TmpCables.Count <= 0) return 0;
            return TmpCables.Max(x => x.CableId);
        }

        public static bool GetCables(out List<CableInfo> cableList)
        {
            cableList = TmpCables;
            return true;
        }

        public static bool GetCablesByName(out List<CableInfo> cableList, string cableName)
        {
            if (!string.IsNullOrWhiteSpace(cableName))
            {
                cableList = TmpCables.Where(cable => cable.CableName.Contains(cableName)).ToList();
                return true;
            }
            cableList = TmpCables;
            return true;
        }

        public static bool IsCableNameExist(CableInfo cmpCable, string cableName, out bool isCableNameExist)
        {
            if (TmpCables.Any(cable => cable.CableName == cableName.Trim() && cable != cmpCable))
            {
                isCableNameExist = true;
                return true;
            }
            isCableNameExist = false;
            return true;
        }

        public static bool CopyCable(CableInfo cable, string username)
        {
            var newCbale = new CableInfo(cable)
            {
                CableName = cable.CableName+" 副本",
                CreateUser = username,
                ModifyDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
            TmpCables.Add(newCbale);

            var circuits = TmpCircuits.Where(circuit => circuit.ParentCableId == cable.CableId).ToList();
            foreach (var srcCircuit in circuits)
            {
                var circuit = new CircuitInfo(srcCircuit)
                {
                    ParentCableId = newCbale.CableId,
                };
                TmpCircuits.Add(circuit);
            }

            var dots = TmpDots.Where(dot => dot.ParentCableId == cable.CableId).ToList();
            foreach (var srcDot in dots)
            {
                var dot = new DotInfo(srcDot)
                {
                    ParentCableId = newCbale.CableId
                };
                TmpDots.Add(dot);
            }
            return true;
        }
        
        public static bool AddCable(CableInfo cable)
        {
            TmpCables.Add(cable);
            var circuitInfo = new CircuitInfo
            {
                ParentCableId = cable.CableId,
                CircuitId = 0,
                Name = "空点"
            };
            AddCircuit(circuitInfo);
            return true;
        }

        public static bool DelCable(CableInfo cable)
        {
            TmpCables.Remove(cable);
            TmpCircuits.RemoveAll(circuit => circuit.ParentCableId == cable.CableId);
            TmpDots.RemoveAll(dot => dot.ParentCableId == cable.CableId);
            return true;
        }
        
        #endregion

        #region Circuit

        public static bool GetCircuits(out List<CircuitInfo> circuitList)
        {
            circuitList = TmpCircuits;
            return true;
        }
	    public static int GetMaxCircuitId(CableInfo cable)
        {
            if (TmpCircuits == null || TmpCircuits.Count <= 0) return 0;
            return TmpCircuits.Where(circuit => circuit.ParentCableId == cable.CableId).Max(x => x.CircuitId);
        }
        public static bool AddCircuit(CircuitInfo circuit)
        {
            TmpCircuits.Add(circuit);
            return true;
        }
        public static bool AddCircuits(List<CircuitInfo> circuits)
        {
            TmpCircuits.AddRange(circuits);
          
            return true;
        }
        public static bool DelCircuits(List<CircuitInfo> circuits)
        {
            TmpCircuits.RemoveAll(circuits.Contains);

            return true;
        }


        #endregion

        #region Dot

        public static int GetMaxDotId(CableInfo cable)
        {
            if (TmpDots == null || TmpDots.Count <= 0) return 0;
            return TmpDots.Where(dot=>dot.ParentCableId == cable.CableId).Max(x => x.DotId);
        }

        public static bool GetDos(out List<DotInfo> dotList)
        {
            dotList = TmpDots;
            return true;
        }

        public static bool AddDot(DotInfo dot)
        {
            TmpDots.Add(dot);
            return true;
        }
        public static bool AddDots(List<DotInfo> dots)
        {
            TmpDots.AddRange(dots);
            return true;
        }
        public static bool DelDots(List<DotInfo> dots)
        {
            TmpDots.RemoveAll(dots.Contains);
            return true;
        }


        #endregion
        

    }
}
