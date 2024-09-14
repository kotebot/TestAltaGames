namespace Ostryzhnyi.QuickRollGame.Scripts.Player.Api
{
    public interface IMovement
    {
        public bool IsMoving { get; }
        public void Move();
    }
}