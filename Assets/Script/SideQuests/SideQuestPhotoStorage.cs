using System.Collections.Generic;
using UnityEngine;

public class SideQuestPhotoStorage : MonoBehaviour
{
    public static SideQuestPhotoStorage Instance;
    public List<Sprite> photos = new List<Sprite>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // อยู่ทุก scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPhoto(Sprite photo)
    {
        if (photo != null)
            photos.Add(photo);
    }
}
