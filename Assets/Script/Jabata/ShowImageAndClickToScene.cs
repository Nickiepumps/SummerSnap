using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ShowImageAndClickToScene : MonoBehaviour
{
    public GameObject imageObject;   // รูป Indicator (ปุ่มกดเควส)
    public GameObject warningImage;  // รูปเตือน (ถ้า Ryu ไกลเกินไป)
    public string sceneToLoad;
    public string questId;

    private bool isPlayerInside = false;
    private Coroutine warningCoroutine;

    [SerializeField] public Camera mainCam;

    void Start()
    {
        if (imageObject != null) imageObject.SetActive(false);
        if (warningImage != null) warningImage.SetActive(false);

        if (GameManager.Instance != null && GameManager.Instance.HasTriggeredMainQuest(questId))
        {
            if (imageObject != null) imageObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isPlayerInside && PhoneUIManager.Instance.isOpen == false && Input.GetMouseButtonDown(0))
        {
            TryStartQuest();
        }
    }

    private void TryStartQuest()
    {
        GameObject player = GameManager.Instance?.GetPlayer();
        GameObject ryu = GameManager.Instance?.GetRyu();

        if (player != null && ryu != null)
        {
            float distance = Vector2.Distance(player.transform.position, ryu.transform.position);

            if (distance > 5f) // ถ้า Ryu ไกลเกินไป
            {
                Debug.Log("⚠ Ryu ไกลเกินไป เริ่มเควสไม่ได้");

                if (warningImage != null)
                {
                    if (warningCoroutine != null) StopCoroutine(warningCoroutine);
                    warningCoroutine = StartCoroutine(ShowWarningForSeconds(2f));
                }

                return; // ❌ ไม่เข้าเควส
            }
        }

        // ✅ ถ้า Ryu อยู่ใกล้พอ
        if (warningImage != null) warningImage.SetActive(false);

        if (GameManager.Instance != null)
            GameManager.Instance.MarkMainQuestTriggered(questId);

        if (player != null)
            PlayerPositionManager.SavePlayerPosition(player.transform.position);

        GameManager.Instance.TogglePhotoshootStatus(true);
        SceneManager.LoadScene(sceneToLoad);
    }

    private IEnumerator ShowWarningForSeconds(float seconds)
    {
        warningImage.SetActive(true);
        yield return new WaitForSeconds(seconds);
        warningImage.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.Instance != null && GameManager.Instance.HasTriggeredMainQuest(questId))
            return;

        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (imageObject != null) imageObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (imageObject != null) imageObject.SetActive(false);
            if (warningImage != null) warningImage.SetActive(false);
        }
    }
}
