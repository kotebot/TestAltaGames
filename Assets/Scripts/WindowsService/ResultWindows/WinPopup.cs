using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using WindowsService.ResultWindows.BaseClasses;

namespace WindowsService.ResultWindows
{
    public class WinPopup : ResultPopup
    {
        [SerializeField, FoldoutGroup("Setup")] private Button _nextLevel;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _nextLevel.onClick.AddListener(LoadNextLevel);

            if(SceneManager.sceneCount == SceneManager.GetActiveScene().buildIndex)
                _nextLevel.gameObject.SetActive(false);
        }
        
        protected override void OnDisable()
        {
            _nextLevel.onClick.RemoveListener(LoadNextLevel);
            
            base.OnDisable();
        }

        private void LoadNextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}