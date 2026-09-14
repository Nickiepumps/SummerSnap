using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;

public class DialogManager : MonoBehaviour
{
    public enum Speaker { Left, Right }
    public enum Emotion { Normal, Angry, Confident, Sad, Smile, Surprised, SweatDrop }
    public enum ChoiceEffectType { None, Heart, Fail }

    [Serializable]
    public class CharacterSprites
    {
        public Sprite normal, normalGray;
        public Sprite angry, angryGray;
        public Sprite confident, confidentGray;
        public Sprite sad, sadGray;
        public Sprite smile, smileGray;
        public Sprite surprised, surprisedGray;
        public Sprite sweatDrop, sweatDropGray;

        public Sprite GetSprite(Emotion emotion, bool isGray)
        {
            switch (emotion)
            {
                case Emotion.Angry: return isGray ? angryGray : angry;
                case Emotion.Confident: return isGray ? confidentGray : confident;
                case Emotion.Sad: return isGray ? sadGray : sad;
                case Emotion.Smile: return isGray ? smileGray : smile;
                case Emotion.Surprised: return isGray ? surprisedGray : surprised;
                case Emotion.SweatDrop: return isGray ? sweatDropGray : sweatDrop;
                default: return isGray ? normalGray : normal;
            }
        }
    }

    [Serializable]
    public class DialogLine
    {
        public Speaker speaker;
        public Emotion emotion;
        [TextArea(2, 5)] public string text;

        [Header("Background Change")]
        public bool changeBackground;
        public bool hideCharacters;
        public Sprite newBackground;

        [Header("Choices")]
        public bool hasChoices;
        public Choice[] choices;

        [Header("Next Line")]
        public int nextLineIndex = -1; // -1 = ใช้ค่าต่อไปตามลำดับ
    }

    [Serializable]
    public class Choice
    {
        public string choiceText;
        public int nextLineIndex;
        public ChoiceEffectType effectType;

        [Header("Choice Result Dialog")]
        [TextArea(2, 5)] public string choiceResultText;
        public Speaker speaker;
        public Emotion emotion = Emotion.Normal;
    }

    [SerializeField] private Volume blurVolume;

    [SerializeField] private GameObject[] moveablenpcCharacterArr;
    private SpriteRenderer[] mainCharacterSpriteRendererArr = new SpriteRenderer[2];

    [Header("Character Sprites (No Powder)")]
    public CharacterSprites leftCharacterSprites;
    public CharacterSprites rightCharacterSprites;

    [Header("Character Sprites (With Powder)")]
    public CharacterSprites leftCharacterSpritesWithPowder;
    public CharacterSprites rightCharacterSpritesWithPowder;

    [Header("Dialog Data")]
    public DialogLine[] dialogueLine;

    [Header("UI Components")]
    public Canvas dialogCanvas;
    public GameObject dialogPanel;
    public TMP_Text dialogText;
    public Image leftCharacterImage;
    public Image rightCharacterImage;
    public Image backgroundImage;
    [SerializeField] private Image imageNextDialogue;

    [Header("Choice UI")]
    public GameObject choicesPanel;
    public Button choiceButtonPrefab;

    [Header("State")]
    public bool hasPowder = false;

    private int currentLineIndex = 0;
    private bool isWaitingForChoice = false;

    private bool isShowingChoiceResult = false;
    private int pendingNextLineIndex = -1;

    public Action onDialogEnd;

    private PlayerController playerController;

    private void Start()
    {
        Initialize();
    }
    void Update()
    {
        if (!dialogPanel.activeSelf) return;

        if (isShowingChoiceResult)
        {
            if (Input.GetKeyDown(KeyCode.Space)) 
            {
                isShowingChoiceResult = false;
                currentLineIndex = pendingNextLineIndex;
                ShowLine(currentLineIndex);
            }
        }
        else if (!isWaitingForChoice)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextLine();
            }
        }
    }
    private void Initialize()
    {
        playerController = GameManager.Instance.GetPlayerController();
        mainCharacterSpriteRendererArr[0] = GameManager.Instance.GetPlayer().GetComponent<SpriteRenderer>();
        mainCharacterSpriteRendererArr[1] = GameManager.Instance.GetRyu().GetComponent<SpriteRenderer>();
    }

    public void SetDialog(DialogData data)
    {
        dialogueLine = data.lines;
    }

    public void StartDialog()
    {
        if (dialogueLine == null || dialogueLine.Length == 0)
        {
            Debug.LogWarning("DialogManager: ไม่มีบทสนทนาที่จะเล่น");
            return;
        }

        blurVolume.weight = 1f;

        foreach(SpriteRenderer sprite in mainCharacterSpriteRendererArr)
        {
            sprite.enabled = false;
        }
        foreach(GameObject npc in moveablenpcCharacterArr)
        {
            npc.SetActive(false);
        }

        playerController.TogglePlayerDialogue(true);
        currentLineIndex = 0;
        if (dialogCanvas != null) dialogCanvas.gameObject.SetActive(true);
        if (dialogPanel != null) dialogPanel.SetActive(true);

        ShowLine(currentLineIndex);
    }

    void ShowLine(int index)
    {
        if (index >= dialogueLine.Length)
        {
            EndDialog();
            return;
        }

        DialogLine line = dialogueLine[index];
        dialogText.text = line.text;

        if(line.hideCharacters == true)
        {
            leftCharacterImage.enabled = false;
            rightCharacterImage.enabled = false;
        }
        else
        {
            leftCharacterImage.enabled = true;
            rightCharacterImage.enabled = true;

            CharacterSprites leftSprites = hasPowder ? leftCharacterSpritesWithPowder : leftCharacterSprites;
            CharacterSprites rightSprites = hasPowder ? rightCharacterSpritesWithPowder : rightCharacterSprites;

            if (line.speaker == Speaker.Left)
            {
                leftCharacterImage.sprite = leftSprites.GetSprite(line.emotion, false);
                rightCharacterImage.sprite = rightSprites.GetSprite(Emotion.Normal, true);
            }
            else
            {
                leftCharacterImage.sprite = leftSprites.GetSprite(Emotion.Normal, true);
                rightCharacterImage.sprite = rightSprites.GetSprite(line.emotion, false);
            }
        }

        if (line.changeBackground && line.newBackground != null)
        {
            backgroundImage.gameObject.SetActive(true);
            backgroundImage.sprite = line.newBackground;
        }
        else
        {
            backgroundImage.gameObject.SetActive(false);
        }

        if (line.hasChoices && line.choices.Length > 0)
        {
            isWaitingForChoice = true;
            ShowChoices(line.choices);
        }
        else
        {
            isWaitingForChoice = false;
            choicesPanel.SetActive(false);
        }
    }

    void ShowChoices(Choice[] choices)
    {
        choicesPanel.SetActive(true);
        imageNextDialogue.enabled = false;

        foreach (Transform child in choicesPanel.transform)
            Destroy(child.gameObject);

        foreach (var choice in choices)
        {
            var btnObj = Instantiate(choiceButtonPrefab, choicesPanel.transform);
            var btnGO = btnObj.gameObject;
            var btn = btnObj.GetComponent<Button>();

            btn.GetComponentInChildren<TMP_Text>().text = choice.choiceText;

            var heartIcon = btnGO.transform.Find("HeartIcon")?.gameObject;
            var failIcon = btnGO.transform.Find("FailIcon")?.gameObject;

            if (heartIcon == null) Debug.LogWarning("HeartIcon not found on button!");
            if (failIcon == null) Debug.LogWarning("FailIcon not found on button!");

            if (heartIcon != null) heartIcon.SetActive(false);
            if (failIcon != null) failIcon.SetActive(false);

            btn.onClick.AddListener(() =>
            {
                Debug.Log("Button clicked! EffectType: " + choice.effectType);

                // แสดง Icon ก่อน
                if (choice.effectType == ChoiceEffectType.Heart && heartIcon != null)
                    heartIcon.SetActive(true);
                else if (choice.effectType == ChoiceEffectType.Fail && failIcon != null)
                    failIcon.SetActive(true);

                // เรียก Coroutine แสดง Choice Result
                StartCoroutine(HandleChoiceSelection(choice, btnGO));
            });
        }
    }

    IEnumerator HandleChoiceSelection(Choice choice, GameObject chosenButton)
    {
        // รอสั้น ๆ เพื่อให้ Icon โชว์ก่อน
        yield return new WaitForSeconds(0.5f);

        choicesPanel.SetActive(false);
        isWaitingForChoice = false;

        dialogText.text = choice.choiceResultText;

        CharacterSprites leftSprites = hasPowder ? leftCharacterSpritesWithPowder : leftCharacterSprites;
        CharacterSprites rightSprites = hasPowder ? rightCharacterSpritesWithPowder : rightCharacterSprites;

        if (choice.speaker == Speaker.Left)
        {
            leftCharacterImage.sprite = leftSprites.GetSprite(choice.emotion, false);
            rightCharacterImage.sprite = rightSprites.GetSprite(Emotion.Normal, true);
        }
        else
        {
            rightCharacterImage.sprite = rightSprites.GetSprite(choice.emotion, false);
            leftCharacterImage.sprite = leftSprites.GetSprite(Emotion.Normal, true);
        }

        pendingNextLineIndex = choice.nextLineIndex;

        // --- Dialog scoring integration ---
        string dialogKey = (GameManager.Instance != null) ? GameManager.Instance.GetCurrentDialogId() : null;
        int dialogScore = 0;
        switch (choice.effectType) // map to good/mid/bad
        {
            case ChoiceEffectType.Heart: dialogScore = 10; break;   // ดี
            case ChoiceEffectType.None: dialogScore = 5; break;     // กลาง
            case ChoiceEffectType.Fail: dialogScore = 0; break;     // แย่
        }
        if (GameManager.Instance != null && !string.IsNullOrEmpty(dialogKey))
        {
            GameManager.Instance.AddDialogScore(dialogKey, dialogScore);
        }

        isShowingChoiceResult = true;
        imageNextDialogue.enabled = true;
    }

    void NextLine()
    {
        if (currentLineIndex < 0 || currentLineIndex >= dialogueLine.Length) return;

        var line = dialogueLine[currentLineIndex];

        if (line.nextLineIndex >= 0 && line.nextLineIndex < dialogueLine.Length)
        {
            currentLineIndex = line.nextLineIndex;
        }
        else
        {
            currentLineIndex++;
        }

        ShowLine(currentLineIndex);
    }
    void EndDialog()
    {
        blurVolume.weight = 0f;

        foreach (SpriteRenderer sprite in mainCharacterSpriteRendererArr)
        {
            sprite.enabled = true;
        }
        foreach (GameObject npc in moveablenpcCharacterArr)
        {
            npc.SetActive(true);
        }

        playerController.TogglePlayerDialogue(false);
        dialogPanel.SetActive(false);
        if (dialogCanvas != null) dialogCanvas.gameObject.SetActive(false);

        onDialogEnd?.Invoke();
    }
}
