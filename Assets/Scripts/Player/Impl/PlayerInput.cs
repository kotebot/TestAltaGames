using System;
using Player.Api;
using UnityEngine;

namespace Player.Impl
{
    public class PlayerInput : MonoBehaviour, IPlayerInput
    {
        public event Action OnStartTap;
        public event Action OnTapping;
        public event Action OnEndTap;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Start tap");
                OnStartTap?.Invoke();
            }
            else if (Input.GetMouseButton(0))
            {
                Debug.Log("Tapping");

                OnTapping?.Invoke();
            }
            else if(Input.GetMouseButtonUp(0))
            {
                Debug.Log("End tap");

                OnEndTap?.Invoke();
            }
        }
    }
}
