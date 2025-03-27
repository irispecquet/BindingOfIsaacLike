using Entities.Player;
using UnityEngine;

namespace Items
{
    public class Healer : PickUp
    {
        [SerializeField] private int _healValue;
        
        protected override void Interact(PlayerController player)
        {
            base.Interact(player);
            player.Heal(_healValue);
        }
    }
}