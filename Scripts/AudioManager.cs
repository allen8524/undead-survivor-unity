using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM")]
    public AudioClip bgmClip;
    public float bgmVolume = 0.5f;
    AudioSource bgmPlayer;
    AudioHighPassFilter bgmEffect;

    [Header("#SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume = 0.5f;
    public int channels = 16;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx { Dead, Hit, LevelUp = 3, Lose, Melee, Range = 7, Select, Win }

    void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;
        bgmEffect = Camera.main != null ? Camera.main.GetComponent<AudioHighPassFilter>() : null;

        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].bypassListenerEffects = true;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlayBgm(bool isPlay)
    {
        if (bgmPlayer == null)
            return;

        if (isPlay)
            bgmPlayer.Play();
        else
            bgmPlayer.Stop();
    }

    public void EffectBgm(bool isPlay)
    {
        if (bgmEffect != null)
            bgmEffect.enabled = isPlay;
    }

    public void PlaySfx(Sfx sfx)
    {
        if (sfxPlayers == null || sfxClips == null || sfxClips.Length == 0)
            return;

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            int clipIndex = (int)sfx;

            if (sfx == Sfx.Hit || sfx == Sfx.Melee)
                clipIndex += Random.Range(0, 2);

            if (clipIndex < 0 || clipIndex >= sfxClips.Length)
                return;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[clipIndex];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }
}
