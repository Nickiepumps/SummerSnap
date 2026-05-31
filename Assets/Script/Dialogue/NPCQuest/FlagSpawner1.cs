using UnityEngine;

public class FlagSpawner1 : MonoBehaviour
{
    public GameObject flagPrefab;
    public string flagID;

    void Start()
    {
        // ถ้าเคยเก็บธงนี้แล้ว → ไม่ต้อง spawn
        if (!QuestManager1.Instance.HasCollectedFlag(flagID))
        {
            GameObject flag = Instantiate(flagPrefab, transform.position, transform.rotation);
            flag.GetComponent<Flag1>().flagID = flagID;
        }
    }
}
