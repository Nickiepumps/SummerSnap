using System.Collections.Generic;
using UnityEngine;

public class QuestManager1 : MonoBehaviour
{
    public static QuestManager1 Instance;

    private HashSet<string> collectedFlags = new HashSet<string>();

    public int totalFlagsNeeded = 2;
    private int flagsCollected = 0;

    private bool flagQuestActive = false;
    private bool flagQuestCompleted = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void StartFlagQuest()
    {
        flagQuestActive = true;
        flagsCollected = 0;
        Debug.Log("Flag Quest started!");
    }

    public bool IsFlagQuestActive() => flagQuestActive;
    public bool IsFlagQuestCompleted() => flagQuestCompleted;
    public bool HasCollectedAllFlags() => flagsCollected >= totalFlagsNeeded;

    public void CollectFlag(string flagID)
    {
        if (!collectedFlags.Contains(flagID))
        {
            collectedFlags.Add(flagID);
            flagsCollected++;
            Debug.Log($"Collected {flagID}. Total = {flagsCollected}");

            if (flagsCollected >= totalFlagsNeeded)
            {
                Debug.Log("All flags collected! Go talk to NPC.");
            }
        }
    }

    public void CompleteFlagQuest()
    {
        flagQuestActive = false;
        flagQuestCompleted = true;
        Debug.Log("Flag Quest completed! Unlock MainQuest3 here.");
        // TODO: unlock MainQuest3
    }

    public bool HasCollectedFlag(string flagID)
    {
        return collectedFlags.Contains(flagID);
    }
}
