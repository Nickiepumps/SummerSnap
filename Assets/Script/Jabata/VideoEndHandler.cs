using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class VideoEndHandler : MonoBehaviour
{
    public string nextSceneName = "MainMenu";
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(1f); // รอ 1 วินาที ก่อนเปลี่ยน
        SceneManager.LoadScene(nextSceneName);
    }
}
