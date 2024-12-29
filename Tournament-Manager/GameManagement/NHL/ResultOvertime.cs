using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManagement.NHL
{
    public class ResultOvertime : ResultPeriod
    {
        public bool GoalScored { get; }

        public ResultOvertime(bool goalScored) 
        { 
        }
    }
}
