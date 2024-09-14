using Ostryzhnyi.DI;
using Player.Api;
using UnityEngine;
using UnityEngine.UI;

namespace WindowsService.MainScreen
{
    public class MainScreenView : MonoBehaviour
    {
        [Inject] private IMovement _playerMovement;
        [Inject] private IPlayer _player;
        
        [SerializeField] private Button _play = default;
        [SerializeField] private GameObject _windows = default;
        [SerializeField] private GameObject _winPopup = default;
        [SerializeField] private GameObject _loosePopup = default;

        private void Start()
        {
            _play.onClick.AddListener(_playerMovement.Move);
            _player.OnDead += OnDead;
            _player.OnWin  += OnWin;
        }
        private void OnDestroy()
        {
            _play.onClick.RemoveListener(_playerMovement.Move);
            _player.OnDead -= OnDead;
            _player.OnWin  -= OnWin;
        }

        private void OnDead()
        {
            _windows.gameObject.SetActive(true);
            _winPopup.gameObject.SetActive(false);
            _loosePopup.gameObject.SetActive(true);
        }

        private void OnWin()
        {
            _windows.gameObject.SetActive(true);
            _winPopup.gameObject.SetActive(true);
            _loosePopup.gameObject.SetActive(false);
        }
    }
}
