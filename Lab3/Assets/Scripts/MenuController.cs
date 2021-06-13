using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
    public void Level1ButtonClicked() {
        SceneManager.LoadScene("Level1");
    }

    public void Level2ButtonClicked() {
        SceneManager.LoadScene("Level2");
    }
}