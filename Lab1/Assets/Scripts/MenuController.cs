using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake() {
        Time.timeScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartButtonClicked() {
        foreach (Transform eachChild in transform) {
            if (eachChild.name != "Score") {
                Debug.Log("Child found. Name: " + eachChild.name);
                eachChild.gameObject.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
    }
}
