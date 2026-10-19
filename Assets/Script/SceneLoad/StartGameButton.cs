using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGameButton : MonoBehaviour
{
    public string loadingSceneName = "LoadingToGameplay"; // ไปหน้า Loading ก่อน
    public AudioSource clickSound;

    public void StartGame()
    {
        if (clickSound != null) clickSound.Play();

        // 🔹 ทำลาย Player/Ryu เดิมจากรอบก่อน
        if (GameManager.Instance != null)
        {
            GameObject oldPlayer = GameManager.Instance.GetPlayerGameObject();
            if (oldPlayer != null) Destroy(oldPlayer);

            GameObject oldRyu = GameManager.Instance.GetRyu();
            if (oldRyu != null) Destroy(oldRyu);

            // 🔹 รีเซ็ต GameManager → สร้าง Player/Ryu ใหม่
            GameManager.Instance.ResetGame();
        }

        StartCoroutine(LoadSceneAfterSound());
    }


    IEnumerator LoadSceneAfterSound()
    {
        yield return new WaitForSeconds(0.6f);
        SceneManager.LoadScene(loadingSceneName);
    }
}
