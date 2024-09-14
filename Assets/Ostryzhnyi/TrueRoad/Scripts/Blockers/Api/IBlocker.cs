using Blockers.Data;

namespace Blockers.Api
{
    public interface IBlocker
    {
        public void SetState(BlockerState state);
        public void Explosion(float radius);
    }
}