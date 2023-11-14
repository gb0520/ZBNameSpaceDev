using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace ZB.Sound
{
    public class Player : MonoBehaviour
    {
        public string PlayerName { get => playerName; }

        [SerializeField] private AudioSource[] audioSources;
        [SerializeField] private AudioClip[] audioClips;
        [SerializeField] private AudioMixerGroup outputAudioMixerGroup;
        [Space]
        [SerializeField] private float volume;

        [Space]
        [SerializeField] private string playerName;
        [SerializeField] private float smoothDuration;
        [SerializeField] private bool loop;
        [SerializeField] private bool onLoop_replayOnSame;

        private int size { get => audioSources.Length; }
        private int minSize = 2;
        private int currentIndex;

        public void Init(PlayerData playerData)
        {
            this.playerName = playerData.playerName;

            this.outputAudioMixerGroup = playerData.outputAudioMixerGroup;

            this.audioClips = Resources.LoadAll<AudioClip>(playerData.path);

            this.smoothDuration = playerData.smoothDuration;

            this.loop = playerData.loop;

            this.onLoop_replayOnSame = playerData.loop;

            //audioSources, SmoothStopCycles ����
            audioSources = new AudioSource[playerData.size];
            SmoothVolumeZeroCycles = new IEnumerator[size];
            currentIndex = 0;

            GameObject audioObj = new GameObject();
            AudioSource audioSource = audioObj.AddComponent<AudioSource>();
            audioSource.name = "AudioSource (0)";
            audioSource.transform.SetParent(this.transform);
            audioSource.loop = loop;
            audioSource.outputAudioMixerGroup = outputAudioMixerGroup;

            audioSources[0] = audioSource;

            for (int i = 1; i < audioSources.Length; i++)
            {
                AudioSource temp = Instantiate(audioSource);
                temp.gameObject.name = $"AudioSource ({i})";
                temp.transform.SetParent(this.transform);

                temp.loop = loop;
                audioSources[i] = temp;
            }
        }
        public void Play(string name)
        {
            //�Ű������� �´� audioClip ��������
            AudioClip audioClip = GetAudioClip(name);
            if (audioClip == null) return;

            //loop �ƴ� ��� �ֱ� �ε��� ���� ��ҿ� ����, ���
            if (!loop)
            {
                currentIndex = (currentIndex + 1) % size;
                SmoothVolumeZeroActive(currentIndex, false);
                audioSources[currentIndex].clip = audioClip;
                audioSources[currentIndex].Play();
                return;
            }

            //loop �� ��� 
            //�̹� ���� ���� ��� ���� ���
            for (int i = 0; i < audioSources.Length; i++)
            {
                if (audioSources[i].clip != null &&
                    audioSources[i].clip.name == name)
                {
                    //�ٽ����
                    if (onLoop_replayOnSame)
                    {
                        audioSources[i].Play();
                        return;
                    }
                    //����
                    return;
                }
            }

            //�� ��ҿ� ����, ���
            for (int i = 0; i < audioSources.Length; i++)
            {
                if (!audioSources[i].isPlaying)
                {
                    currentIndex = i;
                    SmoothVolumeZeroActive(i, false);
                    audioSources[i].clip = audioClip;
                    audioSources[i].Play();
                    return;
                }
            }
            //�� ��� ���� ��� �ֱ� �ε����� �����ε����� ��� ���
            currentIndex = (currentIndex + 1) % size;
            SmoothVolumeZeroActive(currentIndex, false);
            audioSources[currentIndex].clip = audioClip;
            audioSources[currentIndex].Play();
            return;
        }
        public void Stop(string name)
        {
            //loop�ϰ�쿡�� Stop
            if (loop)
            {
                //�ҽ� ��ȸ, �´� �̸� ã�Ƽ� ����
                for (int i = 0; i < audioSources.Length; i++)
                {
                    if (audioSources[i].clip.name == name)
                    {
                        SmoothVolumeZeroActive(i, true);
                        return;
                    }
                }

                Debug.LogWarning($"ZB.Sound.Player -> Stop() / �Ű����� ������");
                return;
            }
            Debug.LogWarning($"ZB.Sound.Player -> Stop() / ���� �÷��̾� �ƴ�");
            return;
        }
        public void StopAll()
        {
            for (int i = 0; i < audioSources.Length; i++)
            {
                SmoothVolumeZeroActive(i, true);
            }
        }
        public void StopCurrent()
        {
            SmoothVolumeZeroActive(currentIndex, true);
        }

        public string GetLastPlayedClipName()
        {
            return audioClips[currentIndex].name;
        }

        private AudioClip GetAudioClip(string name)
        {
            for (int i = 0; i < audioClips.Length; i++)
            {
                if (audioClips[i].name == name)
                    return audioClips[i];
            }
            Debug.LogWarning($"ZB.Sound.Player -> GetAudioClip() / �Ű����� ������ / {name}");
            return null;
        }
        private void SmoothVolumeZeroActive(int index, bool active)
        {
            if (active)
            {
                if (audioSources[index].clip != null && audioSources[index].isPlaying)
                {
                    SmoothVolumeZeroCycles[index] = SmoothVolumeZeroCycle(audioSources[index]);
                    StartCoroutine(SmoothVolumeZeroCycles[index]);
                }
            }
            else
            {
                if (SmoothVolumeZeroCycles[index] != null)
                    StopCoroutine(SmoothVolumeZeroCycles[index]);
                audioSources[index].volume = 1;
            }
        }
        private float AdjustVolumeValue(float value)
        {
            value = Mathf.Clamp(value, 0, 1);
            float temp = Mathf.Clamp(value, 0.0001f, 1) * 100;
            return ((Mathf.Log10(temp) + 2) / 4) * 100 - 80;
        }

        IEnumerator[] SmoothVolumeZeroCycles;
        IEnumerator SmoothVolumeZeroCycle(AudioSource audioSource)
        {
            float waitT = smoothDuration;
            float volume = audioSource.volume;

            while (waitT > 0 && volume >= 0)
            {
                waitT -= Time.deltaTime;
                volume -= 1 / smoothDuration * Time.deltaTime;
                audioSource.volume = volume;
                yield return null;
            }
            audioSource.Stop();
            audioSource.volume = 1;
            audioSource.clip = null;
        }
    }
}