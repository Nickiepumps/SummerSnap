using UnityEngine;
using UnityEngine.UI;
public class PhoneAlbumUI_Work : MonoBehaviour
{
    public static PhoneAlbumUI_Work Instance { get; private set; }
    public Transform contentParent;
    public GameObject photoPrefab;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }
    private void OnEnable()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        if (MainQuestPhotoStorage.Instance != null)
        {
            foreach (var sprite in MainQuestPhotoStorage.Instance.photos)
            {
                AddPhoto(sprite);
            }
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
