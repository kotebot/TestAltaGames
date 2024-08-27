using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace WindowsService.ResultWindows.BaseClasses
{
    public abstract class ResultPopup: MonoBehaviour
    {

        [SerializeField, ] private Button _restart;

        protected virtual void OnEnable()
        {
            _restart.onClick.AddListener(OnRestart);
        }

        protected virtual void OnDisable()
        {
            _restart.onClick.RemoveListener(OnRestart);
        }
    
        private void OnRestart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}