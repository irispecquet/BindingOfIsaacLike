using System.Collections.Generic;
using CameraBehaviour;
using Entities;
using Entities.Enemies;
using LuniLib.Extensions;
using Managers;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        [Header("Room Properties")] 
        [SerializeField] private Transform _gridParent;
        [SerializeField] private Vector2 _roomSize;
        [SerializeField] private Vector2 _nodeSize;
        [SerializeField] private GameObject _nodePrefab;
        [SerializeField] private GameObject _poopPrefab;
        [SerializeField] private int _obstacleSpawnChance;
        [SerializeField] private int _enemySpawnChance;
        [SerializeField] private int _maxEnemyCount;
        [SerializeField] private bool _debugNodePosition;
        [SerializeField] private Door[] _roomDoors;
        [SerializeField] private CameraFollowProperties _cameraFollowProperties;

        [Header("Enemies")] 
        [SerializeField] private GameObject[] _enemyTypePool;
        
        public RoomNode[,] Nodes { get; private set; }
        public bool IsSet { get; private set; }

        private List<Enemy> _enemies = new();
        private bool _roomCleared;

        #region ROOM SETUP

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + _roomSize.x / 2, transform.position.y, 0));
            Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + _roomSize.y / 2, 0));
        }

        private void SetRoom()
        {
            int roomSizeX = Mathf.RoundToInt(_roomSize.x / _nodeSize.x);
            int roomSizeY = Mathf.RoundToInt(_roomSize.y / _nodeSize.y);

            Nodes = new RoomNode[roomSizeX, roomSizeY];

            for (int x = 0; x < Nodes.GetLength(0); x++)
            {
                for (int y = 0; y < Nodes.GetLength(1); y++)
                {
                    float xPosition = _gridParent.position.x + x * _nodeSize.x;
                    float yPosition = _gridParent.position.y + y * _nodeSize.y;

                    int randomOccupied = Random.Range(0, 100);
                    int randomEnemy = Random.Range(0, 100);

                    bool occupied = randomOccupied < _obstacleSpawnChance;
                    bool enemy = randomEnemy < _enemySpawnChance && !occupied && _enemies.Count < _maxEnemyCount;

                    if (_debugNodePosition)
                    {
                        TMP_Text newText = Instantiate(_nodePrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity).GetComponentInChildren<TMP_Text>();
                        newText.text = $"{x} ; {y}\n{xPosition.ToString("{0.0}")} {yPosition.ToString("{0.0}")}";
                        newText.color = occupied || enemy ? Color.red : Color.cyan;
                    }

                    if (occupied)
                        Instantiate(_poopPrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity);

                    if (enemy)
                    {
                        GameObject newEnemy = _enemyTypePool.RandomElement();
                        _enemies.Add(Instantiate(newEnemy, new Vector3(xPosition, yPosition, 0), Quaternion.identity).GetComponentInChildren<Enemy>());
                    }

                    CreateNode(x, y, new Vector3(xPosition, yPosition, 0), occupied);
                }
            }

            foreach (RoomNode node in Nodes)
                node.SetNeighbours(GetNodeNeighbour(node.Index.x, node.Index.y));

            _roomCleared = _enemies.Count <= 0;

            if (!_roomCleared)
            {
                foreach (Enemy enemy in _enemies)
                    enemy.DieEvent += RemoveEnemies;
            }
            else
            {
                OpenRoom();
            }

            IsSet = true;
        }

        private void CreateNode(int x, int y, Vector3 worldPosition, bool occupied)
        {
            Nodes[x, y] = new RoomNode(new Vector2Int(x, y), worldPosition, _nodeSize, occupied);
        }

        private RoomNode[] GetNodeNeighbour(int x, int y)
        {
            List<RoomNode> roomNodes = new List<RoomNode>();

            (int dx, int dy)[] directions =
            {
                (-1, 0),
                (1, 0),
                (0, -1),
                (0, 1),
                (-1, -1),
                (-1, 1),
                (1, 1),
                (1, -1)
            };

            foreach ((int dx, int dy) in directions)
            {
                int newX = x + dx;
                int newY = y + dy;

                if (newX >= 0 && newX < Nodes.GetLength(0) && newY >= 0 && newY < Nodes.GetLength(1) && !Nodes[newX, newY].IsOccupied)
                    roomNodes.Add(Nodes[newX, newY]);
            }

            return roomNodes.ToArray();
        }

        #endregion // ROOM SETUP

        private void Start()
        {
            foreach (Door door in _roomDoors)
                door.Init(this);
        }
        
        private void RemoveEnemies(Entity enemy)
        {
            _enemies.Remove(enemy as Enemy);
            enemy.DieEvent -= RemoveEnemies;

            if (_enemies.Count <= 0)
            {
                _roomCleared = true;
                OpenRoom();
            }
        }

        public void PlayerEnterRoom()
        {
            if(!IsSet)
                SetRoom();
            
            if (!_roomCleared)
            {
                foreach (Door door in _roomDoors)
                    door.SetDoorState(false);
            }
            
            GameManager.Instance.RoomManager.SetCameraProperties(_cameraFollowProperties);

            Debug.Log("Player enter!");
        }

        private void OpenRoom()
        {
            foreach (Door door in _roomDoors)
                door.SetDoorState(true);
            
            Debug.Log("Player can leave!");
            _roomCleared = true;
        }
    }
}