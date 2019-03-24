using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

    [SerializeField]
    string hoverOverSound = "ButtonHover";

    [SerializeField]
    string pressButtonSound = "ButtonPress";

    AudioManager audioManager;

    void Start() {
        audioManager = AudioManager.instance;
        if (audioManager == null) {
            Debug.LogError("AudioManager: No AudioManager found in the scene.");
        }
    }

	public void Quit() {
        audioManager.PlaySound(pressButtonSound);
        Debug.Log("BYE");
        SceneManager.LoadScene("MainMenu");
    }

    public void Retry() {
        audioManager.PlaySound(pressButtonSound);
        SceneManager.LoadScene("MainLevel");
    }

    public void OnMouseOver() {
        audioManager.PlaySound(hoverOverSound);
    }
}
