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
            // A* �˰��� ����
            List<Vector2> path = new List<Vector2>();
            HashSet<Vector2> closedSet = new HashSet<Vector2>();
            HashSet<Vector2> openSet = new HashSet<Vector2> { nowPos };
            Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();

            // ���� ����� �ʱ� ���� ���
            Dictionary<Vector2, float> gScore = new Dictionary<Vector2, float>
        {
            { nowPos, 0 }
        };

            // ���� ����� ��ü ���� ���
            Dictionary<Vector2, float> fScore = new Dictionary<Vector2, float>
        {
            { nowPos, HeuristicCostEstimate(nowPos, goalPos) }
        };

            while (openSet.Count > 0)
            {
                // ���� ��� �߿��� ���� ����� ���� ���� ��� ����
                Vector2 current = openSet.OrderBy(node => fScore[node]).First();

                // ��ǥ�� ������ ��� ��� ��ȯ
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

                // �̿� ��� Ž��
                foreach (Vector2 neighbor in GetNeighbors(current, walls))
                {
                    if (closedSet.Contains(neighbor))
                        continue; // �̹� ó���� ���

                    float tentativeGScore = gScore[current] + 1; // ��� ������ ����ġ�� 1�� ����

                    if (!openSet.Contains(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        // ���ο� ��θ� �߰��ϰų� �� ª�� ����� ��� ������Ʈ
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, goalPos);

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }

            // ��ǥ�� ������ �� ���� ��� �� �迭 ��ȯ
            return new Vector2[0];
        }

        // �޸���ƽ ���� ��� ��� (����: ����ư �Ÿ�)
        private static float HeuristicCostEstimate(Vector2 start, Vector2 goal)
        {
            return Math.Abs(start.x - goal.x) + Math.Abs(start.y - goal.y);
        }

        // �̿� ��� ã��
        private static IEnumerable<Vector2> GetNeighbors(Vector2 node, Vector2[] walls)
        {
            // ���⿡�� �̿� ��带 ������ ����Ͻʽÿ�.
            // ���� ���, �����¿� ������ �̿� ��带 ã�� ����� ������ �� �ֽ��ϴ�.

            // �� ���������� ������ �����¿� �������� �̵��� �� �ִ� ��쿡�� �̿����� �����մϴ�.
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