using UnityEngine;

public class FogTrigger : MonoBehaviour
{
    public float delayBeforeFog = 5f;

    void Start()
    {
        Invoke("TriggerFog", delayBeforeFog);
    }

    void TriggerFog()
    {
        FindObjectOfType<FogController>().ShowFog();
    }
}
