using UnityEngine;

public class PlayBGM : MonoBehaviour
{
    public AudioClip bgmClip;
    [Range(0f, 1f)] public float volume = 0.3f; // ปรับระดับเสียงเพลงจาก Inspector

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.clip = bgmClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = volume; // 🔉 ตั้งค่าความดัง
        audioSource.Play();
    }

    // Optional: เปลี่ยนระดับเสียงจากสคริปต์อื่น
    public void SetVolume(float newVolume)
    {
        audioSource.volume = Mathf.Clamp01(newVolume);
    }
}
