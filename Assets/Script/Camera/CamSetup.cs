using Unity.Cinemachine;
using UnityEngine;

public class CamSetup : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCam;
    [SerializeField] private CinemachineFollow cinemachineCamFollow;
    [SerializeField] private Camera cam;
    private Vector3 camOffset;
    public void Initialize(Transform player)
    {
        PhoneUIManager.Instance.InitializeVirtualCamera();
        cinemachineCam.enabled = false;
        Vector3 playerPos = player.transform.position;
        camOffset = new Vector3(playerPos.x, 0, cinemachineCamFollow.FollowOffset.z);
        cam.transform.position = camOffset;
        cinemachineCam.enabled = true;
    }
}
