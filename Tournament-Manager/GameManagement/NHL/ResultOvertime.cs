using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManagement.NHL
{
    public class ResultOvertime : ResultPeriod
    {
        public bool GoalScored { get; } = false;

        public ResultOvertime(
            int homeScore, int awayScore, int homeShots, int awayShots, int homePowerplays, int awayPowerplays, ResultPeriod prevPeriod,
            OtType type
            ) : base(
                homeScore, awayScore, homeShots, awayShots, homePowerplays, awayPowerplays, prevPeriod, PeriodType.Overtime, false
                ) 
        {
            if (type == OtType.regular) Minutes = 5;
            else Minutes = 20;

            if (homeScore > 0 || awayScore > 0) GoalScored = true;

            report = GenerateReport();
        }

        public enum OtType { regular, playoff }
    }
}
