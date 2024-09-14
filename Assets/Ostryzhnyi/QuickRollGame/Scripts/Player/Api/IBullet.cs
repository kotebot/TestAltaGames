namespace Ostryzhnyi.QuickRollGame.Scripts.Player.Api
{
    public interface IBullet : IMovement
    {
        public float Radius { get; }
        public void Upper(float multiplier);
    }
}