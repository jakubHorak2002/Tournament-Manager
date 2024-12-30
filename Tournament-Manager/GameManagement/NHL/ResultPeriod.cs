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
        public int Minutes { get; protected set; } = 20;


        private List<string> report = new List<string>();

        public ResultPeriod(int homeScore, int awayScore) 
        { 
            HomeScore = homeScore;
            AwayScore = awayScore;
        }

    }
}
