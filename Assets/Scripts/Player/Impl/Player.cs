using System;
using Blockers.Api;
using Player.Api;
using UnityEngine;

namespace Player.Impl
{
    [RequireComponent(typeof(Collider))]
    public class Player: MonoBehaviour, IPlayer
    {
        public event Action OnDead;
        public bool IsAlive { get; private set; }
        public float Radius => transform.localScale.x;

        public void Lower(float multiplier)
        {
            transform.localScale -= (Vector3.one * multiplier);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<IBlocker>(out var blocker))
            {
                Dead();
            }
        }

        private void Dead()
        {
            IsAlive = false;
            OnDead?.Invoke();
        }
    }
}