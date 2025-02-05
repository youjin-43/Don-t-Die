using System.Collections.Generic;
using UnityEngine;
using VInspector;

public enum AudioClipName
{

}

public enum AudioType
{
    BGM,
    Effect,

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

    public void Play(AudioType type, AudioClipName clipName)
    {
        AudioSource audioSource = audioSources[(int)type];
        AudioClip clip = audioClips[clipName];
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
        }
    }

    public void Stop(AudioType type)
    {
        audioSources[(int)type].Stop();
    }
}
