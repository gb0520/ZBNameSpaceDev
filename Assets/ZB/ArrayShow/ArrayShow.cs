using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZB.Sort;

namespace ZB
{
    public class ArrayShow : MonoBehaviour
    {
        [SerializeField] private Transform fold_arrayShow;
        [SerializeField] private RectTransform arrayShow_Original;
        [SerializeField] private List<RectTransform> arrayShows;
        [SerializeField] private List<RectTransform> arrayShows_using;
        [SerializeField] private RectTransform rtf_where;

        private int index;
        private int length;
        private bool isLeftEnd { get => index <= 0; }
        private bool isRightEnd { get => index >= length - 1; }

        [Space]
        [SerializeField] private float interval;

        public void Active(int length, int startPos)
        {
            if (length > 0)
            {
                this.length = length;

                //초기화
                for (int i = 0; i < arrayShows.Count; i++)
                    arrayShows[i].gameObject.SetActive(false);
                arrayShows_using.Clear();

                //원소를 필요한만큼 킨다.
                int index = 0;
                for (int i = 0; i < length && i < arrayShows.Count; i++)
                {
                    arrayShows[i].gameObject.SetActive(true);
                    arrayShows_using.Add(arrayShows[i]);
                    index++;
                }
                for (int i = index; i < length; i++)
                {
                    RectTransform newRtf = Instantiate(arrayShow_Original);
                    newRtf.transform.SetParent(fold_arrayShow);
                    newRtf.transform.localScale = Vector3.one;
                    newRtf.gameObject.SetActive(true);
                    arrayShows_using.Add(newRtf);
                    arrayShows.Add(newRtf);
                }

                //킨 원소들 위치 정렬
                Vector2[] arrayShowPositions = new Vector2[arrayShows_using.Count];
                for (int i = 0; i < arrayShows_using.Count; i++)
                    arrayShowPositions[i] = arrayShows_using[i].anchoredPosition;

                Vector2[] sortedVector = SortUtility.SortByIntervals(arrayShowPositions, Vector2.zero, Vector2.right, interval);
                for (int i = 0; i < arrayShows_using.Count; i++)
                {
                    arrayShows_using[i].anchoredPosition = sortedVector[i];
                }

                //킨 원소들 인덱스 정렬
                Comparison<RectTransform> rtfComparision = (x, y) => x.anchoredPosition.x.CompareTo(y.anchoredPosition.x);
                arrayShows_using.Sort(rtfComparision);

                //where 위치설정
                startPos = startPos >= length ? length - 1 : startPos;
                rtf_where.anchoredPosition = arrayShows_using[startPos].anchoredPosition;
            }
        }

        public void MoveLeft()
        {
            if (!isLeftEnd)
            {
                //원래 있던 이미지 FadeOut
                Fade(arrayShows[index], false);

                //등장할 이미지 FadeIn
                Fade(arrayShows[--index], true);
            }
        }
        public void MoveRight()
        {
            if (!isRightEnd)
            {
                //원래 있던 이미지 FadeOut
                Fade(arrayShows[index], false);

                //등장할 이미지 FadeIn
                Fade(arrayShows[++index], true);
            }
        }

        private void Fade(RectTransform rtf, bool active)
        {
            Image img;
            //채워야 할부분
            if (active)
            {
                //FadeIn

            }
            else
            {
                //FadeOut

            }
        }

        public void Awake()
        {
            fold_arrayShow.gameObject.SetActive(true);
            for (int i = 0; i < fold_arrayShow.childCount; i++)
            {
                Destroy(fold_arrayShow.GetChild(i).gameObject);
            }
            arrayShows = new List<RectTransform>();
            arrayShows_using = new List<RectTransform>();
        }
    }
}