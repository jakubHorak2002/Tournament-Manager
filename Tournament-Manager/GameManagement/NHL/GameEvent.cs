using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameManagement.NHL
{
    public abstract class GameEvent
    {
        public int Min { get; protected set; }
        public int Sec { get; }
        public bool Powerplay { get; }

        public GameEvent(int min, int sec, bool powerplay) 
        {
            Min = min;
            Sec = sec;
            Powerplay = powerplay;
        }

        public void Overflow(int periodLength)
        {
            if (Min >= periodLength) Min -= periodLength;
        }

        public static bool FirstEventEarlier(GameEvent e1, GameEvent e2)
        {
            if (e1 == null) return false;
            if (e2 == null) return true;
            if (e1.Min < e2.Min) return true;
            else if (e1.Min == e2.Min && e1.Sec < e2.Sec) return true;
            return false;
        }

        public override string ToString()
        {
            var powerplay = "";
            if (Powerplay) powerplay = "(pp) ";
            var min = Min.ToString();
            var sec = Sec.ToString();
            if (Min < 10) min = "0" + Min.ToString();
            if (Sec < 10) sec = "0" + Sec.ToString();
            return $"{min} : {sec} {powerplay}";
        }
    }

    public class Goal : GameEvent 
    {
        public bool Home { get; }

        public Goal(int min, int sec, bool home, bool powerplay) : base(min, sec, powerplay)
        {
            Home = home;
        }

        public override string ToString()
        {
            if (Home) return $"{base.ToString()}  GOAL (home)";
            return $"{base.ToString()}  GOAL (away)";
        }
    }

    public class Shot : GameEvent
    {
        public bool OnGoal { get; }
        public bool Home { get; }
        public Shot(int min, int sec, bool home, bool onGoal, bool powerplay) : base(min, sec, powerplay)
        {
            OnGoal = onGoal;
            Home = home;
        }

        public override string ToString()
        {
            if (Home) return $"{base.ToString()}  SHOT (home)";
            return $"{base.ToString()}  SHOT (away)";
        }
    }

    public class Powerplay : GameEvent
    {
        public bool Start { get; }
        public bool Home { get; }
        public int Length { get; }

        public Powerplay(int min, int sec, bool start, bool home, int length = 2) : base(min, sec, true)
        {
            Start = start;
            Home = home;
            Length = length;
        }

        public override string ToString()
        {
            var team = "home";
            var type = "START";
            if (!Home) team = "away";
            if (!Start) type = "END";
            return $"{base.ToString()} POWERPLAY {type} ({team})";
        }
    }

    public class Faceoff : GameEvent
    {
        bool HomeWin { get; }
        public Faceoff(int min, int sec, bool homeWin, bool powerplay) : base(min, sec, powerplay)
        {
            HomeWin = homeWin;
        }
    }
}
