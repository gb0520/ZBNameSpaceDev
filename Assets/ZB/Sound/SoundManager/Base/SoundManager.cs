using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace ZB.Sound
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private bool INITONAWAKE;
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private PlayerData[] playerDatas;
        [SerializeField] float minDecibel;
        [SerializeField] float maxDecibel;

        private Player[] players;

        public void Init()
        {
            if (playerDatas != null && playerDatas.Length > 0)
            {
                players = new Player[playerDatas.Length];

                GameObject playerObj = new GameObject();
                Player player = playerObj.AddComponent<Player>();
                player.name = $"Player : {playerDatas[0].playerName}";
                player.transform.SetParent(this.transform);
                players[0] = player;

                for (int i = 1; i < playerDatas.Length; i++)
                {
                    Player temp = Instantiate(player);
                    temp.gameObject.name = $"Player : {playerDatas[i].playerName}";
                    temp.transform.SetParent(this.transform);
                    temp.Init(playerDatas[i]);
                    players[i] = temp;
                }

                player.Init(playerDatas[0]);
            }
        }

        public Player GetPlayer(string playerName)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].PlayerName == playerName)
                    return players[i];
            }
            Debug.LogWarning($"SoundManager -> GetPlayer / elementName 매개변수 오기입 / {playerName}");
            return null;
        }

        public void Play(string playerName, string name)
        {
            Player player = GetPlayer(playerName);
            if (player == null)
                return;

            player.Play(name);
        }
        public void Stop(string playerName, string name)
        {
            Player player = GetPlayer(playerName);
            if (player == null)
                return;

            player.Stop(name);
        }
        public void StopAll(string playerName)
        {
            Player player = GetPlayer(playerName);
            if (player == null)
                return;

            player.StopAll();
        }
        /// <summary>
        /// BGM과 같이 하나의 사운드만 들려주는 Player에서만 사용 (최근 재생하던 사운드를 멈추고, 하나를 재생한다)
        /// </summary>
        /// <param name="playerName"></param>
        /// <param name="name"></param>
        public void StopCurrentAndPlay(string playerName, string name)
        {
            StopCurrent(playerName);
            Play(playerName, name);
        }
        /// <summary>
        /// BGM과 같이 하나의 사운드만 들려주는 Player에서만 사용 (최근 재생하던 사운드를 멈춘다)
        /// </summary>
        /// <param name="playerName"></param>
        public void StopCurrent(string playerName)
        {
            Player player = GetPlayer(playerName);
            if (player == null)
                return;

            player.StopCurrent();
        }

        public string GetLastPlayedClipName(string playerName)
        {
            Player player = GetPlayer(playerName);
            if (player == null)
                return "";

            return player.GetLastPlayedClipName();
        }

        /// <summary>
        /// 볼륨세팅 Awake에서 잘 작동하지 않음. Start에서 호출 권장
        /// </summary>
        /// <param name="mixerGroupName"></param>
        /// <param name="value">0~1</param>
        public void SetVolume(string mixerGroupName, float value)
        {
            value = Mathf.Clamp(value, 0, 1);
            audioMixer.SetFloat(mixerGroupName, AdjustVolumeValue(value));
        }
        public bool TryGetVolume(string mixerGroupName, out float result)
        {
            float adjustedValue;
            result = 0;
            if (audioMixer.GetFloat(mixerGroupName, out adjustedValue))
            {
                result = InverseAdjustVolumeValue(adjustedValue);
                return true;
            }
            return false;
        }

        private float AdjustVolumeValue(float value)
        {
            float result;
            value = Mathf.Clamp(value, 0, 1);

            float temp = Mathf.Clamp(value, 0.0001f, 1) * 100;
            result = ((Mathf.Log10(temp) + 2) / 4) * (maxDecibel - minDecibel) + minDecibel;
            return result;
        }
        private float InverseAdjustVolumeValue(float adjustedValue)
        {
            adjustedValue = Mathf.Clamp(adjustedValue, minDecibel, maxDecibel);

            float temp = Mathf.Pow(10, ((adjustedValue - minDecibel) / (maxDecibel - minDecibel) * 4) - 2) / 100;
            float value = Mathf.Clamp(temp, 0, 1);
            return value;
        }

        [Space(30)]
        [SerializeField] float testValue;
        [ContextMenu("함수")]
        public void Func()
        {
            Debug.LogError(AdjustVolumeValue(testValue));
        }
        [ContextMenu("역함수")]
        public void InverseFunc()
        {
            Debug.LogError(InverseAdjustVolumeValue(testValue));
        }

        private void Awake()
        {
            if (INITONAWAKE)
                Init();
        }
    }

    [System.Serializable]
    public class PlayerData
    {
        public string playerName;
        public AudioMixerGroup outputAudioMixerGroup;

        [Space]
        [Tooltip("AudioClip을 가져올, Resources 하위폴더 경로")]
        public string path;
        [Tooltip("사운드 정지 명령 때, AudioSource Volume 1->0 까지 가는데 걸리는 시간")]
        public float smoothDuration;
        [Tooltip("생성할 AudioSource 수" + "\n" +
            "BGM 처럼, 한번에 하나의 사운드만 들려줘도 되는 경우 최소사이즈인 2를 입력하면 된다."),
            Range(2, 30)]
        public int size;
        [Tooltip("AudioSource loop check")]
        public bool loop;

        [Space]
        [Tooltip("루프 플레이어일 때, 이미 재생하던 사운드를 재생해라 또 명령받을 경우 정지 후 다시재생")]
        public bool onLoop_replayOnSame;
    }
}