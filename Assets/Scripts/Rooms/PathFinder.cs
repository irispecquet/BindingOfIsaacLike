using System;
using System.Collections.Generic;
using System.Linq;
using LuniLib.Algorithms.Pathfinding.AStar;
using UnityEngine;

namespace Rooms
{
    public class PathFinder
    {
        public List<RoomNode> GetPath(RoomNode originNode, RoomNode targetNode)
        {
            return AStar.GetPath(originNode, targetNode, GetDistance, GetHeuristicDistance, GetNeighbours);
        }

        private float GetHeuristicDistance(RoomNode originNode, RoomNode targetNode)
        {
            double xDistance = originNode.WorldPosition.x - targetNode.WorldPosition.x;
            double yDistance = originNode.WorldPosition.y - targetNode.WorldPosition.y;

            return (float)(Math.Abs(xDistance) + Math.Abs(yDistance));
        }

        private RoomNode[] GetNeighbours(RoomNode node)
        {
            return node.Neighbours.Where(neighbor => neighbor != null && !neighbor.IsOccupied).ToArray();
        }

        private float GetDistance(RoomNode originNode, RoomNode targetNode)
        {
            return Vector2.Distance(originNode.WorldPosition, targetNode.WorldPosition);
        }
    }
}