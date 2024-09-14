using System;
using Ostryzhnyi.QuickRollGame.Scripts.Blockers.Api;
using Ostryzhnyi.QuickRollGame.Scripts.Player.Api;
using UnityEngine;

namespace Ostryzhnyi.QuickRollGame.Scripts.Player.Impl
{
    [RequireComponent(typeof(Collider))]
    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField] [Range(0.01f, 10)] private float _minimumAliveRadius;
        private bool _isAlive = true;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent<IBlocker>(out var blocker))
                Dead();
            else if (collision.gameObject.TryGetComponent<Finish.Finish>(out var finish)) Finish();
        }

        public event Action OnDead;
        public event Action OnWin;

        public bool IsAlive
        {
            get => _isAlive;
            private set
            {
                _isAlive = value;

                if (_isAlive == false)
                    OnDead?.Invoke();
            }
        }

        public bool IsWinner { get; private set; }

        public float Radius => transform.localScale.x;

        public void Lower(float multiplier)
        {
            if (!IsAlive)
                return;

            transform.localScale -= Vector3.one * multiplier;

            if (Radius <= _minimumAliveRadius)
                Dead();
        }

        private void Dead()
        {
            IsAlive = false;
        }

        private void Finish()
        {
            IsWinner = true;
            OnWin?.Invoke();
        }
    }
}