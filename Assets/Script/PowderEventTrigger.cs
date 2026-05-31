using UnityEngine;

public class PowderEventTrigger : MonoBehaviour
{
    public PlayerController playerController;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;

            var controller = other.GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.SetPowderOnFace(true);
            }
            else
            {
                Debug.LogError("PowderEventTrigger: PlayerController not found on Player!");
            }
        }
    }

    public void TriggerPowderEvent()
    {
        playerController.SetPowderOnFace(true); // เรียกให้เปลี่ยนสภาพ player
        // อาจเปิด dialog ด้วยก็ได้
    }
}
