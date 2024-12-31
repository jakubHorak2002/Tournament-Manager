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
        public int Minutes { get; protected set; } = 20;


        private List<GameEvent> report = new List<GameEvent>();

        public ResultPeriod(int homeScore, int awayScore, int homeShots, int awayShots, int homePowerplays, int awayPowerplays) 
        { 
            HomeScore = homeScore;
            AwayScore = awayScore;
            HomeShots = homeShots;
            AwayShots = awayShots;
            HomePowerplays = homePowerplays;
            AwayPowerplays = awayPowerplays;
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

            while (goalReport.Count > 0 && shotReport.Count > 0 && powerplayReport.Count > 0) 
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

    }
}
