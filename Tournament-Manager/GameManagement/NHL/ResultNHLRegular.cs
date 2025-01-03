using GameManagement.ResultGeneration;
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

        //TODO: generate overtime
        protected override void GenerateOvertime(ResultPeriod period3)
        {
            //chance of shootout
            if (RandomGenerator.RandomBool(1))
            {
                if (RandomGenerator.RandomBool(Home.OtWinRate / (Home.OtWinRate + Away.OtWinRate))) 
                    ResultOvertime = new ResultOvertime(1, 0, 5, 5, 1, 0, Period3, ResultOvertime.OtType.regular);
                else ResultOvertime = new ResultOvertime(0, 1, 5, 5, 1, 0, Period3, ResultOvertime.OtType.regular);
            }
        }

        public override string ToString()
        {
            var s = base.ToString();
            if (ResultOvertime != null)
            {
                s += "\novertime:\n\n" + ResultOvertime.ToString();
            }
            return s;
        }
    }
}
