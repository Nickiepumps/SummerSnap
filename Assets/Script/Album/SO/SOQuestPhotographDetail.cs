using UnityEngine;

[CreateAssetMenu(fileName = "QuestPhotographDetail", menuName = "Scriptable Objects/QuestPhotographDetail")]
public class SOQuestPhotographDetail : ScriptableObject
{
    public string photoName;
    [TextArea] public string description;
    public Sprite photoSprite;
}
