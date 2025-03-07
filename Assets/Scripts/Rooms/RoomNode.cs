using System.Collections.Generic;
using UnityEngine;

namespace Rooms
{
    public class RoomNode
    {
        public RoomNode[] Neighbours { get; private set; }
        public Vector2 WorldPosition { get; private set; }

        public RoomNode(Vector2 worldPosition, RoomNode[] neighbours)
        {
            WorldPosition = worldPosition;
            Neighbours = neighbours;
        }
    }
}