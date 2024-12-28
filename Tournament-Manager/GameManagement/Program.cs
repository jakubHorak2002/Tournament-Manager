// See https://aka.ms/new-console-template for more information
using GameManagement.NHL;
using GameManagement.ResultGeneration;
using TournamentManagement.Data;
using TournamentManagement.NHL;

TeamNHL home = new TeamNHL("Dalas Stars", "Stars", 0.9);
TeamNHL away = new TeamNHL("Boston Bruins", "Bruins", 0.1);

int counter = 0;
for (int i = 0; i < 10000; i++)
{
    if ((bool)(new ResultNHL(home, away).HomeIsWinner)) counter++; 
}

Console.WriteLine(counter);

