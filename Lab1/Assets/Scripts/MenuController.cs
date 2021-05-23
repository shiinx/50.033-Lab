using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Menu start button event
    public void StartButtonClicked() {
        SceneManager.LoadScene("Level1");
    }
    
}
