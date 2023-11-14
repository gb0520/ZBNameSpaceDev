using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZB.Sort
{
    public class SortUtility
    {
        public static Vector2[] SortByIntervals(Vector2[] positions, Vector2 center, Vector2 dir, float interval)
        {
            List<int> priority = new List<int>();
            List<int> checkList = new List<int>();
            for (int i = 0; i < positions.Length; i++)
                checkList.Add(i);

            int loopCount = checkList.Count;
            for (int i = 0; i < loopCount; i++)
            {
                int minIndex = 0;
                for (int j = 0; j < checkList.Count; j++)
                {
                    if (positions[j].x <= positions[minIndex].x)
                    {
                        minIndex = j;
                    }
                }
                priority.Add(checkList[minIndex]);
                checkList.RemoveAt(minIndex);
            }

            float[] normalizedInterval = new float[positions.Length];

            for (int i = 0; i < normalizedInterval.Length; i++)
            {
                //원소개수 짝수일 때
                if (positions.Length % 2 == 0)
                {
                    normalizedInterval[i] = i - (positions.Length - 1) / (float)2;
                }

                //원소개수 홀수일 때
                else
                {
                    normalizedInterval[i] = i - (positions.Length / (float)2) + 0.5f;
                }
            }

            for (int i = 0; i < positions.Length; i++)
            {
                positions[i] = dir.normalized * normalizedInterval[priority[i]] * interval + center;
            }

            return positions;
        }
    }
}