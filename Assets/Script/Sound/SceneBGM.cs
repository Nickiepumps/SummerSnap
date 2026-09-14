using UnityEngine;

public class SceneBGM : MonoBehaviour
{
    [Header("Scene BGM")]
    public AudioClip bgm;
    public bool playOnStart = true;

    [Header("Quest Mode")]
    public bool isQuestBGM = false;

    void Start()
    {
        if (AudioManager.Instance == null) return;
        if (!playOnStart) return;

        if (isQuestBGM)
        {
            AudioManager.Instance.PlayTemporaryBGMImmediate(bgm);
            return;
        }

        if (AudioManager.Instance.IsUserBGMActive())
            return;

        if (bgm != null)
        {
            AudioManager.Instance.PlaySceneBGM(bgm);
        }
        else if (AudioManager.Instance.defaultBGM != null)
        {
            AudioManager.Instance.PlaySceneBGM(AudioManager.Instance.defaultBGM);
        }
    }
}
