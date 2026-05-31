using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Loading : MonoBehaviour
{
    public string sceneToLoad = "Stage1";   // ฉากถัดไป
    public float loadingDuration = 3f;      // ระยะเวลาโหลดปลอม (ให้เท่ากับความยาว video)

    void Start()
    {
        StartCoroutine(FakeLoading());
    }

    IEnumerator FakeLoading()
    {
        // รอเวลาเท่ากับ loadingDuration (ให้ video เล่นจบ)
        yield return new WaitForSeconds(loadingDuration);

        // เมื่อครบเวลา เปลี่ยนฉากทันที
        SceneManager.LoadScene(sceneToLoad);
    }
}
