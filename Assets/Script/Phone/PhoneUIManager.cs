using UnityEngine;
using Unity.Cinemachine;

public class PhoneUIManager : MonoBehaviour
{
    public static PhoneUIManager Instance;

    [Header("UI")]
    public GameObject phoneUI;   // UI โทรศัพท์ทั้งหมด
    public PhoneNavigation phoneNavigation;
    [Header("Ryu Coming")]
    public GameObject ryuComingImage; // UI "ริวกำลังไป"

    [Header("Cameras")]
    public CinemachineCamera vcamNormal;
    public CinemachineCamera vcamPhone;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip openPhoneSFX;   // เสียงตอนเปิดโทรศัพท์
    public AudioClip closePhoneSFX;  // เสียงตอนปิดโทรศัพท์

    public bool isOpen { get; private set; } = false;
    private GameManager gameManager => GameManager.Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (ryuComingImage != null)
                DontDestroyOnLoad(ryuComingImage);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TogglePhone();
        }
    }
    public void InitializeVirtualCamera()
    {
        CinemachineCamera cinemachineCam = GameObject.Find("CinemachineCamera").GetComponent<CinemachineCamera>();
        vcamNormal = cinemachineCam;
        if (vcamPhone != null && gameManager != null)
        {
            GameObject player = gameManager.GetPlayer();
            vcamPhone.Target.TrackingTarget = player.transform;
        }
    }

    public void TogglePhone()
    {
        PlayerController player = GameManager.Instance.GetPlayerController();

        if (player.isOnDialogue == true || GameManager.Instance.isPhotoshooting == true) return;

        isOpen = !isOpen;
        if (phoneUI != null) phoneUI.SetActive(isOpen);

        if (player != null)
        {
            player.canMove = !isOpen;
        }

        // 🎵 เล่นเสียง
        if (audioSource != null)
        {
            if (isOpen && openPhoneSFX != null)
                audioSource.PlayOneShot(openPhoneSFX);
            else if (!isOpen && closePhoneSFX != null)
                audioSource.PlayOneShot(closePhoneSFX);
        }

        if (isOpen)
        {
            vcamPhone.gameObject.SetActive(true);
            if (phoneNavigation != null)
                phoneNavigation.ShowPanel("Home");

            if (vcamPhone != null) vcamPhone.Priority = 20;
            if (vcamNormal != null) vcamNormal.Priority = 10;
        }
        else
        {
            if (vcamPhone != null) vcamPhone.Priority = 5;
            if (vcamNormal != null) vcamNormal.Priority = 20;
            vcamPhone.gameObject.SetActive(false);
        }
    }

    public void RegisterSceneCamera(CinemachineCamera cam)
    {
        vcamNormal = cam;

        if (!isOpen && vcamNormal != null)
        {
            vcamNormal.Priority = 20;
        }
    }

    public void ShowRyuComing(float duration = 2f)
    {
        if (ryuComingImage == null) return;

        ryuComingImage.SetActive(true);
        ryuComingImage.transform.SetAsLastSibling();

        CancelInvoke(nameof(HideRyuComing));
        Invoke(nameof(HideRyuComing), duration);
    }

    private void HideRyuComing()
    {
        if (ryuComingImage != null)
            ryuComingImage.SetActive(false);
    }
}
