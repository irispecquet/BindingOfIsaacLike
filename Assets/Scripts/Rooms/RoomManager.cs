using System.Threading.Tasks;
using CameraBehaviour;
using Entities;
using Entities.Player;
using Managers;
using UnityEngine;

namespace Rooms
{
    public class RoomManager : MonoBehaviour
    {
        [SerializeField] private Room _startingRoom;
        [SerializeField] private CameraFollow _cameraFollow;

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
        }

        private void OnPlayerDied(Entity player)
        {
            Debug.Log("Player died!");
        }

        public async void ChangeRoom(Room room, Vector3 playerPosition)
        {
            bool roomCanChange = false;

            GameManager.Instance.UIManager.FadeIn(() => roomCanChange = true);

            while (!roomCanChange)
                await Task.Delay(1);

            CurrentRoom = room;
            room.PlayerEnterRoom();
            _player.transform.position = playerPosition;

            GameManager.Instance.UIManager.FadeOut();
        }

        public void SetCameraProperties(CameraFollowProperties properties)
        {
            _cameraFollow.SetProperties(properties, GameManager.Instance.PlayerController.transform);
        }
    }
}