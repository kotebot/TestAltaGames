using System;

namespace Player.Api
{
    public interface IPlayer
    {
        public event Action OnDead;
        public event Action OnWin;
        
        public bool IsAlive { get; }
        public bool IsWinner { get; }
        public float Radius { get; }

        public void Lower(float multiplier);
    }
}