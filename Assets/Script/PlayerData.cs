using System.Collections.Generic;
using UnityEngine;
public class PlayerData
{
    public Vector2 lastPosition = new Vector2(-8.6f, -2.39f);
    public string lastSpawnPoint = "";
    public bool hasPowder = false;
    public List<string> inventory = new List<string>();
    public bool hasClearMovementTutorial = false;
    public bool hasClearSideQuestTutorial = false;
    public bool hasClearRyuPhoneCallTutorial = false;
}
