using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour {

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text speedText;

    [SerializeField]
    private int healthUpgradeAmount = 25;

    [SerializeField]
    private int speedUpgradeAmount = 5;

    [SerializeField]
    private int upgradeCost = 10;

    private PlayerStats stats;

    void OnEnable() {
        stats = PlayerStats.instance;
        UpdateValues();
    }

    void UpdateValues() {
        healthText.text = "HEALTH: " + stats.maxHealth.ToString();
        speedText.text = "SPEED: " + stats.movementSpeed.ToString();
    }

    public void UpgradeHealth() {
        if (GameMaster.Money >= upgradeCost) {
            stats.maxHealth += healthUpgradeAmount;
            GameMaster.Money -= upgradeCost;
            AudioManager.instance.PlaySound("Money");
            UpdateValues();
        }
        else {
            AudioManager.instance.PlaySound("NoMoney");
        }
    }

    public void UpgradeSpeed() {
        if (GameMaster.Money >= upgradeCost) {
            stats.movementSpeed += speedUpgradeAmount;
            GameMaster.Money -= upgradeCost;
            AudioManager.instance.PlaySound("Money");
            UpdateValues();
        }
        else {
            AudioManager.instance.PlaySound("NoMoney");
        }
    }
}
