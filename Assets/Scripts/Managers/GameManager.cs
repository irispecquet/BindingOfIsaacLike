using Entities.Player;
using LuniLib.SingletonClassBase;
using Rooms;
using UI;
using UnityEngine;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        [field:SerializeField] public PlayerController PlayerController { get; private set; }
        [field:SerializeField] public RoomManager RoomManager { get; private set; }
        [field:SerializeField] public UIManager UIManager { get; private set; }
        
        protected override void InternalAwake()
        {
        }
    }
}