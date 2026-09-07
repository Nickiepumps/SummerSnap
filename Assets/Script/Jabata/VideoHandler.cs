using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.UI;

public class VideoHandler : MonoBehaviour
{
    [SerializeField] private Button buttonSkip;
    public string nextSceneName = "MainMenu";
    private VideoPlayer videoPlayer;
    
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }
        buttonSkip.onClick.AddListener(SkipCutscene);
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(LoadNextScene());
    }
    private void SkipCutscene()
    {
        videoPlayer.Stop();
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(1f); // รอ 1 วินาที ก่อนเปลี่ยน
        SceneManager.LoadScene(nextSceneName);
    }
}
