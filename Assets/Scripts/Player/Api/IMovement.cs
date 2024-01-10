namespace Player.Api
{
    public interface IMovement
    {
        public bool IsMoving { get; }
        public void Move();
    }
}