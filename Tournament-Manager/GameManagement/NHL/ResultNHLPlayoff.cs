using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentManagement.NHL;

namespace GameManagement.NHL
{
    public class ResultNHLPlayoff : ResultNHL
    {
        public ResultNHLPlayoff(TeamNHL home, TeamNHL away) : base(home, away)
        {
        }

      

        protected override void GenerateOvertime(ResultPeriod period3)
        {
            throw new NotImplementedException();
        }

       
    }
}
