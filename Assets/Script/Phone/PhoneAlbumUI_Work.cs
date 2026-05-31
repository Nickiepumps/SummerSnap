using UnityEngine;
using UnityEngine.UI;

public class PhoneAlbumUI_Work : MonoBehaviour
{
    public static PhoneAlbumUI_Work Instance { get; private set; }
    public Transform contentParent;
    public GameObject photoPrefab;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    void OnEnable()
    {
        // ล้างเก่า กันรูปซ้ำ
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        // โหลดรูปจาก MainQuest เท่านั้น
        if (MainQuestPhotoStorage.Instance != null)
        {
            foreach (var sprite in MainQuestPhotoStorage.Instance.photos)
                AddPhoto(sprite);
        }
    }

    public void AddPhoto(Sprite sprite)
    {
        if (sprite == null) return;
        GameObject newPhoto = Instantiate(photoPrefab, contentParent);
        var img = newPhoto.GetComponent<Image>();
        if (img != null) img.sprite = sprite;
    }
}
