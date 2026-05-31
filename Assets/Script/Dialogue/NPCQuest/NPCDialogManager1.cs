using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class NPCDialogManager1 : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialogPanel;
    public TMP_Text dialogText;
    public Button[] choiceButtons;

    [Header("Settings")]
    public float inputDelay = 0.2f;
    public KeyCode nextKey = KeyCode.Space;

    private DialogueLine1[] currentLines;
    private int currentIndex;
    private bool waitingForChoice;
    private bool canInput = true;

    void Awake()
    {
        if (dialogPanel != null)
            dialogPanel.SetActive(false);
    }

    void Update()
    {
        if (dialogPanel.activeSelf && !waitingForChoice && canInput)
        {
            if (Input.GetKeyDown(nextKey) || Input.GetMouseButtonDown(0))
            {
                StartCoroutine(InputCooldown());
                NextLine();
            }
        }
    }

    public void StartDialog(DialogueLine1[] lines)
    {
        if (lines == null || lines.Length == 0) return;

        EndDialog();

        currentLines = lines;
        currentIndex = 0;
        waitingForChoice = false;

        dialogPanel.SetActive(true);
        StartCoroutine(BlockInputShortly());
        StartCoroutine(ShowLineNextFrame());
    }

    private IEnumerator ShowLineNextFrame()
    {
        yield return null;
        ShowLine();
    }

    private void ShowLine()
    {
        if (currentLines == null || currentIndex < 0 || currentIndex >= currentLines.Length)
        {
            EndDialog();
            return;
        }

        DialogueLine1 line = currentLines[currentIndex];
        dialogText.text = line.text;

        if (line.isChoice && line.choices.Length > 0)
        {
            ShowChoices(line);
        }
        else
        {
            HideChoices();
        }
    }

    private void ShowChoices(DialogueLine1 line)
    {
        waitingForChoice = true;

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < line.choices.Length)
            {
                choiceButtons[i].gameObject.SetActive(true);
                choiceButtons[i].GetComponentInChildren<TMP_Text>().text = line.choices[i];

                int choiceIndex = i;
                choiceButtons[i].onClick.RemoveAllListeners();
                choiceButtons[i].onClick.AddListener(() => SelectChoice(choiceIndex));
            }
            else
            {
                choiceButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void HideChoices()
    {
        waitingForChoice = false;
        foreach (var btn in choiceButtons)
            btn.gameObject.SetActive(false);
    }

    private void NextLine()
    {
        if (currentLines == null) return;

        DialogueLine1 line = currentLines[currentIndex];

        if (!line.isChoice && line.nextLineIndices != null && line.nextLineIndices.Length == 1)
        {
            currentIndex = line.nextLineIndices[0];
        }
        else
        {
            currentIndex++;
        }

        if (currentIndex >= currentLines.Length)
        {
            EndDialog();
            return;
        }

        ShowLine();
    }

    private void SelectChoice(int index)
    {
        if (currentLines == null) return;

        DialogueLine1 line = currentLines[currentIndex];

        if (line.nextLineIndices != null && index < line.nextLineIndices.Length)
        {
            currentIndex = line.nextLineIndices[index];
        }
        else
        {
            currentIndex++;
        }

        if (currentIndex >= currentLines.Length)
        {
            EndDialog();
            return;
        }

        waitingForChoice = false;
        ShowLine();
    }

    private void EndDialog()
    {
        if (dialogPanel != null)
            dialogPanel.SetActive(false);

        if (dialogText != null)
            dialogText.text = "";

        HideChoices();
        currentLines = null;
        currentIndex = 0;
        waitingForChoice = false;
    }

    private IEnumerator InputCooldown()
    {
        canInput = false;
        yield return new WaitForSeconds(inputDelay);
        canInput = true;
    }

    private IEnumerator BlockInputShortly()
    {
        canInput = false;
        yield return new WaitForSeconds(0.2f);
        canInput = true;
    }
}
