using Ostryzhnyi.DI;
using Player.Api;
using UnityEngine;

namespace Player.Impl
{
    public class Road : MonoBehaviour
    {
        [Inject] private IPlayer _player;
        [Inject] private IPlayerInput _input;
        
        private float _defaultRadius;

        #region UnityMethods

        private void Start()
        {
            _input.OnTapping += OnChangeSize;

            _defaultRadius = _player.Radius;
        }
        
        private void OnDestroy()
        {
            _input.OnTapping -= OnChangeSize;
        }

        #endregion
       
        private void OnChangeSize()
        {
            var size = _player.Radius / _defaultRadius ;
            
            transform.localScale = new Vector3(size, transform.localScale.y, transform.localScale.z);
        }

    }
}