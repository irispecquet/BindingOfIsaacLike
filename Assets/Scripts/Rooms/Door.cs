using Entities.Player;
using Managers;
using UnityEngine;

namespace Rooms
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private GameObject _openedDoorBackgroundObject;
        [SerializeField] private GameObject _closedDoorBackgroundObject;
        [SerializeField] private Transform _playerSpawnTransform;
        [SerializeField] private Door _joiningDoor;
        
        private bool _isOpened;
        private Room _room;

        public void Init(Room room)
        {
            _room = room;
        }
        
        public void SetDoorState(bool opened)
        {
            _openedDoorBackgroundObject.SetActive(opened);
            _closedDoorBackgroundObject.SetActive(!opened);

            _isOpened = opened;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isOpened && other.GetComponent<PlayerController>())
                GameManager.Instance.RoomManager.ChangeRoom(_joiningDoor._room, _joiningDoor._playerSpawnTransform.position);
        }
    }
}