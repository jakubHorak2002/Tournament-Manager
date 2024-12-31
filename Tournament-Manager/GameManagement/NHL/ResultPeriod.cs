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
                eventBlocks = value * 6;
            } 
        }
        public ResultPeriod? PrevPeriod { get; }


        private int minutes;

        private int eventBlocks;

        protected bool[] homeBlock;
        protected bool[] awayBlock;

        public List<GameEvent> InheritedEvents { get; } = new List<GameEvent>();


        private List<GameEvent> report = new List<GameEvent>();

        public ResultPeriod(int homeScore, int awayScore, int homeShots, int awayShots, int homePowerplays, int awayPowerplays, ResultPeriod? prevPeriod) 
        { 
            HomeScore = homeScore;
            AwayScore = awayScore;
            HomeShots = homeShots;
            AwayShots = awayShots;
            HomePowerplays = homePowerplays;
            AwayPowerplays = awayPowerplays;
            Minutes = 20;
            homeBlock = new bool[60 * Minutes];
            awayBlock = new bool[60 * Minutes];
            PrevPeriod = prevPeriod;

        GenerateReport();
        }

        protected virtual List<GameEvent> GenerateReport()
        {

            List<GameEvent> report = new List<GameEvent>();

            List<GameEvent> goalReport = new List<GameEvent>();
            List<GameEvent> shotReport = new List<GameEvent>();
            List<GameEvent> powerplayStartReport = new List<GameEvent>();
            List<GameEvent> powerplayEndReport = new List<GameEvent>();


            //TODO: generate individual reports
            (var s, var e) = GeneratePowerplays();
            powerplayStartReport.AddRange(s);
            powerplayEndReport.AddRange(e);

            //separate inherited powerplays
            var temp = new List<GameEvent>();
            foreach (var item in powerplayEndReport)
            {
                if (item.Min < 20) temp.Add(item);
                else InheritPowerplay((Powerplay)item);
            }
            powerplayEndReport = temp;

            //merge all reports chronologicaly
            var allReports = new List<List<GameEvent>>() 
            { 
                goalReport, shotReport, powerplayStartReport, powerplayEndReport
            };
            if (PrevPeriod != null) allReports.Add(PrevPeriod.InheritedEvents);

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

            return report;
        }

        protected virtual (List<Powerplay>, List<Powerplay>) GeneratePowerplays()
        {
            var listStart = new List<Powerplay>();
            var listEnd = new List<Powerplay>();
            double home = HomePowerplays;
            double away = AwayPowerplays;

            for (int i = eventBlocks; i > 0; i--)
            {
                if (RandomGenerator.RandomBool((home + away) / i))
                {
                    if (RandomGenerator.RandomBool(home / (home + away)))
                    {
                        //2 min powerplay
                        var p = new Powerplay(TranslateEventBlocks(i).Item1, TranslateEventBlocks(i).Item2 + RandomGenerator.RandomInInterval(0, 10), true, true);
                        var pEnd = new Powerplay(p.Min + p.Length, p.Sec, false, true, p.Length);
                        if (!(homeBlock[GetTime(p.Min, p.Sec)]))
                        {
                            home--;
                            listStart.Add(p);
                            listEnd.Add(pEnd);
                            Block(true, true, i);
                        }
                    }
                    else
                    {
                        //2 min powerplay
                        var p = new Powerplay(TranslateEventBlocks(i).Item1, TranslateEventBlocks(i).Item2 + RandomGenerator.RandomInInterval(0, 10), true, true);
                        var pEnd = new Powerplay(p.Min + p.Length, p.Sec, false, true, p.Length);
                        if (!(awayBlock[GetTime(p.Min, p.Sec)]))
                        {
                            away--;
                            listStart.Add(p);
                            listEnd.Add(pEnd);
                            Block(true, true, i);
                        }
                    }
                }
                if (home + away == 0) break;
            }

            return (listStart, listEnd);
        }

        /// <summary>
        /// Translates Event blocks to (minutes, seconds)
        /// </summary>
        /// <returns></returns>
        protected (int, int) TranslateEventBlocks(int block)
        {
            int blocksPerMin = eventBlocks / Minutes;
            return (Minutes - (block - 1) / blocksPerMin - 1, (blocksPerMin - (block % blocksPerMin) - 1) * (60 / blocksPerMin));
        }

        /// <summary>
        /// Gets one number equivalent of time.
        /// </summary>
        protected int GetTime(int min, int sec)
        {
            return min * 60 + sec;
        }
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

        protected void InheritPowerplay(Powerplay p)
        {
            InheritedEvents.Add(new Powerplay(p.Min - Minutes, p.Sec, p.Start, p.Home, p.Length));
        }
    }
    
}
