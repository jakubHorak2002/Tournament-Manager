// See https://aka.ms/new-console-template for more information
using GameManagement.NHL;
using GameManagement.ResultGeneration;
using TournamentManagement.Data;
using TournamentManagement.NHL;

TeamNHL home = new TeamNHL("Dalas Stars", "Stars", 0.8, 0.1, 3);
TeamNHL away = new TeamNHL("Boston Bruins", "Bruins", 0.6, 0.1, 3);

for (int i = 0; i < 100; i++)
{
    var r = new ResultNHLRegular(home, away);
    Console.WriteLine(r.HomeScore + ":" + r.AwayScore);
}


