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
    public abstract class Result<T> where T : Team
    {
        protected T Home { get; }
        protected T Away { get; }
        public T? Winner { get; private set; }
        public T? Loser { get; private set; }

        public int HomeScore { get; protected set; } = 0;
        public int AwayScore { get; protected set; } = 0;


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


        protected Result(T home, T away)
        {
            Home = home;
            Away = away;

            GenerateResult();
        }

        /// <summary>
        /// Generates all the stats of this match.
        /// </summary>
        /// 
        protected virtual void GenerateResult()
        {
            HomeIsWinner = DetermineWinner();
            DetermineScore();
        }

        /// <summary>
        /// Sets a winner or a draw.
        /// </summary>
        /// <returns>Home is winner.</returns>
        protected virtual bool? DetermineWinner()
        {
            bool? home = null;
            if (!RandomGenerator.RandomBool((Home.DrawRate + Away.DrawRate) / 2))
            {
                home = RandomGenerator.RandomBool(Home.WinRate / (Home.WinRate + Away.WinRate));
            }
           
            return home;
        }

        /// <summary>
        /// Sets both scores according to HomeIsWinner value.
        /// </summary>
        protected virtual void DetermineScore()
        {
            
            if (!(HomeIsWinner is null))
            {
                do
                {
                    HomeScore = GetScoreFromAvarage(Home.AvarageScore);
                    AwayScore = GetScoreFromAvarage(Away.AvarageScore);
                }
                while (HomeScore > AwayScore != HomeIsWinner || AwayScore > HomeScore == HomeIsWinner);
            }
            else
            {
                HomeScore = GetScoreFromAvarage((Home.AvarageScore + Away.AvarageScore) / 2);
                AwayScore = HomeScore;
            }

            
        }

        protected int GetScoreFromAvarage(double avarage)
        {
            int score = 0;

            double L = Math.Exp(-avarage); // e^(-lambda)
            double p = 1;

            do
            {
                score++;
                p *= RandomGenerator.RandomDouble(); // Multiply by a new random number
            } while (p > L);

            return score - 1; // Subtract 1 because k starts at 1
        }
    }
}
