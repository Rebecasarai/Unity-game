using UnityEngine;
using UnityEngine.UI;

public class StatusIndicator : MonoBehaviour {

    [SerializeField]
    private RectTransform healthBar;
    [SerializeField]
    private Text healthText;

    void Start() {
        if (healthBar == null) {
            Debug.LogError("StatusIndicator: No health bar object referenced");
        }
        if (healthText == null) {
            Debug.LogError("StatusIndicator: No health text object referenced");
        }
    }

    public void SetHealth(int _cur, int _max) {
        float _value = (float)_cur / _max;

        healthBar.localScale = new Vector3(_value, healthBar.localScale.y, healthBar.localScale.z);
        healthText.text = _cur + "/" + _max + " HP";
    }
}
