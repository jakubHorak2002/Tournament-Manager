using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManagement.NHL
{
    public class ResultPeriod
    {
        public int HomeScore { get; protected set; } = 0;
        public int AwayScore { get; protected set; } = 0;

        private List<string> report = new List<string>();
    }
}
