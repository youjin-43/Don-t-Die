using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public enum AudioClipName
{
    None = 999,
    BGM = 0,
    FootStep,
    AttackTree,
    AttackMineral,
    Equip,
    GetItem,
    Hit,
    CraftStart,
    CraftEnd,
    PlayerHit,
    PlayerAttack,
    PlayerRevive,
    Mushroom_Flee,
    Mushroom_Die,
    Rat_Flee,
    Rat_Die
}

public enum AudioType
{
    BGM,
    Effect,
    FootStep
}

public class SoundManager : MonoBehaviour
{
    List<AudioSource> audioSources = new List<AudioSource>();
    public SerializedDictionary<AudioClipName, AudioClip> audioClips = new SerializedDictionary<AudioClipName, AudioClip>();

    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Init()
    {
        if (instance == null)
        {
            instance = this;

            // AudioType 종류만큼 AudioSource를 생성하고 SoundManager의 자식으로 만든다.
            string[] audioTypeNames = System.Enum.GetNames(typeof(AudioType));
            for (int i = 0; i < audioTypeNames.Length; i++)
            {
                GameObject go = new GameObject { name = audioTypeNames[i] };
                audioSources.Add(go.AddComponent<AudioSource>());
                go.transform.parent = transform;
            }

            audioSources[(int)AudioType.BGM].loop = true;

            DontDestroyOnLoad(this);
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    void Awake()
    {
        Init();
    }

    public void Play(AudioType type, AudioClipName clipName, float volume = 0.6f)
    {
        AudioSource audioSource = audioSources[(int)type];
        AudioClip clip = audioClips[clipName];
        audioSource.volume = volume;
        if (clip == null) return;

        switch (type)
        {
            case AudioType.BGM:
                {
                    audioSource.clip = clip;
                    audioSource.Play();
                    break;
                }
            case AudioType.Effect:
                {
                    audioSource.PlayOneShot(clip);
                    break;
                }
            case AudioType.FootStep:
                {
                    if (audioSource.isPlaying) { return; }
                    audioSource.PlayOneShot(clip);
                    break;
                }
        }
    }

    public void Stop(AudioType type)
    {
        audioSources[(int)type].Stop();
    }

    public void SetVolume(AudioType type, float volume)
    {
        audioSources[(int)type].volume = volume;
    }

    public float GetVolume()
    {
        return audioSources[0].volume;
    }

    public void FadeVolume(AudioType type, float volume)
    {
        StartCoroutine(FadeVolumeRoutine(type, volume));
    }

    IEnumerator FadeVolumeRoutine(AudioType type, float volume, float duration = 1f)
    {
        AudioSource audioSource = audioSources[(int)type];
        float startVolume = audioSource.volume;
        float timer = 0;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, volume, timer / duration);
            yield return null;
        }

        if (audioSource.volume < float.Epsilon)
        {
            audioSource.Stop();
        }
    }

    public float _currentVolume;

    public void SetAllVolume(float volume)
    {
        foreach (var audioSource in audioSources)
        {
            _currentVolume = audioSource.volume = volume;
        }
    }
}
