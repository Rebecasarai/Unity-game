using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class Player : MonoBehaviour {

    public int fallBoundary = -20;

    public string deathSound = "DeathVoice";
    public string damageSound = "Grunt";
    public string spawnSound = "Spawn";

    private AudioManager audioManager;

    [SerializeField]
    private StatusIndicator statusIndicator;

    private PlayerStats stats;

    void Start() {
        stats = PlayerStats.instance;
        stats.curHealth = stats.maxHealth;

        if (statusIndicator == null) {
            Debug.LogError("StatusIndicator: No status indicator object referenced");
        }
        else {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }

        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

        audioManager = AudioManager.instance;
        if (audioManager == null) {
            Debug.LogError("AudioManager: No AudioManager found in the scene.");
        }
        else {
            audioManager.PlaySound(spawnSound);
        }

        InvokeRepeating("RegenHealth", stats.healthRegenRate, stats.healthRegenRate);
    }

    void RegenHealth() {
        stats.curHealth += 1;
        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }

    void Update() {
        if (transform.position.y <= fallBoundary)
            DamagePlayer(999999);
    }

    void OnUpgradeMenuToggle(bool active) {
        GetComponent<Platformer2DUserControl>().enabled = !active;
        Weapon _weapon = GetComponentInChildren<Weapon>();
        if (_weapon != null) {
            _weapon.enabled = !active;
        }
    }

    void OnDestroy() {
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
    }

    public void DamagePlayer(int damage) {
        stats.curHealth -= damage;
        if (stats.curHealth <= 0) {
            audioManager.PlaySound(deathSound);
            GameMaster.KillPlayer(this);
        }
        else {
            audioManager.PlaySound(damageSound);
        }

        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }
}
