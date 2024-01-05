using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ZB.PathFind
{
    public class AStar
    {
        public static bool TryGetPath(Vector2 nowPos, Vector2 goalPos, Vector2 mapSize, Vector2[] walls, out Vector2[] path)
        {
            path = null;

            // 유효성 검사: 시작점과 목표점이 유효한지 확인
            if (!IsWithinBounds(nowPos, mapSize) || !IsWithinBounds(goalPos, mapSize))
                return false;

            // 유효성 검사: 시작점과 목표점이 벽에 위치하는지 확인
            if (IsBlocked(nowPos, walls) || IsBlocked(goalPos, walls))
                return false;

            // A* 알고리즘을 사용하여 경로 찾기
            List<Vector2> openSet = new List<Vector2>();
            HashSet<Vector2> closedSet = new HashSet<Vector2>();
            Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();
            Dictionary<Vector2, float> gScore = new Dictionary<Vector2, float>();

            openSet.Add(nowPos);
            gScore[nowPos] = 0;

            while (openSet.Count > 0)
            {
                Vector2 current = GetLowestFScore(openSet, gScore);
                openSet.Remove(current);
                closedSet.Add(current);

                if (current == goalPos)
                {
                    path = ReconstructPath(cameFrom, goalPos);
                    return true;
                }

                foreach (Vector2 neighbor in GetNeighbors(current, mapSize, walls))
                {
                    if (closedSet.Contains(neighbor))
                        continue;

                    float tentativeGScore = gScore[current] + Vector2.Distance(current, neighbor);

                    if (!openSet.Contains(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }

            // 경로를 찾을 수 없는 경우
            return false;
        }

        private static Vector2 GetLowestFScore(List<Vector2> openSet, Dictionary<Vector2, float> gScore)
        {
            float lowestFScore = float.MaxValue;
            Vector2 lowestNode = Vector2.zero;

            foreach (Vector2 node in openSet)
            {
                if (gScore.TryGetValue(node, out float currentFScore) && currentFScore < lowestFScore)
                {
                    lowestFScore = currentFScore;
                    lowestNode = node;
                }
            }

            return lowestNode;
        }

        private static Vector2[] ReconstructPath(Dictionary<Vector2, Vector2> cameFrom, Vector2 current)
        {
            List<Vector2> path = new List<Vector2>();

            while (cameFrom.ContainsKey(current))
            {
                path.Insert(0, current);
                current = cameFrom[current];
            }

            path.Insert(0, current); // Add the starting point

            return path.ToArray();
        }

        private static IEnumerable<Vector2> GetNeighbors(Vector2 position, Vector2 mapSize, Vector2[] walls)
        {
            Vector2[] possibleNeighbors = new Vector2[]
            {
            new Vector2(position.x + 1, position.y),
            new Vector2(position.x - 1, position.y),
            new Vector2(position.x, position.y + 1),
            new Vector2(position.x, position.y - 1),
            };

            foreach (Vector2 neighbor in possibleNeighbors)
            {
                if (IsWithinBounds(neighbor, mapSize) && !IsBlocked(neighbor, walls))
                    yield return neighbor;
            }
        }

        private static bool IsWithinBounds(Vector2 position, Vector2 mapSize)
        {
            return position.x >= 0 && position.x < mapSize.x && position.y >= 0 && position.y < mapSize.y;
        }

        private static bool IsBlocked(Vector2 position, Vector2[] walls)
        {
            foreach (Vector2 wall in walls)
            {
                if (position.x == wall.x && position.y == wall.y)
                    return true;
            }

            return false;
        }
    }
}
            