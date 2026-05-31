using UnityEngine;

public class InfluencerBlink : MonoBehaviour
{
    public Sprite openEyesSprite;   // รูปลืมตา
    public Sprite closedEyesSprite; // รูปหลับตา
    public float blinkInterval = 3f; // เวลาระหว่างการกระพริบ (วินาที)
    public float closedDuration = 0.2f; // เวลาที่หลับตา (สั้น ๆ)

    private SpriteRenderer spriteRenderer;
    private float timer;
    private bool isBlinking;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        timer = blinkInterval;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (!isBlinking && timer <= 0f)
        {
            // เปลี่ยนเป็นรูปหลับตา
            spriteRenderer.sprite = closedEyesSprite;
            isBlinking = true;
            timer = closedDuration;
        }
        else if (isBlinking && timer <= 0f)
        {
            // กลับเป็นลืมตา
            spriteRenderer.sprite = openEyesSprite;
            isBlinking = false;
            timer = blinkInterval;
        }
    }

    public bool IsEyesClosed()
    {
        return spriteRenderer.sprite == closedEyesSprite;
    }
}
