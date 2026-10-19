using UnityEngine;

public class TriggerRyuPhoneTutorial : MonoBehaviour
{
    [SerializeField] private PlayerControlTutorialManager playerControlTutorialManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerControlTutorialManager.TriggerRyuPhoneTutorial();
        }
    }
}
