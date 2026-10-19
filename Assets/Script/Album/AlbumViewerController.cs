using Cysharp.Threading.Tasks;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AlbumViewerController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Button buttonViewDescription;
    [SerializeField] private Button buttonHideUI;
    [SerializeField] private Button buttonExitViewer;

    [SerializeField] private CanvasGroup canvasMainQuestDescriptionView;
    [SerializeField] private CanvasGroup canvasMainQuestPhotoDetail;
    [SerializeField] private CanvasGroup canvasAchivementDescriptionView;
    [SerializeField] private CanvasGroup canvasAchivementPhotoDetail;
    [SerializeField] private CanvasGroup canvasPhotoView;
    [SerializeField] private CanvasGroup canvasViewerUIDisplay;

    [SerializeField] private Image mainquestPhotoView;
    [SerializeField] private Image sidequestPhotoView;
    [SerializeField] private Image imageHideUIButton;

    [SerializeField] private TMP_Text textMainQuestPhotoName;
    [SerializeField] private TMP_Text textMainQuestDescription;
    [SerializeField] private TMP_Text textSideQuestPhotoName;
    [SerializeField] private TMP_Text textSideQuestDescription;

    [SerializeField] private Sprite spriteUnHideUI;
    [SerializeField] private Sprite spriteHideUI;

    private bool isUIHidden = false;
    private bool isDisplayDescription = false;
    private PhotoType photoType;
    public void Initialize()
    {
        buttonViewDescription.onClick.AddListener(() => DisplayPhotoDescriptionToggle().Forget());
        buttonHideUI.onClick.AddListener(() => HidePhotoViewerUIToggle(0).Forget());
        buttonExitViewer.onClick.AddListener(() => ExitPhotoViewer().Forget());
    }
    public void PhotoDetailInitialize(string photoName, string description, Sprite photoSprite, PhotoType type)
    {
        PhoneUIManager.Instance.enabled = false;
        photoType = type;
        switch (type)
        {
            case PhotoType.MainQuestPhoto:
                textMainQuestPhotoName.text = photoName;
                textMainQuestDescription.text = description;
                mainquestPhotoView.sprite = photoSprite;
                mainquestPhotoView.gameObject.SetActive(true);
                break;
            case PhotoType.SideQuestPhoto:
                break;
            case PhotoType.AchivementPhoto:
                textSideQuestPhotoName.text = photoName;
                textSideQuestDescription.text = description;
                sidequestPhotoView.sprite = photoSprite;
                sidequestPhotoView.gameObject.SetActive(true);
                break;
        }
        OpenPhotoViewer().Forget();
    }
    private async UniTaskVoid DisplayPhotoDescriptionToggle()
    {
        if(isDisplayDescription == false)
        {
            switch (photoType)
            {
                case PhotoType.MainQuestPhoto:
                    canvasMainQuestDescriptionView.interactable = false;
                    await Tween.Alpha(canvasMainQuestDescriptionView, 1f, 0.15f, Easing.Standard(Ease.Linear));
                    canvasMainQuestDescriptionView.interactable = true;
                    break;
                case PhotoType.SideQuestPhoto:
                    break;
                case PhotoType.AchivementPhoto:
                    canvasAchivementDescriptionView.interactable = false;
                    await Tween.Alpha(canvasAchivementDescriptionView, 1f, 0.15f, Easing.Standard(Ease.Linear));
                    canvasAchivementDescriptionView.interactable = true;
                    break;
            }
            isDisplayDescription = true;
        }
        else
        {
            switch (photoType)
            {
                case PhotoType.MainQuestPhoto:
                    canvasMainQuestDescriptionView.interactable = false;
                    await Tween.Alpha(canvasMainQuestDescriptionView, 0f, 0.15f, Easing.Standard(Ease.Linear));
                    break;
                case PhotoType.SideQuestPhoto:
                    break;
                case PhotoType.AchivementPhoto:
                    canvasAchivementDescriptionView.interactable = false;
                    await Tween.Alpha(canvasAchivementDescriptionView, 0f, 0.15f, Easing.Standard(Ease.Linear));
                    break;
            }
            isDisplayDescription = false;
        }
    }
    private async UniTaskVoid HidePhotoViewerUIToggle(float value)
    {
        if (photoType == PhotoType.MainQuestPhoto)
        {
            await Tween.Alpha(canvasMainQuestPhotoDetail, value, 0.15f, Easing.Standard(Ease.Linear))
                .Group(Tween.Alpha(canvasViewerUIDisplay, value, 0.15f, Easing.Standard(Ease.Linear)));
        }
        else
        {
            await Tween.Alpha(canvasAchivementPhotoDetail, value, 0.15f, Easing.Standard(Ease.Linear))
                .Group(Tween.Alpha(canvasViewerUIDisplay, value, 0.15f, Easing.Standard(Ease.Linear)));
        }
        if (value == 1)
        {
            canvasViewerUIDisplay.interactable = true;
        }
        else
        {
            canvasViewerUIDisplay.interactable = false;
        }
        isUIHidden = !isUIHidden;
        if (isUIHidden == true)
        {
            imageHideUIButton.sprite = spriteUnHideUI;
        }
        else
        {
            imageHideUIButton.sprite = spriteHideUI;
        }
    }
    private async UniTaskVoid ExitPhotoViewer()
    {
        await Tween.Alpha(canvasPhotoView, 0f, 0.15f, Easing.Standard(Ease.Linear));
        gameObject.SetActive(false);
        mainquestPhotoView.gameObject.SetActive(false);
        sidequestPhotoView.gameObject.SetActive(false);
        isDisplayDescription = false;
        if (photoType == PhotoType.MainQuestPhoto)
        {
            canvasMainQuestPhotoDetail.alpha = 1f;
            canvasMainQuestDescriptionView.alpha = 0f;
        }
        else
        {
            canvasAchivementPhotoDetail.alpha = 1f;
            canvasAchivementDescriptionView.alpha = 0f;
        }
        canvasViewerUIDisplay.alpha = 1;
        canvasViewerUIDisplay.interactable = true;
        PhoneUIManager.Instance.enabled = true;
    }
    private async UniTaskVoid OpenPhotoViewer()
    {
        gameObject.SetActive(true);
        await Tween.Alpha(canvasPhotoView, 1f, 0.15f, Easing.Standard(Ease.Linear));
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        HidePhotoViewerUIToggle(1f).Forget();
        isUIHidden = true;
    }
}
