using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class CameraAutoFollow : MonoBehaviour
{
    private CinemachineCamera vcam;

    void Start()
    {
        vcam = GetComponent<CinemachineCamera>();
        StartCoroutine(WaitForPlayer());
    }

    private IEnumerator WaitForPlayer()
    {
        GameObject player = null;

        // รอจนกว่าจะเจอ Player
        while ((player = GameObject.FindWithTag("Player")) == null)
            yield return null;

        vcam.Follow = player.transform;
        vcam.LookAt = player.transform; // ถ้าต้องการให้กล้องหันตาม
    }
}
