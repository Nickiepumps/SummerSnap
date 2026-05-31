using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public Vector2 lastPosition = new Vector2(-1.9f, -2.39f);
    public string lastSpawnPoint = "";
    public bool hasPowder = false;
    public List<string> inventory = new List<string>();

}
