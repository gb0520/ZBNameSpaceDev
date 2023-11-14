using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace ZB
{
    public class TimeScaleBrain : MonoBehaviour
    {
        static public TimeScaleBrain Instance = null;

        public bool CanEnterMenu { get => canEnterMenu; }
        public bool InMenu { get => inMenu; }

        bool canEnterMenu;
        bool inMenu;

        [Header("������ ��� ����")]
        [SerializeField] bool infinite_Scaling;
        [Header("�����ϸ� ���")]
        [SerializeField] float scaled_multiple;          //���� �ð����� ���
        [Header("�����ϸ� ������")]
        [SerializeField] bool timescaling;               //�ð�������
        [Header("�����ϸ� ������ ���ӽð�"), Space(30)]
        [SerializeField] float scaled_runtime;           //���� �ð����� ����ð�
        [Header("�����ϸ� ������ ���ӽð�")]
        [SerializeField] float scaling_PassedTime;       //�ð������� : �������ð�

        float Original_FixedDeltaTime;

        WaitForSeconds scalingEndTiming_WFS = new WaitForSeconds(0.5f);


        //�ð����� ���� �� Ÿ�ֿ̹� �ݹ�
        public UnityEvent ScaleEndCallBack;

        /// <summary>
        /// �ð�����
        /// </summary>
        /// <param name="�ð����� ���"></param>
        /// <param name="���� ���ӽð�"></param>
        /// <param name="�������� �޴� ����Ұ� ����"></param>
        public void ChangeTimeScale(float scaled_multiple, float scaled_runtime, bool CanEnterMenu_InScaling = true)
        {
            infinite_Scaling = false;

            this.scaled_multiple = scaled_multiple;
            this.scaled_runtime = scaled_runtime;
            canEnterMenu = CanEnterMenu_InScaling;

            StartCoroutine(PassingTimeCheck());
        }
        public void ChangeTimeScale(float scaled_multiple, bool CanEnterMenu_InScaling = true)
        {
            infinite_Scaling = true;

            this.scaled_multiple = scaled_multiple;
            canEnterMenu = CanEnterMenu_InScaling;

            StartCoroutine(PassingTimeCheck());
        }

        /// <summary>
        /// �ð����� ��������
        /// </summary>
        public void ChangeTimeScale_runtimeSkip()
        {
            timescaling = false;
            scaling_PassedTime = 0;
            Change_OriginalTimeScale();
        }

        /// <summary>
        /// �޴����� �ð�����
        /// </summary>
        public void EnterMenu_ChangeTimeScale()
        {
            if (canEnterMenu)
            {
                inMenu = true;
                Time.timeScale = 0;
            }
        }

        /// <summary>
        /// �޴����� �ð�����
        /// </summary>
        public void ExitMenu_ChangeTimeScale()
        {
            inMenu = false;
            if (!timescaling)
                Time.timeScale = 1;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }

            Original_FixedDeltaTime = Time.fixedDeltaTime;
            scaled_multiple = 1;
            scaled_runtime = 0;
        }
        private void Change_OriginalTimeScale()
        {
            scaled_multiple = 1;
            scaled_runtime = 0;
            canEnterMenu = true;

            if (!inMenu)
                Time.timeScale = 1;
            Time.fixedDeltaTime = Original_FixedDeltaTime;
        }
        IEnumerator PassingTimeCheck(bool infiniteScaling = false)
        {
            timescaling = true;
            while (timescaling)
            {
                if (!inMenu)
                {
                    Time.timeScale = scaled_multiple;
                    Time.fixedDeltaTime = Original_FixedDeltaTime * scaled_multiple;

                    if (!infiniteScaling)
                    {
                        scaling_PassedTime += Time.unscaledDeltaTime;

                        if (scaling_PassedTime > scaled_runtime)
                        {
                            timescaling = false;
                            scaling_PassedTime = 0;
                        }
                    }
                }
                else if (canEnterMenu)
                {
                    Time.timeScale = 0;
                }

                yield return null;
            }

            Change_OriginalTimeScale();
            ScaleEndCallBack.Invoke();
        }
    }
}