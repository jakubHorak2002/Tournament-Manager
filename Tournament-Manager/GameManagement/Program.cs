﻿// See https://aka.ms/new-console-template for more information
using GameManagement.NHL;
using GameManagement.ResultGeneration;
using TournamentManagement.Data;
using TournamentManagement.NHL;

TeamNHL home = new TeamNHL("Dalas Stars", "Stars", "Sta", 0.8, 1, 3, 1, 0, 0, 30);
TeamNHL away = new TeamNHL("Boston Bruins", "Bruins", "Bru", 0.6, 1, 3, 0, 1, 0, 30);

while (true)
for (int i = 0; i < 1; i++)
{
    var r = new ResultNHLRegular(home, away);
    Console.Write(r.HomeScore + ":" + r.AwayScore);
    Console.WriteLine("  ot: " + r.ResultOvertime.HomeScore + ":" + r.ResultOvertime.AwayScore);
    Console.WriteLine(r.ToString());

    Console.ReadLine();
    Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n");
}


