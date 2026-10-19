using UnityEngine;
public class TriggerSideQuestTutorial : MonoBehaviour
{
    [SerializeField] private PlayerControlTutorialManager playerControlTutorialManager;
    [SerializeField] private BoxCollider2D boxCollider;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerControlTutorialManager.TriggerSidequestTutorial();
            boxCollider.enabled = false;
        }
    }
}
