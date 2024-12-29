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
        public TeamNHL(string name, string shortName, double winRate, double drawRate, double avarageScore) : 
            base(name, shortName, winRate, drawRate, avarageScore)
        {
                
        }
    }
}
