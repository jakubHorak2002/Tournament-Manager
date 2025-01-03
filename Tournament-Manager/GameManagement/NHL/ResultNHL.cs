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
        public int HomePowerplays { get; protected set; } = 0;
        public int AwayPowerplays { get; protected set; } = 0;
        public int HomeShots { get; protected set; } = 0;
        public int AwayShots { get; protected set; } = 0;



        public ResultNHL(TeamNHL home, TeamNHL away) : base(home, away)
        {
            DetermineShots();
            DeterminePowerplay();
            //TODO:generate faceoffs

            //goals
            (int[] homeGoals, int[] awayGoals) = AssignToPeriod(HomeScore, AwayScore);
            //shots
            (int[] homeShots, int[] awayShots) = AssignToPeriod(HomeShots, AwayShots);
            //TODO: change amount of powerplays
            //powerplay
            (int[] homePowerplays, int[] awayPowerplays) = AssignToPeriod(4, 4);

            //TODO: change period1
            Period1 = new ResultPeriod(homeGoals[0], awayGoals[0], homeShots[0], awayShots[0], homePowerplays[0], awayPowerplays[0], null);
            //Period1 = new ResultPeriod(homeGoals[0], awayGoals[0], homeShots[0], awayShots[0], 4, 4, null);
            Period2 = new ResultPeriod(homeGoals[1], awayGoals[1], homeShots[1], awayShots[1], homePowerplays[1], awayPowerplays[1], Period1);
            Period3 = new ResultPeriod(homeGoals[2], awayGoals[2], homeShots[2], awayShots[2], homePowerplays[2], awayPowerplays[2], Period2);

            if (HomeScore == AwayScore) 
            {
                GenerateOvertime(Period3);
            }
        }

        abstract protected void GenerateOvertime(ResultPeriod period3);

        protected virtual void DeterminePowerplay()
        {
            //TODO: modify powerplay generation
            //powerplay
            RandomGenerator.GetRandomFromAvarage(Home.AvgPowerplay); 
            RandomGenerator.GetRandomFromAvarage(Away.AvgPowerplay);
        }

        protected virtual void DetermineShots()
        {
            //shots
            HomeShots = RandomGenerator.GetRandomFromAvarage(Home.AvgShots);
            AwayShots = RandomGenerator.GetRandomFromAvarage(Away.AvgShots);
        }

        protected (int[], int[]) AssignToPeriod(int homeAmount, int awayAmount)
        {
            int[] home = { 0, 0, 0 };
            int[] away = { 0, 0, 0 };
            //home assignment
            for (int i = 0; i < homeAmount; i++)
            {
                int a = RandomGenerator.RandomInInterval(0, 3);
                home[a]++;
            }
            //away assignment
            for (int i = 0; i < awayAmount; i++)
            {
                int a = RandomGenerator.RandomInInterval(0, 3);
                away[a]++;
            }

            return (home, away);
        }

        public override string ToString()
        {
            return $"Period1: \n{Period1.ToString()}\nPeriod2: \n{Period2.ToString()}\nPeriod3: \n{Period3.ToString()}\n";
        }
    }
}
