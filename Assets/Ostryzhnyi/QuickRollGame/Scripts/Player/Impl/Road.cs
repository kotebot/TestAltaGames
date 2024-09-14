using Ostryzhnyi.DI;
using Ostryzhnyi.QuickRollGame.Scripts.Player.Api;
using UnityEngine;

namespace Ostryzhnyi.QuickRollGame.Scripts.Player.Impl
{
    public class Road : MonoBehaviour
    {
        private float _defaultRadius;
        [Inject] private IPlayerInput _input;
        [Inject] private IPlayer _player;

        private void OnChangeSize()
        {
            var size = _player.Radius / _defaultRadius;

            transform.localScale = new Vector3(size, transform.localScale.y, transform.localScale.z);
        }

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
    }
}