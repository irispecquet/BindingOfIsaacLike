using DefaultNamespace;
using Player;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UILifeManager _uiLifeManager;
        
        private void Start()
        {
            GameManager.Instance.PlayerController.RefreshLife += _uiLifeManager.RefreshHearts;
        }

        protected void OnDestroy()
        {
            GameManager.Instance.PlayerController.RefreshLife -= _uiLifeManager.RefreshHearts;
        }
    }
}