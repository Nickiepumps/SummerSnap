using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject playerPrefab;
    public GameObject ryuPrefab;
    public Transform endingSpawnPoint;

    public PlayerData playerData;

    private GameObject player;
    private PlayerController playerController;
    private GameObject ryu;
    private bool currentDialogHasPowder = false;
    public bool isPhotoshooting { get; private set; } = false;

    [Header("Animator Controllers - Ryu")]
    public RuntimeAnimatorController ryuControllerNoPowder;
    public RuntimeAnimatorController ryuControllerWithPowder;

    // -------------------------
    // ✅ ระบบเก็บ Quest/Dialogs/Photo
    // -------------------------
    private HashSet<string> takenPhotos = new HashSet<string>();
    private HashSet<string> triggeredDialogs = new HashSet<string>();
    private HashSet<string> triggeredMainQuests = new HashSet<string>();


    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            playerData = new PlayerData();

            player = Instantiate(playerPrefab);
            playerController = player.GetComponent<PlayerController>();
            DontDestroyOnLoad(player);

            ryu = Instantiate(ryuPrefab);
            DontDestroyOnLoad(ryu);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var cam = Object.FindFirstObjectByType<CinemachineCamera>();
        if (cam != null && player != null)
        {
            cam.Follow = player.transform;
        }
    }

    // -------------------------
    // ✅ Player
    // -------------------------
    public GameObject GetPlayer() => player;
    public PlayerController GetPlayerController() => playerController;

    // -------------------------
    // ✅ Ryu
    // -------------------------
    public GameObject GetRyu() => ryu;

    public Animator GetRyuAnimator()
    {
        return ryu != null ? ryu.GetComponent<Animator>() : null;
    }

    public RuntimeAnimatorController GetRyuPowderController(bool hasPowder)
    {
        return hasPowder ? ryuControllerWithPowder : ryuControllerNoPowder;
    }

    // -------------------------
    // ✅ ระบบ Powder
    // -------------------------
    public void SetCurrentDialogPowdered(bool value) => currentDialogHasPowder = value;
    public bool IsCurrentDialogPowdered() => currentDialogHasPowder;

    // -------------------------
    // ✅ ระบบ SideQuest Photo
    // -------------------------
    public bool HasTaken(string questId) => takenPhotos.Contains(questId);
    public void MarkAsTaken(string questId)
    {
        if (!string.IsNullOrEmpty(questId))
            takenPhotos.Add(questId);
    }

    // -------------------------
    // ✅ ระบบ Dialog
    // -------------------------
    public bool HasTriggeredDialog(string dialogId) => triggeredDialogs.Contains(dialogId);
    public void MarkDialogTriggered(string dialogId)
    {
        if (!string.IsNullOrEmpty(dialogId))
            triggeredDialogs.Add(dialogId);
    }

    // -------------------------
    // ✅ ระบบ MainQuest Trigger
    // -------------------------
    public bool HasTriggeredMainQuest(string questId) => triggeredMainQuests.Contains(questId);
    public void MarkMainQuestTriggered(string questId)
    {
        if (!string.IsNullOrEmpty(questId))
            triggeredMainQuests.Add(questId);
    }

    // -------------------------
    // ✅ Scoring (MainQuest + Dialog)
    // -------------------------
    [Header("MainQuest Scores")]
    public int scoreLevel1;
    public int scoreLevel2;
    public int scoreLevel3;
    public int scoreLevel4;

    [Header("Dialog Scores")]
    public int scoreDekpang;
    public int scoreDog;
    public int scoreInTemple;

    private string currentDialogId = null;

    public void SetCurrentDialogId(string id) { currentDialogId = id; }
    public string GetCurrentDialogId() { return currentDialogId; }

    // ✅ เพิ่มกลับมาแล้ว
    public void AddMainQuestScore(int level, int score)
    {
        switch (level)
        {
            case 1: scoreLevel1 = Mathf.Max(scoreLevel1, score); break;
            case 2: scoreLevel2 = Mathf.Max(scoreLevel2, score); break;
            case 3: scoreLevel3 = Mathf.Max(scoreLevel3, score); break;
            case 4: scoreLevel4 = Mathf.Max(scoreLevel4, score); break;
        }
    }

    public int GetMainQuestScore(int level)
    {
        switch (level)
        {
            case 1: return scoreLevel1;
            case 2: return scoreLevel2;
            case 3: return scoreLevel3;
            case 4: return scoreLevel4;
            default: return 0;
        }
    }

    public void AddDialogScore(string dialogKey, int score)
    {
        // Expect dialogKey to be "Dekpang", "Dog", "InTemple"
        if (string.IsNullOrEmpty(dialogKey)) return;
        switch (dialogKey)
        {
            case "Dekpang": scoreDekpang = Mathf.Max(scoreDekpang, score); break;
            case "Dog": scoreDog = Mathf.Max(scoreDog, score); break;
            case "InTemple": scoreInTemple = Mathf.Max(scoreInTemple, score); break;
            default:
                // If custom other IDs exist, ignore or extend switch as needed
                break;
        }
    }

    public int GetTotalScore()
    {
        return scoreLevel1 + scoreLevel2 + scoreLevel3 + scoreLevel4
             + scoreDekpang + scoreDog + scoreInTemple;
    }

    // ✅ นับจำนวน Dialog ที่ดี (คะแนน >= threshold)
    public int CountGoodDialogs(int threshold = 20)
    {
        int count = 0;
        if (scoreDekpang >= threshold) count++;
        if (scoreDog >= threshold) count++;
        if (scoreInTemple >= threshold) count++;
        return count;
    }

    // -------------------------
    // ✅ Reset Game (วิธีที่ 1)
    // -------------------------
    public void ResetGame()
    {
        // รีเซ็ตข้อมูล PlayerData
        playerData = new PlayerData();

        // รีเซ็ต Quest/Photo/Dialog
        takenPhotos.Clear();
        triggeredDialogs.Clear();
        triggeredMainQuests.Clear();

        // รีเซ็ตคะแนน
        scoreLevel1 = scoreLevel2 = scoreLevel3 = scoreLevel4 = 0;
        scoreDekpang = scoreDog = scoreInTemple = 0;
        currentDialogId = null;
        currentDialogHasPowder = false;

        // ลบ player/ryu เก่า
        if (player != null) Destroy(player);
        if (ryu != null) Destroy(ryu);

        // สร้าง player/ryu ใหม่ รอบเดียว
        player = Instantiate(playerPrefab, playerData.lastPosition, Quaternion.identity);
        DontDestroyOnLoad(player);

        ryu = Instantiate(ryuPrefab, Vector3.zero, Quaternion.identity);
        DontDestroyOnLoad(ryu);
    }
    public void TogglePhotoshootStatus(bool status)
    {
        isPhotoshooting = status;
    }
}
