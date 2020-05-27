using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ecosystem.UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private CanvasGroup pauseMenuPanel;
        [SerializeField] private SceneField mainMenuScene;

        private float pausedTimeScale = 1f;
        
        private void Start()
        {
            Pause(false);
        }

        public void MainMenu()
        {
            DisposeEntities();

            SceneManager.LoadScene(mainMenuScene);
        }

        public void TogglePause()
        {
            bool isPaused = pauseMenuPanel.gameObject.activeSelf;
            Pause(!isPaused);
        }

        private void Pause(bool value)
        {
            if (value) pausedTimeScale = Time.timeScale;
            Time.timeScale = value ? 0 : pausedTimeScale;
            pauseMenuPanel.gameObject.SetActive(value);
        }

        public void OnTogglePause() => TogglePause();

        private void DisposeEntities()
        {
            var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            entityManager.DestroyEntity(entityManager.UniversalQuery);
        }
    }
}
