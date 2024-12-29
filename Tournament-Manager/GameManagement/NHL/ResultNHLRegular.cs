using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentManagement.NHL;

namespace GameManagement.NHL
{
    public class ResultNHLRegular : ResultNHL
    {
        /// <summary>
        /// No overtime if null.
        /// </summary>
        public ResultOvertime? ResultOvertime { get; private set; }
        /// <summary>
        /// No shootout if null.
        /// </summary>
        public ResultShootout? ResultShootout { get; private set; }


        public ResultNHLRegular(TeamNHL home, TeamNHL away) : base(home, away)
        {
        }
    }
}
