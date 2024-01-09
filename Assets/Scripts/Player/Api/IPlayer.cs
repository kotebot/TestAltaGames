using System;

namespace Player.Api
{
    public interface IPlayer
    {
        public event Action OnDead;
        public bool IsAlive { get; }
        public float Radius { get; }

        public void Lower(float multiplier);
    }
}