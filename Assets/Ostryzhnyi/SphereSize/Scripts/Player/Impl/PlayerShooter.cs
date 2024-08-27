using System;
using Player.Api;
using Player.Impl.Bullets;
using UnityEngine;
using Ostryzhnyi.DI;

namespace Player.Impl
{
    public class PlayerShooter : MonoBehaviour, IPlayerShooter//can remove mono beh and move to zeject container
    {
        [Inject] private IPlayerInput _input;
        [Inject] private IPlayer _player;
        [Inject] private IMovement _playerMovement;
        [Inject] private DIContainer _diContainer;
        
        [SerializeField]//if move to default c#, all setting must be move to ShottingSettigns and bind this class
        private Bullet _bulletPrefab;
        [SerializeField]
        private Transform _spawnBulletPosition;
        
        [SerializeField, Range(0.001f, 10)]
        private float _changeSizeSpeed;

        private IBullet _activeBullet;

        #region UnityMethods

        private void OnEnable()
        {
            _input.OnStartTap += Spawn;
            _input.OnEndTap   += Shoot;
            _input.OnTapping  += ChangeSize;
        }

        private void OnDisable()
        {
            _input.OnStartTap -= Spawn;
            _input.OnEndTap   -= Shoot;
            _input.OnTapping  -= ChangeSize;
        }

        #endregion
      
        public void Spawn()
        {
            if (_playerMovement.IsMoving)
                return;
            
            _activeBullet = Instantiate(_bulletPrefab, _spawnBulletPosition);
            _diContainer.InjectDependencies(_activeBullet);
            _player.Lower(_activeBullet.Radius);
        }

        public void Shoot()
        {
            if (_playerMovement.IsMoving)
                return;
            
            _activeBullet.Move();

            _activeBullet = null;
        }

        private void ChangeSize()
        {
            if (_playerMovement.IsMoving)
                return;
            
            if (_activeBullet != null)
            {
                _activeBullet.Upper(_changeSizeSpeed);
            }
            else
            {
                Debug.LogError("Active bullet is null");
            }
            
            _player.Lower(_changeSizeSpeed);
        }
    }
}