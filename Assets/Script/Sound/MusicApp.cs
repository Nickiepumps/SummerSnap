using UnityEngine;
using UnityEngine.UI;

public class MusicAppUI : MonoBehaviour
{
    [Header("Songs")]
    public AudioClip[] songs;

    [Header("UI")]
    public Button[] songButtons;

    void Start()
    {
        for (int i = 0; i < songButtons.Length; i++)
        {
            int index = i; // เก็บค่าไว้ใน local variable
            songButtons[i].onClick.AddListener(() => PlaySong(index));
        }
    }

    void PlaySong(int index)
    {
        if (index < 0 || index >= songs.Length) return;

        AudioManager.Instance.PlayUserBGM(songs[index]);

        Debug.Log("Play song: " + songs[index].name);
    }
}
