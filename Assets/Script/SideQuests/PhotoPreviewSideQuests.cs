using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PhotoPreviewSideQuests : MonoBehaviour
{
    public GameObject previewCanvas;
    public Image previewImage;
    public Image frameImage;

    public Button takeButton;
    public Button cancelButton;

    public AudioSource cameraSound;
    public float previewTime = 1.5f;
    public float animationTime = 0.3f;

    private Sprite currentSprite;
    private string currentQuestId;

    private void Start()
    {
        var canvasComp = previewCanvas.GetComponent<Canvas>();
        if (canvasComp != null)
            canvasComp.sortingOrder = 100;

        previewCanvas.SetActive(false);
        previewImage.gameObject.SetActive(false);

        if (frameImage != null) frameImage.gameObject.SetActive(false);
        if (takeButton != null) takeButton.gameObject.SetActive(false);
        if (cancelButton != null) cancelButton.gameObject.SetActive(false);

        if (takeButton != null) takeButton.onClick.AddListener(OnTakePhoto);
        if (cancelButton != null) cancelButton.onClick.AddListener(OnCancel);
    }

    public void ShowButtons(Sprite sprite, string questId)
    {
        if (sprite == null) return;

        if (GameManager.Instance != null && GameManager.Instance.HasTaken(questId))
        {
            Debug.Log($"[PhotoPreview] {questId} เคยถ่ายแล้ว");
            return;
        }

        currentSprite = sprite;
        currentQuestId = questId;

        previewCanvas.SetActive(true);

        if (takeButton != null) takeButton.gameObject.SetActive(true);
        if (cancelButton != null) cancelButton.gameObject.SetActive(true);

        previewImage.gameObject.SetActive(false);
        if (frameImage != null) frameImage.gameObject.SetActive(false);
    }

    public void OnTakePhoto()
    {
        if (currentSprite == null) return;

        if (cameraSound != null) cameraSound.Play();

        previewImage.sprite = currentSprite;
        previewImage.SetNativeSize();
        previewImage.gameObject.SetActive(true);

        if (frameImage != null) frameImage.gameObject.SetActive(true);
        if (takeButton != null) takeButton.gameObject.SetActive(false);
        if (cancelButton != null) cancelButton.gameObject.SetActive(false);

        previewImage.transform.localScale = Vector3.one * 1.5f;
        if (frameImage != null) frameImage.transform.localScale = Vector3.one * 1.5f;

        StartCoroutine(AnimatePreview());
    }

    private IEnumerator AnimatePreview()
    {
        float elapsed = 0f;
        while (elapsed < animationTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationTime;
            float scale = Mathf.Lerp(1.5f, 1f, t);
            previewImage.transform.localScale = Vector3.one * scale;
            if (frameImage != null) frameImage.transform.localScale = Vector3.one * scale;
            yield return null;
        }

        yield return new WaitForSeconds(previewTime);

        elapsed = 0f;
        while (elapsed < animationTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / animationTime;
            float scale = Mathf.Lerp(1f, 0f, t);
            previewImage.transform.localScale = Vector3.one * scale;
            if (frameImage != null) frameImage.transform.localScale = Vector3.one * scale;
            yield return null;
        }

        ClosePreview(true); // ✅ กรณีถ่ายรูปจริง
    }

    public void OnCancel()
    {
        if (!string.IsNullOrEmpty(currentQuestId))
        {
            var objs = FindObjectsOfType<PhotoQuestObject>();
            foreach (var obj in objs)
            {
                if (obj.questId == currentQuestId)
                {
                    obj.ResetClicked();
                    break;
                }
            }
        }

        ClosePreview(false); // ❌ กดยกเลิก = แค่ปิด UI
    }


    private void ClosePreview(bool isPhotoTaken)
    {
        previewImage.gameObject.SetActive(false);
        if (frameImage != null) frameImage.gameObject.SetActive(false);
        if (takeButton != null) takeButton.gameObject.SetActive(false);
        if (cancelButton != null) cancelButton.gameObject.SetActive(false);
        previewCanvas.SetActive(false);

        if (isPhotoTaken)
        {
            if (currentSprite != null)
            {
                // ✅ เก็บรูปลง SideQuest Storage
                if (SideQuestPhotoStorage.Instance != null)
                    SideQuestPhotoStorage.Instance.AddPhoto(currentSprite);
            }

            if (!string.IsNullOrEmpty(currentQuestId))
            {
                var objs = FindObjectsOfType<PhotoQuestObject>();
                foreach (var obj in objs)
                {
                    if (obj.questId == currentQuestId)
                    {
                        obj.MarkTaken();
                        break;
                    }
                }
            }
        }

        currentSprite = null;
        currentQuestId = null;
    }

}
