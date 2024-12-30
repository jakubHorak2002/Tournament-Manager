using GameManagement.ResultGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentManagement.Data;
using TournamentManagement.NHL;

namespace GameManagement.NHL
{
    public abstract class ResultNHL : Result<TeamNHL>
    {
        public ResultPeriod Period1 { get; }
        public ResultPeriod Period2 { get; }
        public ResultPeriod Period3 { get; }




        public ResultNHL(TeamNHL home, TeamNHL away) : base(home, away)
        {
            int[] homeGoals = { 0, 0, 0 };
            int[] awayGoals = { 0, 0, 0 };
            //home goals assignment
            for (int i = 0; i < HomeScore; i++) 
            {
                int a = RandomGenerator.RandomInInterval(0, 3);
                homeGoals[a]++;
            }
            //away goals assignment
            for (int i = 0; i < AwayScore; i++)
            {
                int a = RandomGenerator.RandomInInterval(0, 3);
                awayGoals[a]++;
            }

            Period1 = new ResultPeriod(homeGoals[0], awayGoals[0]);
            Period2 = new ResultPeriod(homeGoals[1], awayGoals[1]);
            Period3 = new ResultPeriod(homeGoals[2], awayGoals[2]);

            if (HomeScore == AwayScore) 
            {
                GenerateOvertime();
            }
        }

        abstract protected void GenerateOvertime();
    }
}
