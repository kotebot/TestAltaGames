using System;
using Core.WindowsService.Api.Service;
using Player.Api;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using WindowsService.ResultWindows;
using Zenject;

namespace WindowsService.MainScreen
{
    public class MainScreenView : MonoBehaviour
    {
        [Inject] private IMovement _playerMovement;
        [Inject] private IPlayer _player;
        [Inject] private IWindowsService _windowsService;
        
        
        [FoldoutGroup("Setup"), SerializeField, ChildGameObjectsOnly]
        private Button _play = default;

        private void OnEnable()
        {
            _play.onClick.AddListener(_playerMovement.Move);
            _player.OnDead += OnDead;
            _player.OnWin  += OnWin;
        }
        private void OnDisable()
        {
            _play.onClick.RemoveListener(_playerMovement.Move);
            _player.OnDead -= OnDead;
            _player.OnWin  -= OnWin;
        }

        private void OnDead()
        {
            _windowsService.ShowWindow<LoosePopup>();
        }

        private void OnWin()
        {
            _windowsService.ShowWindow<WinPopup>();
        }
    }
}
