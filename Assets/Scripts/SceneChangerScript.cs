using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerScript : MonoBehaviour {
    public void ChangeScene(int sceneN) {
        if (sceneN != SceneManager.GetActiveScene().buildIndex) {
            SceneManager.LoadScene(sceneN);
        }
    }
}
