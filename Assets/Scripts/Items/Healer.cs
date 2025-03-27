using Entities.Player;
using UnityEngine;

namespace Items
{
    public class Healer : PickUp
    {
        [SerializeField] private int _healValue;

        protected override bool TryInteract(PlayerController player)
        {
            return player.TryHeal(_healValue);
        }
    }
}