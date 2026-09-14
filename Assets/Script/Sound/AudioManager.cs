using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Default BGM")]
    public AudioClip defaultBGM;

    private AudioClip sceneClip;
    private AudioClip userClip;
    private AudioClip lastQuestClip;

    private Coroutine fadeCoroutine;

    private enum BGMMode { Scene, User, Quest }
    private BGMMode currentMode = BGMMode.Scene;
    private string loadingToGameplaySceneName = "LoadingToGameplay";
    private string introSceneName = "IntroStartGame";
    private string gameplayToLoadingSceneName = "LoadingToMainmenu";

    public float CurrentBGMVolume { get; private set; } = 0.5f;
    public float CurrentSFXVolume { get; private set; } = 0.5f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadVolume();
            bgmSource.volume = CurrentBGMVolume;
            sfxSource.volume = CurrentSFXVolume;

            SceneManager.sceneLoaded += OnSceneLoaded;

            if (defaultBGM != null)
                PlaySceneBGM(defaultBGM);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == introSceneName || scene.name == loadingToGameplaySceneName || scene.name == gameplayToLoadingSceneName)
        {
            StartFadeBGM(null, 0.5f);
            return;
        }

        if (IsUserBGMActive()) return;

        if (currentMode == BGMMode.Quest) return;

        if (FindObjectOfType<SceneBGM>() == null && defaultBGM != null)
        {
            PlaySceneBGM(defaultBGM);
        }
    }
    public void PlaySceneBGM(AudioClip clip)
    {
        if (clip == null) return;
        sceneClip = clip;
        currentMode = BGMMode.Scene;
        StartFadeBGM(clip, 0.5f);
    }
    public void PlayUserBGM(AudioClip clip)
    {
        if (clip == null) return;
        userClip = clip;
        currentMode = BGMMode.User;
        StartFadeBGM(clip, 0.5f);
    }
    public void PlayTemporaryBGMImmediate(AudioClip clip)
    {
        if (clip == null) return;
        lastQuestClip = bgmSource.clip;
        currentMode = BGMMode.Quest;

        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.volume = CurrentBGMVolume;
        bgmSource.Play();
    }
    public void RestoreLastBGM()
    {
        AudioClip restoreClip = null;

        if (userClip != null)
        {
            restoreClip = userClip;
            currentMode = BGMMode.User;
        }
        else if (sceneClip != null)
        {
            restoreClip = sceneClip;
            currentMode = BGMMode.Scene;
        }
        else
        {
            restoreClip = defaultBGM;
            currentMode = BGMMode.Scene;
        }

        if (restoreClip != null)
            StartFadeBGM(restoreClip, 0.5f);
    }
    public void PlaySFX(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip, CurrentSFXVolume);
    }

    public bool IsUserBGMActive()
    {
        return currentMode == BGMMode.User && userClip != null;
    }
    public void SetBGMVolume(float volume)
    {
        CurrentBGMVolume = volume;
        if (bgmSource != null)
            bgmSource.volume = volume;
        PlayerPrefs.SetFloat("BGMVolume", volume);
    }
    public void SetSFXVolume(float volume)
    {
        CurrentSFXVolume = volume;
        if (sfxSource != null)
            sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
    public void LoadVolume()
    {
        float bgmVol = PlayerPrefs.GetFloat("BGMVolume", 0.5f);
        float sfxVol = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        SetBGMVolume(bgmVol);
        SetSFXVolume(sfxVol);
    }
    private void StartFadeBGM(AudioClip newClip, float fadeTime)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeBGM(newClip, fadeTime));
    }
    private IEnumerator FadeBGM(AudioClip newClip, float fadeTime)
    {
        if (bgmSource.isPlaying)
        {
            float startVolume = bgmSource.volume;
            float t = 0;
            while (t < fadeTime)
            {
                t += Time.deltaTime;
                bgmSource.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
                yield return null;
            }
        }

        bgmSource.clip = newClip;
        bgmSource.loop = true;
        bgmSource.Play();

        float targetVolume = CurrentBGMVolume;
        float t2 = 0;
        while (t2 < fadeTime)
        {
            t2 += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0, targetVolume, t2 / fadeTime);
            yield return null;
        }
        bgmSource.volume = targetVolume;
    }
}
