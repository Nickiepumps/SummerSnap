using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    public static PersistentUI instance;
    [SerializeField] private PhoneUIManager phoneUIManager;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void TogglePhoneUI(bool state)
    {
        phoneUIManager.enabled = state;
    }
}
