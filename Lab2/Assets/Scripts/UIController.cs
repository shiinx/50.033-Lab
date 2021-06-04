using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {
    // Start is called before the first frame update
    private void Start() {
        foreach (Transform eachChild in transform) {
            if (eachChild.name != "Score") {
                eachChild.gameObject.SetActive(false);
            }
        }
    }

    public void RestartButtonClicked() {
        SceneManager.LoadScene("Level1");
        Time.timeScale = 1.0f;
    }
}