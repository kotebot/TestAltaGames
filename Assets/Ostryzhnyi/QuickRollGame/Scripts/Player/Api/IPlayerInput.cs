using System;

namespace Ostryzhnyi.QuickRollGame.Scripts.Player.Api
{
    public interface IPlayerInput
    {
        public event Action OnStartTap;
        public event Action OnTapping;
        public event Action OnEndTap;
    }
}