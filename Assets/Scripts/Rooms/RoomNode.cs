using UnityEngine;

namespace Rooms
{
    public class RoomNode
    {
        public RoomNode[] Neighbours { get; private set; }
        public Vector2 WorldPosition { get; private set; }
        public Vector2Int Index { get; private set; }
        public bool IsOccupied;
        
        private readonly Vector2 _size;

        public RoomNode(Vector2Int index, Vector2 worldPosition, Vector2 size, bool isOccupied)
        {
            Index = index;
            WorldPosition = worldPosition;
            _size = size;
            IsOccupied = isOccupied;
        }

        public void SetNeighbours(RoomNode[] neighbours)
        {
            Neighbours = neighbours;
        }
        
        public bool ContainsPosition(Vector3 position)
        {
            return position.x >= WorldPosition.x - _size.x / 2 &&
                   position.x <= WorldPosition.x + _size.x / 2 &&
                   position.y >= WorldPosition.y - _size.y / 2 &&
                   position.y <= WorldPosition.y + _size.y / 2;
        }
    }
}