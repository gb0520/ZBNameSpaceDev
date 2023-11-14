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

        [Header("무한정 대기 여부")]
        [SerializeField] bool infinite_Scaling;
        [Header("스케일링 배수")]
        [SerializeField] float scaled_multiple;          //받은 시간조정 배수
        [Header("스케일링 진행중")]
        [SerializeField] bool timescaling;               //시간조정중
        [Header("스케일링 지정된 지속시간"), Space(30)]
        [SerializeField] float scaled_runtime;           //받은 시간조정 진행시간
        [Header("스케일링 지나간 지속시간")]
        [SerializeField] float scaling_PassedTime;       //시간조정중 : 지나간시간

        float Original_FixedDeltaTime;

        WaitForSeconds scalingEndTiming_WFS = new WaitForSeconds(0.5f);


        //시간조정 끝날 때 타이밍에 콜백
        public UnityEvent ScaleEndCallBack;

        /// <summary>
        /// 시간조정
        /// </summary>
        /// <param name="시간조정 배수"></param>
        /// <param name="조정 지속시간"></param>
        /// <param name="조정동안 메뉴 입장불가 여부"></param>
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
        /// 시간조정 강제종료
        /// </summary>
        public void ChangeTimeScale_runtimeSkip()
        {
            timescaling = false;
            scaling_PassedTime = 0;
            Change_OriginalTimeScale();
        }

        /// <summary>
        /// 메뉴입장 시간조정
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
        /// 메뉴퇴장 시간조정
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