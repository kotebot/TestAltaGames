using Ostryzhnyi.DI;
using Ostryzhnyi.SphereSize.Scripts.Map;
using Ostryzhnyi.SphereSize.Scripts.Map.Api;
using Ostryzhnyi.SphereSize.Scripts.Map.Impl;
using Player.Api;
using Player.Impl;
using UnityEngine;

namespace Ostryzhnyi.SphereSize.Scripts.DI
{
    public class SphereSizeInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInputPanel _playerInputTouch;
        [SerializeField] private Player.Impl.Player _player;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerShooter _playerShooter;
        [SerializeField] private LevelData _levelData;
        [SerializeField] private MapCreator _mapCreator;
        
        protected override void Register()
        {
            Container.Register<IPlayer>(_player);

            Container.Register<IPlayerInput>(_playerInputTouch);
            
            Container.Register<IMovement>(_playerMovement);

            Container.Register<IPlayerShooter>(_playerShooter);
            
            Container.Register<LevelData>(_levelData);
            
            Container.Register<ILevelNumber>(_mapCreator);
        }
    }
}