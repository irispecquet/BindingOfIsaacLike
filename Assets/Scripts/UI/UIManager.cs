using System;
using Entities.Player;
using LuniLib.View;
using Managers;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UILifeManager _uiLifeManager;
        [SerializeField] private Fade _fade;
        
        private PlayerController _playerController;
        
        private void Start()
        {
            _playerController = GameManager.Instance.PlayerController;
            _playerController.RefreshLife += _uiLifeManager.RefreshHearts;
        }
        
        public void FadeIn(Action onComplete = null) => _fade.FadeIn(onComplete);
        public void FadeOut(Action onComplete = null) => _fade.FadeOut(onComplete);

        protected void OnDestroy()
        {
            _playerController.RefreshLife -= _uiLifeManager.RefreshHearts;
        }
    }
}