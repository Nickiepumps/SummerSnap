using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FogController : MonoBehaviour
{
    public GameObject fogImage; // SpriteRenderer ของหมอก
    public GameObject fogButtonPrefab; // Prefab ปุ่ม fog
    public RectTransform spawnArea; // พื้นที่สุ่มปุ่ม (UI)

    public int minButtons = 2;
    public int maxButtons = 5;
    public int maxFogRounds = 3; // จำนวนรอบหมอกสูงสุด
    private int currentFogRound = 0; // รอบหมอกปัจจุบัน

    private List<GameObject> activeButtons = new List<GameObject>();
    private bool fogActive = false; // ตอนนี้ใช้แล้ว ไม่มี warning แล้ว

    public void ShowFog()
    {
        if (currentFogRound >= maxFogRounds || fogActive)
            return;

        currentFogRound++;
        fogActive = true;

        fogImage.SetActive(true);
        GenerateButtons();
    }

    public void HideFog()
    {
        fogImage.SetActive(false);

        foreach (GameObject btn in activeButtons)
        {
            Destroy(btn);
        }

        activeButtons.Clear();

        // ถ้ายังไม่ถึงรอบสุดท้าย → รอสุ่มเวลาแล้วแสดง fog ใหม่
        if (currentFogRound < maxFogRounds)
        {
            float delay = Random.Range(2f, 3f);
            Invoke("ShowFog", delay);
        }
    }

    private void GenerateButtons()
    {
        int buttonCount = Random.Range(minButtons, maxButtons + 1);
        for (int i = 0; i < buttonCount; i++)
        {
            GameObject btnObj = Instantiate(fogButtonPrefab, spawnArea);
            btnObj.GetComponent<RectTransform>().anchoredPosition = GetRandomPosition();

            int clickTimes = Random.Range(1, 4); // ต้องคลิก 1–3 ครั้ง
            btnObj.GetComponent<FogButton>().Setup(clickTimes, this);
            activeButtons.Add(btnObj);
        }
    }

    private Vector2 GetRandomPosition()
    {
        float x = Random.Range(-spawnArea.rect.width / 2, spawnArea.rect.width / 2);
        float y = Random.Range(-spawnArea.rect.height / 2, spawnArea.rect.height / 2);
        return new Vector2(x, y);
    }

    public void RemoveButton(GameObject btn)
    {
        activeButtons.Remove(btn);
        Destroy(btn);

        if (activeButtons.Count == 0)
        {
            // ทุกปุ่มถูกกดจนหมด → ค่อย ๆ จางหมอก
            StartCoroutine(FadeOutFog());
        }
    }

    private IEnumerator FadeOutFog()
    {
        SpriteRenderer sr = fogImage.GetComponent<SpriteRenderer>();
        Color color = sr.color;

        for (float a = 1f; a >= 0f; a -= 0.05f)
        {
            color.a = a;
            sr.color = color;
            yield return new WaitForSeconds(0.05f);
        }

        sr.color = new Color(1, 1, 1, 1); // รีเซตความทึบ
        fogImage.SetActive(false);
        fogActive = false;

        yield return new WaitForSeconds(0.1f); // หน่วงนิดก่อนเริ่มรอบใหม่
        HideFog(); // ไปต่อรอบถัดไป
    }
}
