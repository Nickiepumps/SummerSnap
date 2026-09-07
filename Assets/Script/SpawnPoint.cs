using UnityEngine;
using Unity.Cinemachine;

public class SpawnPoint : MonoBehaviour
{
    public string spawnPointName;
    [SerializeField] private CamSetup camSetup;
    void Start()
    {
        StartCoroutine(SetCharacterPositionNextFrame());
    }

    private System.Collections.IEnumerator SetCharacterPositionNextFrame()
    {
        yield return null; // รอ 1 frame

        GameObject player = GameManager.Instance != null ? GameManager.Instance.GetPlayer() : null;
        GameObject ryu = GameManager.Instance != null ? GameManager.Instance.GetRyu() : null;
        if (player == null || ryu == null) yield break;

        // ถ้ามีตำแหน่งที่ Save ไว้ → ใช้อันนั้นแทน SpawnPoint
        if (PlayerPositionManager.HasSavedPosition())
        {
            player.transform.position = PlayerPositionManager.RestorePlayerPosition();

            camSetup.Initialize(player.transform);

            //var cam = Object.FindFirstObjectByType<CinemachineCamera>();
            //if (cam != null) cam.Follow = player.transform;

            // วาง Ryu ใกล้ Player
            ryu.transform.position = player.transform.position + new Vector3(-2f, 0, 0);

            yield break;
        }

        // ถ้าไม่มีตำแหน่ง Save → ใช้ spawnPointName ตามปกติ
        if (GameManager.Instance.playerData.lastSpawnPoint == spawnPointName)
        {
            player.transform.position = transform.position;

            camSetup.Initialize(player.transform);

            //var cam = Object.FindFirstObjectByType<CinemachineCamera>();
            //if (cam != null) cam.Follow = player.transform;

            // วาง Ryu ใกล้ Player
            ryu.transform.position = transform.position + new Vector3(-2f, 0, 0);
        }
        else
        {
            camSetup.Initialize(player.transform);
        }
    }
}
