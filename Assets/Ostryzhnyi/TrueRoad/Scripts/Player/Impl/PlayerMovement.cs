using Ostryzhnyi.DI;
using Player.Api;
using Player.Impl.BaseClasses;

namespace Player.Impl
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