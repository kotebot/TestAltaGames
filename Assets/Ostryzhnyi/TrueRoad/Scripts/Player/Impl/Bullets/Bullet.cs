using Blockers.Api;
using Player.Api;
using Player.Impl.BaseClasses;
using UnityEngine;

namespace Player.Impl.Bullets
{
    public class Bullet : BaseMovement, IBullet
    {
        public float Radius => transform.localScale.x;

        protected override bool CanMove()
        {
            return true;
        }

        public void Upper(float multiplier)
        {
            transform.localScale += (Vector3.one * multiplier);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<IBlocker>(out var blocker))
            {
                blocker.Explosion(transform.localScale.x);
            }
            Destroy(gameObject);
        }
    }
}