using UnityEngine;
using System.Collections;
using TMPro;

public class EnemyWaveManager : MonoBehaviour
{
    [Header("Prefabs & Spawn Points")]
    public GameObject enemyPrefab;    // Kéo prefab Enemy
    public GameObject bossPrefab;     // Kéo prefab Boss
    public Transform[] spawnPoints;   // Kéo các spawn point

    [Header("Wave Settings")]
    public int maxWaves = 50;
    public float spawnInterval = 1f;

    [Header("Base Stats")]
    public float baseHP = 10f;
    public float baseDamage = 2f;
    public int baseExp = 5;

    [Header("UI References")]
    public TMP_Text waveText;   // Kéo TextMeshPro - Wave vào
    public TMP_Text killsText;  // Kéo TextMeshPro - Kills vào
    public TMP_Text typeText;   // Kéo TextMeshPro - Type vào

    // ----- Internal State -----
    private int currentWave = 0;
    private int killsThisWave = 0;
    private int kills = 0;                       // Tổng số enemy đã defeat
    private int requiredKillsThisWave => 4;      // 3 normal + 1 boss

    private float currentHP;
    private float currentDamage;
    private int currentExp;
    public int GetCurrentWave() => currentWave;
    public int GetTotalKills() => kills;

    public void SetWaveState(int wave, int totalKills)
    {
        currentWave = wave;
        kills = totalKills;
        killsThisWave = 0;
        StartNextWave();
    }
    void Awake()
    {
        var d = SaveLoadManager.pendingLoadedData;
        if (d != null)
        {
            SetWaveState(d.currentWave, d.totalKills);
            SaveLoadManager.pendingLoadedData = null;
            Debug.Log("[EnemyWaveManager] Đã load lại state từ SaveData!");
        }
    }

    void Start()
    {
        // Subscribe vào event của Enemy_Health
        Enemy_Health.OnMonsterDefeated += OnEnemyDefeated;

        // Khởi tạo stats
        currentHP = baseHP;
        currentDamage = baseDamage;
        currentExp = baseExp;

        // Bắt đầu wave đầu
        StartNextWave();
    }

    void OnDestroy()
    {
        Enemy_Health.OnMonsterDefeated -= OnEnemyDefeated;
    }

    void StartNextWave()
    {
        if (currentWave >= maxWaves)
        {
            Debug.Log("Hoàn thành tất cả wave!");
            UpdateUI(isBoss: false);
            return;
        }

        currentWave++;
        killsThisWave = 0;

        // Cập nhật UI cho wave mới (chưa có boss)
        UpdateUI(isBoss: false);

        // Bắt đầu coroutine spawn
        StartCoroutine(SpawnWaveSequence());
    }

    IEnumerator SpawnWaveSequence()
    {
        // 1) Spawn 3 Enemy
        for (int i = 0; i < 3; i++)
        {
            Spawn(enemyPrefab, scaleFactor: 1f);
            yield return new WaitForSeconds(spawnInterval);
        }

        // 2) Đợi kill xong 3 Enemy
        yield return new WaitUntil(() => killsThisWave >= 3);

        // 3) Spawn Boss (scale 1.5×)
        Spawn(bossPrefab, scaleFactor: 1.5f);
        UpdateUI(isBoss: true);
    }

    void Spawn(GameObject prefab, float scaleFactor)
    {
        Vector3 pos = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        GameObject go = Instantiate(prefab, pos, Quaternion.identity);
        go.transform.localScale *= scaleFactor;

        // Thiết lập stats cho Enemy_Health
        var eh = go.GetComponent<Enemy_Health>();
        eh.maxHealth = Mathf.RoundToInt(currentHP * scaleFactor);
        eh.currentHealth = eh.maxHealth;
        eh.expReward = Mathf.RoundToInt(currentExp * scaleFactor);

        // Thiết lập stats cho Enemy_Combat
        var ec = go.GetComponent<Enemy_Combat>();
        ec.damage = Mathf.RoundToInt(currentDamage * scaleFactor);
    }

    void OnEnemyDefeated(int expReward)
    {
        // Mỗi khi enemy chết
        killsThisWave++;
        kills++;   // Tăng tổng kills

        // Xác định đã spawn boss chưa (kill thứ 4 là boss)
        bool isBoss = killsThisWave > 3;
        UpdateUI(isBoss);

        // Nếu đã clear xong wave (3 + 1 boss) thì tăng stats và wave tiếp
        if (killsThisWave >= requiredKillsThisWave)
        {
            currentHP *= 1.1f;
            currentDamage *= 1.1f;
            currentExp = Mathf.RoundToInt(currentExp * 1.5f);

            StartCoroutine(NextWaveDelay());
        }
    }

    IEnumerator NextWaveDelay()
    {
        yield return new WaitForSeconds(2f);
        StartNextWave();
    }

    void UpdateUI(bool isBoss)
    {
        if (waveText != null)
            waveText.text = $"Wave: {currentWave}/{maxWaves}";

        if (killsText != null)
            killsText.text = $"Kills: {kills}";

        if (typeText != null)
            typeText.text = isBoss
                ? "<color=red>Type: Boss</color>"
                : "Type: Normal";
    }
}
