// See https://aka.ms/new-console-template for more information
using GameManagement.NHL;
using GameManagement.ResultGeneration;
using TournamentManagement.Data;
using TournamentManagement.NHL;

TeamNHL home = new TeamNHL("Dalas Stars", "Stars", 0.7f);
TeamNHL away = new TeamNHL("Boston Bruins", "Bruins", 0.6f);
Result r = new ResultNHL(home, away);

Console.WriteLine(r.HomeIsWinner);