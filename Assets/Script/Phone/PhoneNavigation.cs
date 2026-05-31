using UnityEngine;
using System.Collections.Generic;

public class PhoneNavigation : MonoBehaviour
{
    [System.Serializable]
    public class PanelEntry
    {
        public string panelName;
        public GameObject panelObj;
    }

    public List<PanelEntry> panels = new List<PanelEntry>();

    private Dictionary<string, GameObject> panelDict;
    private GameObject currentPanel;

    void Awake()
    {
        panelDict = new Dictionary<string, GameObject>();
        foreach (var entry in panels)
        {
            panelDict[entry.panelName] = entry.panelObj;
            entry.panelObj.SetActive(false); // ปิดทุก panel
        }

        if (panelDict.ContainsKey("Home"))
            ShowPanel("Home");
    }

    public void ShowPanel(string panelName)
    {
        if (currentPanel != null)
            currentPanel.SetActive(false);

        if (panelDict.ContainsKey(panelName))
        {
            currentPanel = panelDict[panelName];
            currentPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("ไม่มี Panel ชื่อ: " + panelName);
        }
    }
}
