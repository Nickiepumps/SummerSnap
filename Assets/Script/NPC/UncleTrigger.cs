using UnityEngine;

public class UncleTrigger : MonoBehaviour
{
    public Animator npcAnimator; // Animator ของ NPC
    public string playerTag = "Player"; // Tag ที่ใช้กับ Player
    public int triggerValue = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            npcAnimator.SetInteger("UncleAct", triggerValue);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            npcAnimator.SetInteger("UncleAct", 0); // กลับไป idle หรือไม่ก็ได้
        }
    }
}
