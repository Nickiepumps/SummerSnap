using UnityEngine;
public class NPCDisplayManager : MonoBehaviour
{
    public static NPCDisplayManager instance;

    [SerializeField] private GameObject[] moveablenpcCharacterArr;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public void ToggleNPCDisplay(bool active)
    {
        foreach (GameObject npc in moveablenpcCharacterArr)
        {
            npc.SetActive(active);
        }
    }
}
