using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Vector2 _roomSize;
        [SerializeField] private Vector2 _nodeSize;
        [SerializeField] private GameObject _nodePrefab;
        [SerializeField] private bool _debugNodePosition;

        public RoomNode[,] Nodes { get; private set; }

        private void Awake()
        {
            int roomSizeX = Mathf.RoundToInt(_roomSize.x / _nodeSize.x);
            int roomSizeY = Mathf.RoundToInt(_roomSize.y / _nodeSize.y);

            Nodes = new RoomNode[roomSizeX, roomSizeY];

            for (int x = 0; x < Nodes.GetLength(0); x++)
            {
                for (int y = 0; y < Nodes.GetLength(1); y++)
                {
                    float xPosition = transform.position.x + x * _nodeSize.x;
                    float yPosition = transform.position.y + y * _nodeSize.y;
                    int randomOccupied = Random.Range(0, 6);
                    bool occupied = randomOccupied == 1;

                    if (_debugNodePosition)
                    {
                        TMP_Text newText = Instantiate(_nodePrefab, new Vector3(xPosition, yPosition, 0), Quaternion.identity).GetComponentInChildren<TMP_Text>();
                        newText.text = $"{x} ; {y}\n{xPosition} ; {yPosition}";
                        newText.color = occupied ? Color.red : Color.cyan;
                    }

                    CreateNode(x, y, new Vector3(xPosition, yPosition, 0), occupied);
                }
            }

            foreach (RoomNode node in Nodes)
                node.SetNeighbours(GetNodeNeighbour(node.Index.x, node.Index.y));
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
    }
}