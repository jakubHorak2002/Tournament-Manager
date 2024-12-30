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

        public ResultOvertime(int homeScore, int awayScore, OtType type) : base(homeScore, awayScore) 
        {
            if (type == OtType.regular) Minutes = 5;
            else Minutes = 20;

            if (homeScore > 0 || awayScore > 0) GoalScored = true;
        }

        public enum OtType { regular, playoff }
    }
}
