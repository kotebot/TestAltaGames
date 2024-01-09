using System.Collections;
using Player.Api;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player.Impl.BaseClasses
{
    public abstract class BaseMovement : MonoBehaviour, IMovement
    {
        [BoxGroup("Settings"), SerializeField, Range(0, 100)] private float _speed;
        
        public void Move()
        {
            StartCoroutine(MovementRoutine());
        }

        protected abstract bool CanMove();

        private IEnumerator MovementRoutine()
        {
            while (CanMove())
            {
                transform.Translate(Vector3.forward * (Time.deltaTime * _speed));
                yield return new WaitForEndOfFrame();
            }
            
            yield break;
        }
    }
}