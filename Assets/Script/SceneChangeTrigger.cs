using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour
{
    public string sceneToLoad;
    public string destinationSpawnPointName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.playerData.lastSpawnPoint = destinationSpawnPointName;
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
