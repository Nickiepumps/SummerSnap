using UnityEngine;

public class PowderEventController : MonoBehaviour
{
    private bool hasTriggered = false;

    void Update()
    {
        if (hasTriggered) return;

        // เช็คจาก GameManager ว่า dialog ปัจจุบันมี powder หรือไม่
        if (GameManager.Instance != null && GameManager.Instance.IsCurrentDialogPowdered())
        {
            var player = GameManager.Instance.GetPlayerGameObject();
            if (player != null)
            {
                var controller = player.GetComponent<PlayerController>();
                if (controller != null)
                {
                    controller.SetPowderOnFace(true);
                    Debug.Log("Player got powdered via PowderEventController!");
                    hasTriggered = true; // ป้องกัน trigger ซ้ำ
                }
            }
        }
    }
}
