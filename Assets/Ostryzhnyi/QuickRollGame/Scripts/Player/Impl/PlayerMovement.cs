using Ostryzhnyi.DI;
using Ostryzhnyi.QuickRollGame.Scripts.Player.Api;
using Ostryzhnyi.QuickRollGame.Scripts.Player.Impl.BaseClasses;

namespace Ostryzhnyi.QuickRollGame.Scripts.Player.Impl
{
    public class PlayerMovement : BaseMovement
    {
        [Inject] private IPlayer _player;

        protected override bool CanMove()
        {
            return _player.IsAlive && !_player.IsWinner;
        }
    }
}