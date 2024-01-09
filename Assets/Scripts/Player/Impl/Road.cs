using System;
using Player.Api;
using UnityEngine;
using Zenject;

namespace Player.Impl
{
    public class Road : MonoBehaviour
    {
        [Inject] private IPlayer _player;
        [Inject] private IPlayerInput _input;

        #region UnityMethods

        private void OnEnable()
        {
            _input.OnTapping += OnChangeSize;
        }
        
        private void OnDisable()
        {
            _input.OnTapping -= OnChangeSize;
        }

        #endregion
       
        private void OnChangeSize()
        {
            transform.localScale = new Vector3(_player.Radius, transform.localScale.y, transform.localScale.z);
        }

    }
}