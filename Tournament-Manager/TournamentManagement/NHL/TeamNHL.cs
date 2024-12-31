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
        public double SoWinRate { get; }
        public double AvgPowerplay { get; }
        public double AvgShots { get; }

        //TODO: fix Team constructor
        public TeamNHL(
            string name, string shortName, string abbreviation, double winRate, double drawRate, double avgScore, 
            double otWinRate, double soWinRate, double avgPowerplay, double avgShots
            ) : base(
                name, shortName, abbreviation, winRate, drawRate, avgScore
                )
        {
            OtWinRate = otWinRate;
            SoWinRate = SoWinRate;
            AvgPowerplay = avgPowerplay;
            AvgShots = avgShots;
        }
    }
}
