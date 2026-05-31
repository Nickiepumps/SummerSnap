using UnityEngine;
using UnityEngine.UI;

public class PhotoPreviewUI : MonoBehaviour
{
    public GameObject panel;
    public Image photoImage;
    public GameObject goodEffect; // เฉพาะ 3 ดาว
    public GameObject badEffect;  // เฉพาะ 0 ดาว


    void Start()
    {
        if (panel != null) panel.SetActive(false);
        if (goodEffect != null) goodEffect.SetActive(false);
        if (badEffect != null) badEffect.SetActive(false);

    }

    public void ShowPreview(Sprite photoSprite, int stars)
    {
        if (panel != null) panel.SetActive(true);

        if (photoImage != null)
            photoImage.sprite = photoSprite;


        // Reset effect
        if (goodEffect != null) goodEffect.SetActive(false);
        if (badEffect != null) badEffect.SetActive(false);

        // เงื่อนไข effect
        if (stars == 3 && goodEffect != null)
            goodEffect.SetActive(true);
        else if (stars == 0 && badEffect != null)
            badEffect.SetActive(true);
        // 1–2 ดาว → ไม่มี effect
    }

    public void HidePreview()
    {
        if (panel != null) panel.SetActive(false);
        if (goodEffect != null) goodEffect.SetActive(false);
        if (badEffect != null) badEffect.SetActive(false);

        
    }
}
