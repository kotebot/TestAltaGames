using Ostryzhnyi.QuickRollGame.Scripts.Blockers.Data;

namespace Ostryzhnyi.QuickRollGame.Scripts.Blockers.Api
{
    public interface IBlocker
    {
        public void SetState(BlockerState state);
        public void Explosion(float radius);
    }
}