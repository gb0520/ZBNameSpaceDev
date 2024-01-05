using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ZB.PathFind
{
    public class AStar
    {
        public static Vector2[] GetPath(Vector2 nowPos, Vector2 goalPos, Vector2[] walls)
        {
            // A* 알고리즘 구현
            List<Vector2> path = new List<Vector2>();
            HashSet<Vector2> closedSet = new HashSet<Vector2>();
            HashSet<Vector2> openSet = new HashSet<Vector2> { nowPos };
            Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();

            // 시작 노드의 초기 예상 비용
            Dictionary<Vector2, float> gScore = new Dictionary<Vector2, float>
        {
            { nowPos, 0 }
        };

            // 시작 노드의 전체 예상 비용
            Dictionary<Vector2, float> fScore = new Dictionary<Vector2, float>
        {
            { nowPos, HeuristicCostEstimate(nowPos, goalPos) }
        };

            while (openSet.Count > 0)
            {
                // 현재 노드 중에서 예상 비용이 가장 적은 노드 선택
                Vector2 current = openSet.OrderBy(node => fScore[node]).First();

                // 목표에 도달한 경우 경로 반환
                if (current == goalPos)
                {
                    path.Add(current);
                    while (cameFrom.ContainsKey(current))
                    {
                        current = cameFrom[current];
                        path.Insert(0, current);
                    }
                    return path.ToArray();
                }

                openSet.Remove(current);
                closedSet.Add(current);

                // 이웃 노드 탐색
                foreach (Vector2 neighbor in GetNeighbors(current, walls))
                {
                    if (closedSet.Contains(neighbor))
                        continue; // 이미 처리된 노드

                    float tentativeGScore = gScore[current] + 1; // 모든 엣지의 가중치가 1로 가정

                    if (!openSet.Contains(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        // 새로운 경로를 발견하거나 더 짧은 경로인 경우 업데이트
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, goalPos);

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }

            // 목표에 도달할 수 없는 경우 빈 배열 반환
            return new Vector2[0];
        }

        // 휴리스틱 예상 비용 계산 (예시: 맨해튼 거리)
        private static float HeuristicCostEstimate(Vector2 start, Vector2 goal)
        {
            return Math.Abs(start.x - goal.x) + Math.Abs(start.y - goal.y);
        }

        // 이웃 노드 찾기
        private static IEnumerable<Vector2> GetNeighbors(Vector2 node, Vector2[] walls)
        {
            // 여기에서 이웃 노드를 적절히 계산하십시오.
            // 예를 들어, 상하좌우 방향의 이웃 노드를 찾는 방법을 구현할 수 있습니다.

            // 이 예제에서는 간단히 상하좌우 방향으로 이동할 수 있는 경우에만 이웃으로 간주합니다.
            List<Vector2> neighbors = new List<Vector2>();
            if (!walls.Contains(new Vector2(node.x, node.y + 1)))
                neighbors.Add(new Vector2(node.x, node.y + 1));
            if (!walls.Contains(new Vector2(node.x, node.y - 1)))
                neighbors.Add(new Vector2(node.x, node.y - 1));
            if (!walls.Contains(new Vector2(node.x + 1, node.y)))
                neighbors.Add(new Vector2(node.x + 1, node.y));
            if (!walls.Contains(new Vector2(node.x - 1, node.y)))
                neighbors.Add(new Vector2(node.x - 1, node.y));

            return neighbors;
        }
    }
}