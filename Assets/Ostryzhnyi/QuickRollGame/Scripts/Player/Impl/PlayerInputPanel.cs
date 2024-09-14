using System;
using Ostryzhnyi.QuickRollGame.Scripts.Player.Api;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ostryzhnyi.QuickRollGame.Scripts.Player.Impl
{
    public class PlayerInputPanel : MonoBehaviour, IPlayerInput, IPointerDownHandler, IPointerUpHandler
    {
        private bool _isClicked;

        private void FixedUpdate()
        {
            if (_isClicked)
                OnTapping?.Invoke();
        }

        public event Action OnStartTap;
        public event Action OnTapping;
        public event Action OnEndTap;

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