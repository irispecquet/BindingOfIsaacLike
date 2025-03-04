using Player;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UILifeManager _uiLifeManager;
        
        private PlayerController _playerController;
        
        private void Start()
        {
            _playerController = GameManager.Instance.PlayerController;
            _playerController.RefreshLife += _uiLifeManager.RefreshHearts;
        }

        protected void OnDestroy()
        {
            _playerController.RefreshLife -= _uiLifeManager.RefreshHearts;
        }
    }
}