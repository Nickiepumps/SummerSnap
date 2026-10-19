using UnityEngine;
using UnityEngine.UI;
public class PhotoData : MonoBehaviour
{
    private SOQuestPhotographDetail photoSO;
    [SerializeField] AlbumViewerController albumViewerController;
    
    [SerializeField] private Button buttonViewPhoto;
    [SerializeField] private Image photoImage;
    [SerializeField] private Image photoImageUnnoticedNotif;

    private PhotoType photoType;
    public bool isNoticed = false;
    public void Initialize(Sprite photo, AlbumViewerController controller, SOQuestPhotographDetail photoSO, PhotoType photoType)
    {
        buttonViewPhoto.onClick.AddListener(ViewPhoto);
        if(photoSO.photoSprite != null)
        {
            photoImage.sprite = photoSO.photoSprite;
        }
        else
        {
            photoImage.sprite = photo;
        }
        albumViewerController = controller;
        this.photoSO = photoSO;
        this.photoType = photoType;
    }
    private void ViewPhoto()
    {
        albumViewerController.PhotoDetailInitialize(photoSO.photoName, photoSO.description, photoImage.sprite, photoType);
        isNoticed = true;
        photoImageUnnoticedNotif.enabled = false;
    }
}
