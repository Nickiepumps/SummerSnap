using UnityEngine;

public class NPCDialog : MonoBehaviour
{
    public DialogueLine[] lines;

    void OnMouseDown()
    {
        NPCDialogManager manager = FindObjectOfType<NPCDialogManager>();
        if (manager != null)
        {
            manager.StartDialog(lines);
        }
        else
        {
            Debug.LogWarning("No NPCDialogManager found in this scene!");
        }
    }
}
