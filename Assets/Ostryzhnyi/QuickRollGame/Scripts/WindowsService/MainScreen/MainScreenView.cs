using Ostryzhnyi.DI;
using Ostryzhnyi.QuickRollGame.Scripts.Player.Api;
using UnityEngine;
using UnityEngine.UI;

namespace Ostryzhnyi.QuickRollGame.Scripts.WindowsService.MainScreen
{
    public class MainScreenView : MonoBehaviour
    {
        [SerializeField] private Button _play;
        [SerializeField] private GameObject _windows;
        [SerializeField] private GameObject _winPopup;
        [SerializeField] private GameObject _loosePopup;
        [Inject] private IPlayer _player;
        [Inject] private IMovement _playerMovement;

        private void Start()
        {
            _play.onClick.AddListener(_playerMovement.Move);
            _player.OnDead += OnDead;
            _player.OnWin += OnWin;
        }

        private void OnDestroy()
        {
            _play.onClick.RemoveListener(_playerMovement.Move);
            _player.OnDead -= OnDead;
            _player.OnWin -= OnWin;
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