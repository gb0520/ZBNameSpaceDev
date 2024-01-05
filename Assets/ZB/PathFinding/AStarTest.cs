using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ZB.PathFind
{
    public class AStarTest : MonoBehaviour
    {
        [SerializeField] private Vector2 startPos;
        [SerializeField] private Vector2 goalPos;
        [SerializeField] private Vector2 mapSize;
        [SerializeField] private Vector2[] walls;
        [SerializeField] private Vector2[] path;

        [ContextMenu("GETPATH")]
        public void GetPath()
        {
            if (!AStar.TryGetPath(startPos, goalPos, mapSize, walls, out path))
            {
                Debug.LogError("!");
            }
        }
    }
}