using Ostryzhnyi.DI;
using Ostryzhnyi.QuickRollGame.Scripts.Map.Api;
using Ostryzhnyi.QuickRollGame.Scripts.WindowsService.ResultWindows.BaseClasses;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ostryzhnyi.QuickRollGame.Scripts.WindowsService.ResultWindows
{
    public class WinPopup : ResultPopup
    {
        [SerializeField] private Button _nextLevel;
        [Inject] private ILevelNumber _levelNumber;

        protected override void OnEnable()
        {
            base.OnEnable();

            _nextLevel.onClick.AddListener(LoadNextLevel);

            if (SceneManager.sceneCount == SceneManager.GetActiveScene().buildIndex)
                _nextLevel.gameObject.SetActive(false);
        }

        protected override void OnDisable()
        {
            _nextLevel.onClick.RemoveListener(LoadNextLevel);

            base.OnDisable();
        }

        private void LoadNextLevel()
        {
            _levelNumber.LevelNumber++;
            SceneManager.LoadScene(0);
        }
    }
}