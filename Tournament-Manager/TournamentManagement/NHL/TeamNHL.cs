using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentManagement.Data;

namespace TournamentManagement.NHL
{
    public class TeamNHL : Team
    {
        public double OtWinRate { get; }
        public double ShootoutWinRate { get; }

        //TODO: fix Team constructor
        public TeamNHL(string name, string shortName, string abbreviation, double winRate, double drawRate, double avarageScore) :
            base(name, shortName, abbreviation, winRate, drawRate, avarageScore)
        {
                
        }
    }
}
