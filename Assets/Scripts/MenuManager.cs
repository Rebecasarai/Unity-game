using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class MenuManager : MonoBehaviour {

    [SerializeField]
    string hoverOverSound = "ButtonHover";

    [SerializeField]
    string pressButtonSound = "ButtonPress";

    AudioManager audioManager;

    void Start () {
        audioManager = AudioManager.instance;
        if (audioManager == null) {
            Debug.LogError("AudioManager: No AudioManager found in the scene.");
        }
        else {
            audioManager.StopSound("LevelMusic");
            audioManager.PlaySound("MenuMusic");
        }
    }

	public void StartGame () {
        audioManager.PlaySound(pressButtonSound);
        SceneManager.LoadScene("MainLevel");
	}

    public void QuitGame() {
        audioManager.PlaySound(pressButtonSound);
        Debug.Log("BYE");
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public void OnMouseOver () {
        audioManager.PlaySound(hoverOverSound);
    }
}
