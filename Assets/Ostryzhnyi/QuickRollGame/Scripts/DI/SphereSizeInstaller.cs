using Ostryzhnyi.DI;
using Ostryzhnyi.QuickRollGame.Scripts.Map;
using Ostryzhnyi.QuickRollGame.Scripts.Map.Api;
using Ostryzhnyi.QuickRollGame.Scripts.Map.Data;
using Ostryzhnyi.QuickRollGame.Scripts.Map.Impl;
using Ostryzhnyi.QuickRollGame.Scripts.Player.Api;
using Ostryzhnyi.QuickRollGame.Scripts.Player.Impl;
using UnityEngine;

namespace Ostryzhnyi.QuickRollGame.Scripts.DI
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

            Container.Register(_levelData);

            Container.Register<ILevelNumber>(_mapCreator);
        }
    }
}