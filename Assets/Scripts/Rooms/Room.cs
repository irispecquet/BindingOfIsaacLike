using System.Collections.Generic;
using UnityEngine;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Vector2 _roomSize;
        [SerializeField] private Vector2 _nodeSize;
        [SerializeField] private GameObject _nodePrefab;
        [SerializeField] private bool _debugNodePosition;

        private RoomNode[,] _nodes;

        private void Start()
        {
            int roomSizeX = Mathf.RoundToInt(_roomSize.x);
            int roomSizeY = Mathf.RoundToInt(_roomSize.y);

            _nodes = new RoomNode[roomSizeX, roomSizeY];

            for (int x = 0; x < _nodes.GetLength(0); x++)
            {
                for (int y = 0; y < _nodes.GetLength(1); y++)
                {
                    float xPosition = transform.position.x + x * _nodeSize.x;
                    float yPosition = transform.position.y + y * _nodeSize.y;

                    if (_debugNodePosition)
                        Instantiate(_nodePrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity);

                    CreateNode(x, y);
                }
            }
        }

        private void CreateNode(int x, int y)
        {
            List<RoomNode> roomNodes = new List<RoomNode>();

            (int dx, int dy)[] directions =
            {
                (-1, 0),
                (1, 0),
                (0, -1),
                (0, 1)
            };

            foreach ((int dx, int dy) in directions)
            {
                int newX = x + dx;
                int newY = y + dy;

                if (newX >= 0 && newX < _nodes.GetLength(0) && newY >= 0 && newY < _nodes.GetLength(1))
                    roomNodes.Add(_nodes[newX, newY]);
            }

            _nodes[x, y] = new RoomNode(new Vector2(x, y), roomNodes.ToArray());
        }
    }
}