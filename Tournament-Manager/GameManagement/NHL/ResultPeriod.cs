using GameManagement.ResultGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private int minutes;

        private int eventBlocks;


        private List<GameEvent> report = new List<GameEvent>();

        public ResultPeriod(int homeScore, int awayScore, int homeShots, int awayShots, int homePowerplays, int awayPowerplays) 
        { 
            HomeScore = homeScore;
            AwayScore = awayScore;
            HomeShots = homeShots;
            AwayShots = awayShots;
            HomePowerplays = homePowerplays;
            AwayPowerplays = awayPowerplays;
            Minutes = 20;

            GenerateReport();
        }

        protected virtual List<GameEvent> GenerateReport()
        {

            List<GameEvent> report = new List<GameEvent>();

            List<GameEvent> goalReport = new List<GameEvent>();
            List<GameEvent> shotReport = new List<GameEvent>();
            List<GameEvent> powerplayReport = new List<GameEvent>();

            bool[] homeBlock = new bool[60 * Minutes];
            bool[] awayBlock = new bool[60 * Minutes];

            //TODO: generate individual reports
            powerplayReport.AddRange(GeneratePowerplays());

            while (goalReport.Count > 0 || shotReport.Count > 0 || powerplayReport.Count > 0) 
            {
                List<GameEvent> min = goalReport;
                if (GameEvent.FirstEventEarlier(shotReport[0], min[0]))
                {
                    min = shotReport;
                }
                if (GameEvent.FirstEventEarlier(powerplayReport[0], min[0]))
                {
                    min = powerplayReport;
                }
                report.Add(min[0]);
                min.RemoveAt(0);
            }

            return report;
        }

        protected virtual List<Powerplay> GeneratePowerplays()
        {
            var list = new List<Powerplay>();
            double home = HomePowerplays;
            double away = AwayPowerplays;

            for (int i = eventBlocks; i > 0; i--)
            {
                if (RandomGenerator.RandomBool((home + away) / i))
                {
                    if (RandomGenerator.RandomBool(home / (home + away)))
                    {
                        home--;
                        list.Add(new Powerplay(TranslateEventBlocks(i).Item1, TranslateEventBlocks(i).Item2 + RandomGenerator.RandomInInterval(0, 10), true, true, false));
                    }
                    else
                    {
                        away--;
                        list.Add(new Powerplay(TranslateEventBlocks(i).Item1, TranslateEventBlocks(i).Item2 + RandomGenerator.RandomInInterval(0, 10), true, true, false));
                    }
                }
                if (home + away == 0) break;
            }

            return list;
        }

        /// <summary>
        /// Translates Event blocks to (minutes, seconds)
        /// </summary>
        /// <returns></returns>
        protected (int, int) TranslateEventBlocks(int block)
        {
            int blocksPerMin = eventBlocks / Minutes;
            return (Minutes - (block - 1) / blocksPerMin - 1, (blocksPerMin - (block % blocksPerMin)) * (60 / blocksPerMin));
        }
    }
    
}
