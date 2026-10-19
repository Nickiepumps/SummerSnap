using System.Collections.Generic;
using UnityEngine;
public class PhoneAlbumUI_Work : MonoBehaviour
{
    public static PhoneAlbumUI_Work Instance { get; private set; }
    [SerializeField] private AlbumViewerController albumViewerController;
    public Transform contentParent;
    public GameObject photoPrefab;
    [SerializeField] private GameObject photoMainQuestNotif;
    private List<PhotoData> photoDataList = new List<PhotoData>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        /*foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }*/

        /*if (MainQuestPhotoStorage.Instance != null)
        {
            foreach (var sprite in MainQuestPhotoStorage.Instance.photos)
            {
                AddPhoto(sprite);
            }
        }*/
    }
    private void OnDisable()
    {
        
    }
    public void CheckNoticedPhoto()
    {
        if (photoDataList.Count == 0) return;
        foreach (PhotoData photoData in photoDataList)
        {
            if(photoData.isNoticed == false)
            {
                photoMainQuestNotif.SetActive(true);
                return;
            }
        }
        photoMainQuestNotif.SetActive(false);
    }
    public void AddPhoto(Sprite sprite, SOQuestPhotographDetail photoSO)
    {
        if (sprite == null) return;
        GameObject newPhoto = Instantiate(photoPrefab, contentParent);
        PhotoData photoData = newPhoto.GetComponent<PhotoData>();
        photoData.Initialize(sprite, albumViewerController, photoSO, PhotoType.MainQuestPhoto);
        photoDataList.Add(photoData);
        photoMainQuestNotif.SetActive(true);
        GameManager.Instance.GetPlayerController().TogglePhoneNotif(true);
        /*var img = newPhoto.GetComponent<Image>();
        if (img != null)
        {
            img.sprite = sprite;
        }*/
    }
}
