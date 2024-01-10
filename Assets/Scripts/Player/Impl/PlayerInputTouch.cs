using System;
using Player.Api;
using UnityEngine;

namespace Player.Impl
{
    [Obsolete("Used only for debug. Include tap on all screen.")]
    public class PlayerInputTouch : MonoBehaviour, IPlayerInput
    {
        public event Action OnStartTap;
        public event Action OnTapping;
        public event Action OnEndTap;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnStartTap?.Invoke();
            }
            else if (Input.GetMouseButton(0))
            {
                OnTapping?.Invoke();
            }
            else if(Input.GetMouseButtonUp(0))
            {
                OnEndTap?.Invoke();
            }
        }
    }
}
