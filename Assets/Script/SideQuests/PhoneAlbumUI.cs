using System.Collections.Generic;
using UnityEngine;
public class PhoneAlbumUI : MonoBehaviour
{
    public static PhoneAlbumUI Instance { get; private set; }
    [SerializeField] private AlbumViewerController albumViewerController;
    public Transform contentParent;
    public GameObject photoPrefab;
    [SerializeField] private GameObject photoSideQuestNotif;
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
        }

        if (SideQuestPhotoStorage.Instance != null)
        {
            foreach (var sprite in SideQuestPhotoStorage.Instance.photos)
            {
                AddPhoto(sprite);
            }
        }*/
    }
    public void CheckNoticedPhoto()
    {
        if (photoDataList.Count == 0) return;
        foreach (PhotoData photoData in photoDataList)
        {
            if (photoData.isNoticed == false)
            {
                photoSideQuestNotif.SetActive(true);
                return;
            }
        }
        photoSideQuestNotif.SetActive(false);
    }
    public void AddPhoto(Sprite sprite, SOQuestPhotographDetail photoSO)
    {
        if (sprite == null) return;
        GameObject newPhoto = Instantiate(photoPrefab, contentParent);
        PhotoData photoData = newPhoto.GetComponent<PhotoData>();
        photoData.Initialize(sprite, albumViewerController, photoSO, PhotoType.AchivementPhoto);
        photoDataList.Add(photoData);
        photoSideQuestNotif.SetActive(true);
        GameManager.Instance.GetPlayerController().TogglePhoneNotif(true);
    }
}
