using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentManagement.Data;

namespace GameManagement.ResultGeneration
{
    /// <summary>
    /// Holds all the result data.
    /// </summary>
    public abstract class Result
    {
        protected Team Home { get; }
        protected Team Away { get; }
        public Team? Winner { get; private set; }
        public Team? Loser { get; private set; }

        public bool? HomeIsWinner { 
            get { return homeIsWinner; } 
            set
            {
                homeIsWinner = value;
                if (homeIsWinner != null && (bool)homeIsWinner)
                {
                    Winner = Home;
                    Loser = Away;
                }
                else
                {
                    Loser = Home;
                    Winner = Away;
                }
            }
        }

        private bool? homeIsWinner;


        protected Result(Team home, Team away)
        {
            Home = home;
            Away = away;

            HomeIsWinner = GenerateResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Home team is a winner. Returns null if game is a draw.</returns>
        /// 
        protected virtual bool? GenerateResult()
        {
            bool home = DetermineWinner();

            return home;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Home is winner.</returns>
        protected bool DetermineWinner()
        {
            bool home = RandomGenerator.RandomBool(Home.WinRate / (Home.WinRate + Away.WinRate));
            
            return home;
        }
    }
}
