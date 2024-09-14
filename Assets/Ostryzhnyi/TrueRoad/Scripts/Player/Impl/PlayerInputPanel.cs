using System;
using Player.Api;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player.Impl
{
    public class PlayerInputPanel : MonoBehaviour, IPlayerInput, IPointerDownHandler, IPointerUpHandler
    {
        public event Action OnStartTap;
        public event Action OnTapping;
        public event Action OnEndTap;

        private bool _isClicked = false;

        private void FixedUpdate()
        {
            if(_isClicked)
                OnTapping?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnStartTap?.Invoke();
            _isClicked = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isClicked = false;
            OnEndTap?.Invoke();
        }
    }
}