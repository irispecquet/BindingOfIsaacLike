using Entities.Player;
using UnityEngine;

namespace Items
{
    public abstract class PickUp : MonoBehaviour
    {
        [SerializeField] private Transform _selfTransform;
        
        public void Pick(PlayerController player)
        {
            if(TryInteract(player))
                Destroy(_selfTransform.gameObject);
        }

        protected virtual bool TryInteract(PlayerController player)
        {
            return true;
        }
    }
}