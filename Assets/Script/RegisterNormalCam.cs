using UnityEngine;
using Unity.Cinemachine;

public class RegisterNormalCam : MonoBehaviour
{
    void Start()
    {
        var cam = GetComponent<CinemachineCamera>();
        if (PhoneUIManager.Instance != null && cam != null)
        {
            PhoneUIManager.Instance.RegisterSceneCamera(cam);
        }
    }
}
