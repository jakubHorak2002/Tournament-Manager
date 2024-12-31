using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManagement.NHL
{
    public abstract class GameEvent
    {
        public int Min { get; }
        public int Sec { get; }

        public GameEvent(int min, int sec) 
        {
            Min = min;
            Sec = sec;
        }

        public static bool FirstEventEarlier(GameEvent e1, GameEvent e2)
        {
            if (e1.Min < e2.Min) return true;
            else if (e1.Min == e2.Min && e1.Sec < e2.Sec) return true;
            return false;

        }
    }

    public class Goal : GameEvent 
    {
        public bool Home { get; }

        public Goal(int min, int sec, bool home) : base(min, sec)
        {
            Home = home;
        }
    }

    public class Shot : GameEvent
    {
        public bool OnGoal { get; }
        public Shot(int min, int sec, bool onGoal) : base(min, sec)
        {
            OnGoal = onGoal;
        }
    }

    public class Powerplay : GameEvent
    {
        public bool Start { get; }
        public bool Home { get; }
        public bool PowerplayScore { get; }
        public bool ShorthandScore { get; }

        public Powerplay(int min, int sec, bool start, bool home, bool powerplayScore) : base(min, sec)
        {
            Start = start;
            Home = home;
            PowerplayScore = powerplayScore;
        }
    }

    public class Faceoff : GameEvent
    {
        bool HomeWin { get; }
        public Faceoff(int min, int sec, bool homeWin) : base(min, sec)
        {
            HomeWin = homeWin;
        }
    }
}
