using System;

namespace Ostryzhnyi.QuickRollGame.Scripts.Player.Api
{
    public interface IPlayer
    {
        public bool IsAlive { get; }
        public bool IsWinner { get; }
        public float Radius { get; }
        public event Action OnDead;
        public event Action OnWin;

        public void Lower(float multiplier);
    }
}