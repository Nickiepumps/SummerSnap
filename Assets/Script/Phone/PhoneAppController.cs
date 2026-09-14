using System.Collections;
using UnityEngine;

public class PhoneAppController : MonoBehaviour
{
    [Header("Phone Settings")]
    public AudioClip phoneRingClip; // เสียงโทรศัพท์
    public GameObject callingUI;
    public float phoneRingLoopDelay = 0.3f;

    private AudioSource audioSource;
    private RyuController ryuController;
    private Coroutine phoneRingCoroutine;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        var ryu = GameManager.Instance.GetRyu();
        if (ryu != null)
        {
            ryuController = ryu.GetComponent<RyuController>();
            if (ryuController != null)
            {
                // ลบ HandleRyuArrived ที่เล่นเสียงหัวเราะ
                ryuController.OnArrivedAtPlayer += HandleRyuArrivedUIOnly;
            }
        }
    }

    public void CallRyuFromPhone()
    {
        if (ryuController != null)
        {
            PersistentUI.instance.TogglePhoneUI(false);

            ryuController.CallRyu();

            if (callingUI != null)
                callingUI.SetActive(true);

            if (phoneRingCoroutine != null)
                StopCoroutine(phoneRingCoroutine);
            phoneRingCoroutine = StartCoroutine(PhoneRingLoop());
        }
    }

    private IEnumerator PhoneRingLoop()
    {
        while (ryuController != null && ryuController.IsCalling())
        {
            if (audioSource != null && phoneRingClip != null)
                audioSource.PlayOneShot(phoneRingClip);

            yield return new WaitForSeconds(phoneRingClip.length + phoneRingLoopDelay);
        }
    }
    private void HandleRyuArrivedUIOnly()
    {
        PersistentUI.instance.TogglePhoneUI(true);

        if (phoneRingCoroutine != null)
        {
            StopCoroutine(phoneRingCoroutine);
            phoneRingCoroutine = null;
        }

        if (callingUI != null && callingUI.activeSelf)
            callingUI.SetActive(false);

        PhoneUIManager.Instance.TogglePhone();
    }

    private void OnDestroy()
    {
        if (ryuController != null)
            ryuController.OnArrivedAtPlayer -= HandleRyuArrivedUIOnly;
    }
}
