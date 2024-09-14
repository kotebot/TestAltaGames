using Ostryzhnyi.QuickRollGame.Scripts.Blockers.Api;
using Ostryzhnyi.QuickRollGame.Scripts.Player.Api;
using Ostryzhnyi.QuickRollGame.Scripts.Player.Impl.BaseClasses;
using UnityEngine;

namespace Ostryzhnyi.QuickRollGame.Scripts.Player.Impl.Bullets
{
    public class Bullet : BaseMovement, IBullet
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<IBlocker>(out var blocker))
                blocker.Explosion(transform.localScale.x);
            Destroy(gameObject);
        }

        public float Radius => transform.localScale.x;

        public void Upper(float multiplier)
        {
            transform.localScale += Vector3.one * multiplier;
        }

        protected override bool CanMove()
        {
            return true;
        }
    }
}