using System.Collections;
using Ostryzhnyi.QuickRollGame.Scripts.Player.Api;
using UnityEngine;

namespace Ostryzhnyi.QuickRollGame.Scripts.Player.Impl.BaseClasses
{
    public abstract class BaseMovement : MonoBehaviour, IMovement
    {
        [SerializeField] [Range(0, 100)] private float _speed;

        public bool IsMoving { get; private set; }

        public void Move()
        {
            StartCoroutine(MovementRoutine());
        }

        protected abstract bool CanMove();

        private IEnumerator MovementRoutine()
        {
            IsMoving = true;

            while (CanMove())
            {
                transform.Translate(Vector3.forward * (Time.deltaTime * _speed));
                yield return new WaitForEndOfFrame();
            }

            IsMoving = false;
        }
    }
}