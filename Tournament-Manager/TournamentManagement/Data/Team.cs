using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TournamentManagement.Data
{
    public abstract class Team
    {
        public string Name { get; }
        public string ShortName { get; }

        public double WinRate { get; }
        public double DrawRate { get; }
        public double AvarageScore { get; }

        protected Team(string name, string shortName, double winRate, double drawRate, double avarageScore)
        {
            Name = name;
            ShortName = shortName;
            WinRate = winRate;
            DrawRate = drawRate;
            AvarageScore = avarageScore;
        }

       
    }
}
