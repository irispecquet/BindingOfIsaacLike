using LuniLib.UnityUtils;
using LuniLib.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private SceneField _gameplayScene;
        [SerializeField] private InputsHandler _inputsHandler;
        [SerializeField] private Fade _fade;

        private void Start()
        {
            _inputsHandler.ConfirmEvent += LoadScene;
            _fade.FadeOut();
        }

        private void LoadScene()
        {
            _inputsHandler.ConfirmEvent -= LoadScene; 
            _fade.FadeIn(() => SceneManager.LoadScene(_gameplayScene));
        }
    }
}