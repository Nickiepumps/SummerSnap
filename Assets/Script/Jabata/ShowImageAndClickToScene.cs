using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
public class ShowImageAndClickToScene : MonoBehaviour
{
    [SerializeField] private PhotoPreviewSideQuests photoPreviewSideQuests;

    public GameObject imageObject;
    public GameObject warningImage;
    public string sceneToLoad;
    public string questId;

    private bool isPlayerInside = false;
    private Coroutine warningCoroutine;

    public Camera mainCam;

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
        if (photoPreviewSideQuests.GetSideQuestActiveStatus() == true) return;

        GameObject player = GameManager.Instance?.GetPlayer();
        GameObject ryu = GameManager.Instance?.GetRyu();

        if (player != null && ryu != null)
        {
            float distance = Vector2.Distance(player.transform.position, ryu.transform.position);

            if (distance > 5f)
            {
                if (warningImage != null)
                {
                    if (warningCoroutine != null) StopCoroutine(warningCoroutine);
                    warningCoroutine = StartCoroutine(ShowWarningForSeconds(2f));
                }
                return;
            }
        }

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
