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
    public abstract class ResultNHL : Result
    {
        public ResultPeriod Period1 { get; protected set; }
        public ResultPeriod Period2 { get; protected set; }
        public ResultPeriod Period3 { get; protected set; }

        


        public ResultNHL(TeamNHL home, TeamNHL away) : base(home, away) 
        {
            
        }

       

    }
}
