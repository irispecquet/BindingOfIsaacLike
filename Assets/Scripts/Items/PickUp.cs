using Entities.Player;
using UnityEngine;

namespace Items
{
    public abstract class PickUp : MonoBehaviour
    {
        [SerializeField] private Transform _selfTransform;
        
        public void Pick(PlayerController player)
        {
            Interact(player);
            Destroy(_selfTransform.gameObject);
        }

        protected virtual void Interact(PlayerController player)
        {
            Debug.Log($"Interacting with {name}");
        }
    }
}