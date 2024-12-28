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

        public float WinRate { get; }

        protected Team(string name, string shortName, float winRate)
        {
            Name = name;
            ShortName = shortName;
            WinRate = winRate;
        }
    }
}
