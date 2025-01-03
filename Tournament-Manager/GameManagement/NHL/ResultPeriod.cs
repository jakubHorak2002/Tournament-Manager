using GameManagement.ResultGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameManagement.NHL
{
    public class ResultPeriod
    {
        public int HomeScore { get; } 
        public int AwayScore { get; }
        public int HomeShots { get; }
        public int HomePowerplays { get; }
        public int AwayPowerplays { get; }
        public int AwayShots { get; }
        public int Minutes { 
            get => minutes;
            protected set 
            {
                minutes = value;
                homeBlock = new bool[60 * Minutes];
                awayBlock = new bool[60 * Minutes];
            } 
        }
        private int minutes;
        public PeriodType Type { get; }
        public ResultPeriod? PrevPeriod { get; }

        protected bool[] homeBlock;
        protected bool[] awayBlock;

        public List<GameEvent> OverflowedEvents { get; } = new List<GameEvent>();


        protected List<GameEvent> report = new List<GameEvent>();

        public ResultPeriod(
            int homeScore, int awayScore, int homeShots, int awayShots, int homePowerplays, int awayPowerplays,
            ResultPeriod? prevPeriod, PeriodType type, bool genReport = true
            ) 
        { 
            HomeScore = homeScore;
            AwayScore = awayScore;
            HomeShots = homeShots;
            AwayShots = awayShots;
            HomePowerplays = homePowerplays;
            AwayPowerplays = awayPowerplays;
            Minutes = 20;
            PrevPeriod = prevPeriod;
            Type = type;

            if (genReport) report = GenerateReport();
        }

        public virtual List<GameEvent> GenerateReport()
        {
            List<GameEvent> report = new List<GameEvent>();
            report.Add(new PeriodMark(0, 0, true, Type));

            var goalReport = new List<GameEvent>();
            var shotReport = new List<GameEvent>();
            var powerplayReports = new List<List<GameEvent>>();

            var allReports = new List<List<GameEvent>>();

            if (PrevPeriod != null)
            {
                allReports.Add(PrevPeriod.OverflowedEvents);
                foreach (var item in PrevPeriod.OverflowedEvents)
                {
                    //blocking based on overflowed events
                    Block(true, true, GetTime(item.Min, item.Sec));
                }
            }

            //POWER PLAY
            powerplayReports = GeneratePowerplays();
            //POWER PLAY

            //SHOT
            shotReport.AddRange(GenerateShots(HomeShots, AwayShots, HomeScore, AwayScore, Minutes * 60));
            //SHOT

            //merge all reports chronologicaly
            allReports.Add(shotReport);
            allReports.AddRange(powerplayReports);
            

            while (true)
            {
                List<GameEvent>? min = null;
                foreach (var item in allReports)
                {
                    if (item.Count == 0) continue;
                    if (min == null || GameEvent.FirstEventEarlier(item[0], min[0])) min = item;
                }
                if (min != null)
                {
                    report.Add(min[0]);
                    min.RemoveAt(0);
                }
                else break;
            }

            //overflowed events
            while(report.Any() && report.Last().Min >= Minutes)
            {
                report.Last().Overflow(Minutes);
                OverflowedEvents.Insert(0, report.Last());
                report.RemoveAt(report.Count - 1);
            }

            report.Add(new PeriodMark(0, 0, false, Type));

            return report;
        }


        /// <summary>
        /// Generates full report for one powerplay.
        /// </summary>
        /// <returns>Returns list or null if powerplay can't be generated.</returns>
        protected virtual List<GameEvent>? GeneratePowerplayReport(
            int min, int sec, bool home, bool[] blockArray, int hShots, int aShots
            )
        {
            //goal scored
            int hGoals, aGoals;
            if (home)
            {
                hGoals = PowerplayGoalsScored();
                aGoals = ShorthandedGoalsScored();
            }
            else
            {
                aGoals = PowerplayGoalsScored();
                hGoals = ShorthandedGoalsScored();
            }

            var list = new List<GameEvent>();

            var p = new Powerplay(min, sec, true, home);
            var pEnd = new Powerplay(p.Min + p.Length / 60, p.Sec, false, home, p.Length);
            if (GetTime(p.Min, p.Sec) >= blockArray.Length || !(blockArray[GetTime(p.Min, p.Sec)]))
            if (GetTime(p.Min, p.Sec) >= blockArray.Length || !(blockArray[GetTime(p.Min, p.Sec)]))
            {
                //generating report
                list.Add(p);
                list.AddRange(GenerateShots(hShots, aShots, hGoals, aGoals, p.Length, GetTime(p.Min, p.Sec), true));

                //goal scored
                if ((home && hGoals > 0) || (!home && aGoals > 0))
                {
                    int i = 0;
                    foreach (var item in list)
                    {
                        i++;
                        if (item is Goal)
                        {
                            break;
                        }
                    }
                    list.RemoveRange(i, list.Count - i);
                    if (list.Last() is Goal) pEnd = new Powerplay(list.Last().Min, list.Last().Sec, false, true, p.Length);
                }
                

                list.Add(pEnd);
                //full powerplay block
                Block(true, true, GetTime(p.Min, p.Sec) + pEnd.Length / 2, pEnd.Length / 2 + 5);

                //TODO: powerplay overlapping + shot sometimes during powerplay bug
                return list;
            }

            return null;
        }

        protected int PowerplayGoalsScored()
        {
            return 1;
        }

        protected int ShorthandedGoalsScored()
        {
            return 0;
        }


        /// <summary>
        /// Gets list of all powerplay reports in a period.
        /// </summary>
        /// <returns></returns>
        protected virtual List<List<GameEvent>> GeneratePowerplays()
        {
            var list = new List<List<GameEvent>>();
            double home = HomePowerplays;
            double away = AwayPowerplays;

            //TODO: remake shot count
            for (int i = Minutes * 60; i > 0; i--)
            {
                if (home + away > 0 && RandomGenerator.RandomBool((home + away) / i))
                {
                    if (home + away > 0 && RandomGenerator.RandomBool(home / (home + away)))
                    {
                        var l = GeneratePowerplayReport(
                            GetTime(Minutes * 60 - i).Item1, GetTime(Minutes * 60 - i).Item2, true, homeBlock, 5, 5
                            );
                        if (l != null)
                        {
                            list.Add(l);
                            home--;
                        }
                    }
                    else
                    {
                        var l = GeneratePowerplayReport(GetTime(Minutes * 60 - i).Item1, GetTime(Minutes * 60 - i).Item2, false, awayBlock, 5, 5);
                        if (l != null)
                        {
                            list.Add(l);
                            away--;
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Returns list of shots and goals based on given inputs.
        /// </summary>
        /// <param name="eventSection">Number of blocks in the period.</param>
        /// <returns></returns>
        protected List<GameEvent> GenerateShots(
            double hShots, double aShots, double hGoals, double aGoals, int secLength, int offset = 0, bool powerplay = false 
            )
        {
            var list = new List<GameEvent>();

            for (int i = secLength; i > 0; i--)
            {
                if (RandomGenerator.RandomBool((hShots + aShots) / i))
                {
                    (int, int) time = GetTime(offset + secLength - i);
                    if (hShots + aShots > 0 && RandomGenerator.RandomBool(hShots / (hShots + aShots)))
                    {
                        //TODO:change shot on goal
                        var s = new Shot(time.Item1, time.Item2, true, false, powerplay);
                        if (GetTime(s.Min, s.Sec) >= homeBlock.Length || !(homeBlock[GetTime(s.Min, s.Sec)]))
                        {
                            list.Add(s);
                            Block(true, true, GetTime(s.Min, s.Sec));

                            //GOAL
                            if (hShots > 0 && RandomGenerator.RandomBool(hGoals / hShots))
                            {
                                hGoals--;
                                list.Add(new Goal(s.Min, s.Sec, true, powerplay));
                            }

                            hShots--;
                        }
                    }
                    else
                    {
                        var s = new Shot(time.Item1, time.Item2, false, false, powerplay);
                        if (GetTime(s.Min, s.Sec) >= awayBlock.Length || !(awayBlock[GetTime(s.Min, s.Sec)]))
                        {
                            list.Add(s);
                            Block(true, true, GetTime(s.Min, s.Sec));

                            //GOAL
                            if (aShots > 0 && RandomGenerator.RandomBool(aGoals / aShots))
                            {
                                aGoals--;
                                list.Add(new Goal(s.Min, s.Sec, false, powerplay));
                            }

                            aShots--;
                        }
                    }
                }
            }

            return list;
        }


        /// <summary>
        /// Gets one number equivalent to time.
        /// </summary>
        protected int GetTime(int min, int sec)
        {
            return min * 60 + sec;
        }
        /// <summary>
        /// Translate val to (min, sec).
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        protected (int, int) GetTime(int val)
        {
            return (val / 60, val % 60);
        }

        /// <summary>
        /// Sets blocks for ivent on specified position.
        /// </summary>
        protected void Block(bool home, bool away, int pos, int blockSize = 5)
        {
            if (home)
            {
                for (int i = pos - blockSize; i < pos + blockSize + 1; i++) 
                {
                    if (i >= 0 && i < Minutes * 60)
                    {
                        homeBlock[i] = true;
                    }
                }
            }
            if (away)
            {
                for (int i = pos - blockSize; i < pos + blockSize + 1; i++)
                {
                    if (i >= 0 && i < Minutes * 60)
                    {
                        awayBlock[i] = true;
                    }
                }
            }
        }


        public override string ToString()
        {
            string s = "";
            foreach (var gameEvent in report) 
            {
                s += gameEvent.ToString() + "\n";
            }
            return s;
        }
    }

    public enum PeriodType
    {
        Period1, Period2, Period3, Overtime, Shootout
    }

}
