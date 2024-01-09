using Player.Impl;
using UnityEngine;
using Zenject;

namespace Player.Zenject
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private Impl.Player _player;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerShooter _playerShooter;
        
        public override void InstallBindings()
        {
            Container
                .BindInterfacesTo<PlayerInput>()
                .FromInstance(_playerInput)
                .AsSingle();
            
            Container
                .BindInterfacesTo<Impl.Player>()
                .FromInstance(_player)
                .AsSingle();
            
            Container
                .BindInterfacesTo<PlayerMovement>()
                .FromInstance(_playerMovement)
                .AsSingle()
                .Lazy();
            
            Container
                .BindInterfacesTo<PlayerShooter>()
                .FromInstance(_playerShooter)
                .AsSingle()
                .Lazy();
        }
    }
}