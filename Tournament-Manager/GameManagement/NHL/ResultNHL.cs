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
    public class ResultNHL : Result
    {

        public ResultNHL(TeamNHL home, TeamNHL away) : base(home, away) 
        {
            
        }

        protected override bool? GenerateResult()
        {
            bool homeWins = DetermineWinner();

            

            return homeWins;
        }
    }
}
