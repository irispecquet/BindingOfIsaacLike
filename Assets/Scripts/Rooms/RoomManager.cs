using Entities;
using Entities.Player;
using Managers;
using UnityEngine;

namespace Rooms
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private Room _startingRoom;
        
        public Room CurrentRoom { get; private set; }

        private PlayerController _player;

        private void Awake()
        {
            CurrentRoom = _startingRoom;
        }

        private void Start()
        {
            _player = GameManager.Instance.PlayerController;
            _player.DieEvent += OnPlayerDied;
            
            ChangeRoom(_startingRoom, _startingRoom.transform.position);
            _startingRoom.SetRoom();
        }

        private void OnPlayerDied(Entity player)
        {
            Debug.Log("Player died!");
        }

        public void ChangeRoom(Room room, Vector3 playerPosition) 
        {
            GameManager.Instance.UIManager.FadeIn(() => GameManager.Instance.UIManager.FadeOut());
            CurrentRoom = room;
            _player.transform.position = playerPosition;
        }
    }
}