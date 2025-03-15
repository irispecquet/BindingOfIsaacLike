using UnityEngine;

namespace Rooms
{
    public class RoomManager : MonoBehaviour
    {
        [field:SerializeField] public Room CurrentRoom { get; private set; }
    }
}