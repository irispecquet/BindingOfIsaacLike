using System.Collections.Generic;
using Entities.Player;
using Managers;
using Rooms;
using UnityEngine;

namespace Entities.Enemies
{
    public abstract class Follower : Enemy
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _pathUpdateInterval = 1.0f; // Time in seconds between path updates

        protected PlayerController _player;
        private PathFinder _pathFinder;
        private List<RoomNode> _currentPath;
        private int _currentPathIndex;
        private float _pathUpdateTimer;

        protected override void Start()
        {
            base.Start();
            _pathFinder = new PathFinder();
            _player = GameManager.Instance.PlayerController;
            _currentPath = new List<RoomNode>();
            _currentPathIndex = 0;
            _pathUpdateTimer = _pathUpdateInterval;
        }
        
        public virtual void Update()
        {
            _pathUpdateTimer += Time.deltaTime;
            if (_pathUpdateTimer >= _pathUpdateInterval || _currentPath == null || _currentPathIndex >= _currentPath.Count)
            {
                UpdatePath();
                _pathUpdateTimer = 0.0f;
            }

            if (_currentPath != null && _currentPathIndex < _currentPath.Count)
                MoveAlongPath();
        }

        private void UpdatePath()
        {
            RoomNode currentNode = GetCurrentRoomNode();
            RoomNode playerNode = GetPlayerRoomNode();

            if (currentNode != null && playerNode != null)
            {
                _currentPath = _pathFinder.GetPath(currentNode, playerNode);
                _currentPathIndex = 0;
            }
        }

        private void MoveAlongPath()
        {
            if (_currentPathIndex < _currentPath.Count)
            {
                RoomNode targetNode = _currentPath[_currentPathIndex];
                Vector3 targetPosition = targetNode.WorldPosition;
                _selfTransform.position = Vector3.MoveTowards(_selfTransform.position, targetPosition, _speed * Time.deltaTime);

                if (Vector3.Distance(_selfTransform.position, targetPosition) < 0.1f)
                    _currentPathIndex++;
            }
        }
        
        private RoomNode GetCurrentRoomNode()
        {
            Room room = GameManager.Instance.RoomManager.CurrentRoom;

            foreach (RoomNode node in room.Nodes)
            {
                if (node.ContainsPosition(transform.position))
                    return node;
            }

            return null;
        }

        private RoomNode GetPlayerRoomNode()
        {
            return _player.CurrentRoomNode;
        }
    }
}