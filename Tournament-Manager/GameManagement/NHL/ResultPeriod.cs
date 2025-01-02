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
        protected int sectionsPerMin = 12;
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
                eventSection = value * sectionsPerMin;
            } 
        }
        public ResultPeriod? PrevPeriod { get; }


        private int minutes;

        private int eventSection;

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

            report = GenerateReport();
        }

        protected virtual List<GameEvent> GenerateReport()
        {

            List<GameEvent> report = new List<GameEvent>();

            var goalReport = new List<GameEvent>();
            var shotReport = new List<GameEvent>();
            var powerplayReports = new List<List<GameEvent>>();

            //POWER PLAY
            powerplayReports = GeneratePowerplays();

            //TODO: separate inherited powerplays

            //POWER PLAY

            //SHOT
            shotReport.AddRange(GenerateShots(HomeShots, AwayShots, HomeScore, AwayScore, eventSection));
            //SHOT

            //merge all reports chronologicaly
            var allReports = new List<List<GameEvent>>() 
            { 
                shotReport
            };
            allReports.AddRange(powerplayReports);
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


        /// <summary>
        /// Generates full report for one powerplay.
        /// </summary>
        /// <param name="sectionOffset">Absolute section index.</param>
        /// <param name="home"></param>
        /// <param name="blockArray"></param>
        /// <returns>Returns list or null if powerplay can't be generated.</returns>
        protected virtual List<GameEvent>? GeneratePowerplayReport(int sectionOffset, bool home, bool[] blockArray, int hShots, int aShots)
        {
            var list = new List<GameEvent>();

            var p = new Powerplay(
                TranslateEventBlocks(sectionOffset).Item1,
                TranslateEventBlocks(sectionOffset).Item2 + RandomGenerator.RandomInInterval(0, 60 / sectionsPerMin),
                true, true);
            var pEnd = new Powerplay(p.Min + p.Length, p.Sec, false, true, p.Length);
            if (!(blockArray[GetTime(p.Min, p.Sec)]))
            {
                //generating report
                list.Add(p);
                Block(true, true, GetTime(p.Min, p.Sec));
                list.AddRange(GenerateShots(hShots, aShots, 0, 0, p.Length * sectionsPerMin, sectionOffset - p.Length * sectionsPerMin));
                list.Add(pEnd);
                Block(true, true, GetTime(p.Min, p.Sec));

                return list;
            }

            return null;
        }


        /// <summary>
        /// Gets List of all powerplay reports in a period.
        /// </summary>
        /// <returns></returns>
        protected virtual List<List<GameEvent>> GeneratePowerplays()
        {
            var list = new List<List<GameEvent>>();
            double home = HomePowerplays;
            double away = AwayPowerplays;

            //TODO: remake shot count
            for (int i = eventSection; i > 0; i--)
            {
                if (RandomGenerator.RandomBool((home + away) / i))
                {
                    if (RandomGenerator.RandomBool(home / (home + away)))
                    {
                        var l = GeneratePowerplayReport(i, true, homeBlock, 5, 5);
                        if (l != null)
                        {
                            list.Add(l);
                            home--;
                        }
                    }
                    else
                    {
                        var l = GeneratePowerplayReport(i, false, awayBlock, 5, 5);
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
        protected List<GameEvent> GenerateShots(double hShots, double aShots, double hGoals, double aGoals, int eventSection, int blockOffset = 0)
        {
            var list = new List<GameEvent>();

            for (int i = eventSection; i > 0; i--)
            {
                if (RandomGenerator.RandomBool((hShots + aShots) / i))
                {
                    if (RandomGenerator.RandomBool(hShots / (hShots + aShots)))
                    {
                        //TODO:change shot on goal
                        var s = new Shot(
                            TranslateEventBlocks(i + blockOffset).Item1, 
                            TranslateEventBlocks(i + blockOffset).Item2 + RandomGenerator.RandomInInterval(0, 60 / sectionsPerMin), 
                            true, false);
                        if (!(homeBlock[GetTime(s.Min, s.Sec)]))
                        {
                            list.Add(s);
                            Block(true, true, GetTime(s.Min, s.Sec));

                            //GOAL
                            if (RandomGenerator.RandomBool(hGoals / hShots))
                            {
                                hGoals--;
                                list.Add(new Goal(s.Min, s.Sec, true));
                            }

                            hShots--;
                        }
                    }
                    else
                    {
                        var s = new Shot(
                            TranslateEventBlocks(i  + blockOffset).Item1, 
                            TranslateEventBlocks(i + blockOffset).Item2 + RandomGenerator.RandomInInterval(0, 60 / sectionsPerMin), 
                            false, false);
                        if (!(awayBlock[GetTime(s.Min, s.Sec)]))
                        {
                            list.Add(s);
                            Block(true, true, GetTime(s.Min, s.Sec));

                            //GOAL
                            if (RandomGenerator.RandomBool(aGoals / aShots))
                            {
                                aGoals--;
                                list.Add(new Goal(s.Min, s.Sec, false));
                            }

                            aShots--;
                            
                        }
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Translates Event blocks to (minutes, seconds) for inverted block index.
        /// </summary>
        /// <returns></returns>
        protected (int, int) TranslateEventBlocks(int block)
        {
            int blocksPerMin = eventSection / Minutes;
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
    
}
