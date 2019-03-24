using UnityEngine;
using System.Collections;

public class GameMaster : MonoBehaviour {

    public static GameMaster gm;

    [SerializeField]
    private int maxLives = 3;
    private static int _remainingLives;
    public static int RemainingLives {
        get { return _remainingLives; }
    }

    [SerializeField]
    private int startingMoney;
    public static int Money;

    void Awake() {
        if (gm == null) {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 3.7f;
    public Transform spawnParticles;
    public Transform deathParticles;
    public string respawnCountdownSound = "RespawnCountdown";

    public CameraShake cameraShake;

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject upgradeMenu;

    [SerializeField]
    private WaveSpawner waveSpawner;

    public delegate void UpgradeMenuCallback(bool active);
    public UpgradeMenuCallback onToggleUpgradeMenu;

    AudioManager audioManager;

    void Start() {
        _remainingLives = maxLives;
        Money = startingMoney;

        audioManager = AudioManager.instance;
        if (audioManager == null) {
            Debug.LogError("AudioManager: No AudioManager found in the scene.");
        }
        else {
            audioManager.StopSound("MenuMusic");
            audioManager.PlaySound("LevelMusic");
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.U)) {
            ToggleUpgradeMenu();
        }
    }

    private void ToggleUpgradeMenu() {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        waveSpawner.enabled = !upgradeMenu.activeSelf;
        onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);
    }

    public void EndGame() {
        Debug.Log("GAME OVER");
        audioManager.PlaySound("GameOver");
        gameOverUI.SetActive(true);
    }

    public IEnumerator _RespawnPlayer() {
        audioManager.PlaySound(respawnCountdownSound);
        yield return new WaitForSeconds(spawnDelay);

        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate(spawnParticles, spawnPoint.position, spawnPoint.rotation) as Transform;
        Destroy(clone, 3f);
    }

	public static void KillPlayer(Player player) {
        Transform clone = Instantiate(gm.deathParticles, player.transform.position, player.transform.rotation) as Transform;
        Destroy(player.gameObject);
        Destroy(clone, 3f);
        _remainingLives -= 1;
        if (_remainingLives <= 0) {
            gm.EndGame();
        }
        else {
            gm.StartCoroutine(gm._RespawnPlayer());
        }
    }

    public static void KillEnemy(Enemy enemy) {
        gm._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy _enemy) {
        audioManager.PlaySound(_enemy.deathSound);

        Transform _clone = Instantiate(_enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as Transform;
		Destroy(_clone, 5f);

        cameraShake.Shake(_enemy.deathShakeAmount, _enemy.deathShakeLength);
		Destroy(_enemy.gameObject);
	}
}
