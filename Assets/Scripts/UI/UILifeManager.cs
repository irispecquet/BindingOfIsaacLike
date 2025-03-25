using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UILifeManager : MonoBehaviour
    {
        [SerializeField] private Sprite _fullHeart;
        [SerializeField] private Sprite _semiHeart;
        [SerializeField] private Sprite _emptyHeart;
        [SerializeField] private Image[] _allHeartImages;

        public void RefreshHearts(int life)
        {
            if (life < 0)
                return;
            
            bool isEven = life % 2 == 0;
            int total = life / 2;

            foreach (Image heartImage in _allHeartImages)
                heartImage.sprite = _emptyHeart;
            
            for (int i = 0; i < total; i++)
                _allHeartImages[i].sprite = _fullHeart;
            
            if (!isEven)
                _allHeartImages[total].sprite = _semiHeart;
        }
    }
}