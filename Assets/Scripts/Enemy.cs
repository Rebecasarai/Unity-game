using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour {

    [System.Serializable]
    public class EnemyStats {
        public int maxHealth = 100;
        private int _curHealth;
        public int curHealth {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public int damage = 20;

        public void Init() {
            curHealth = maxHealth;
        }
    }

    public EnemyStats stats = new EnemyStats();

    public string deathSound = "Explosion";

    public Transform deathParticles;
    public float deathShakeAmount = 0.1f;
    public float deathShakeLength = 0.1f;

    public int killReward = 10;

    [Header("Optional: ")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    void Start() {
        stats.Init();
        if (statusIndicator != null) {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }

        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

        if (deathParticles == null) {
            Debug.LogError("No death particles referenced on Enemy");
        }
    }

    void OnUpgradeMenuToggle(bool active) {
        GetComponent<EnemyAI>().enabled = !active;
    }

    public void DamageEnemy(int damage) {
        stats.curHealth -= damage;
        if (stats.curHealth <= 0) {
            GameMaster.KillEnemy(this);
        }

        if (statusIndicator != null) {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }
    }

    void OnCollisionEnter2D(Collision2D _colInfo) {
        Player _player = _colInfo.collider.GetComponent<Player>();
        if (_player != null) {
            _player.DamagePlayer(stats.damage);
            DamageEnemy(999999);
        }
    }

    void OnDestroy() {
        GameMaster.Money += killReward;
        AudioManager.instance.PlaySound("Money");
        GameMaster.gm.onToggleUpgradeMenu -= OnUpgradeMenuToggle;
    }
}
