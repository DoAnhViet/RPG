using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class EnemyWaveManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints; // Kéo 3 vị trí spawn vào đây trên inspector
    public float baseHP = 10f;
    public float baseDamage = 2f;
    public int baseExp = 5;

    private int enemiesToSpawn = 3; // 3 thường, 1 boss
    private int enemiesAlive = 0;
    private float currentHP;
    private float currentDamage;
    private int currentExp;

    private bool spawningBoss = false;

    void Start()
    {
        // Đăng ký nhận sự kiện enemy chết
        Enemy_Health.OnMonsterDefeated += OnEnemyDefeated;

        // Khởi tạo chỉ số ban đầu
        currentHP = baseHP;
        currentDamage = baseDamage;
        currentExp = baseExp;

        SpawnWave();
    }

    void OnDestroy()
    {
        Enemy_Health.OnMonsterDefeated -= OnEnemyDefeated;
    }

    void SpawnWave()
    {
        StartCoroutine(SpawnWaveCoroutine());
    }

    IEnumerator SpawnWaveCoroutine()
    {
        if (!spawningBoss)
        {
            for (int i = 0; i < 3; i++)
            {
                SpawnEnemy(spawnPoints[Random.Range(0, spawnPoints.Length)].position, currentHP, currentDamage, currentExp);
                enemiesAlive++;
                yield return new WaitForSeconds(3f); // Delay 3s cho mỗi enemy
            }
        }
        else
        {
            yield return new WaitForSeconds(3f); // Delay 3s trước khi spawn boss
            Vector3 bossSpawn = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            float bossHP = currentHP * 2.3f;
            float bossDamage = currentDamage * 1.9f;
            int bossExp = Mathf.RoundToInt(currentExp * 1.7f);

            SpawnEnemy(bossSpawn, bossHP, bossDamage, bossExp);
            enemiesAlive++;
        }
    }
    void SpawnEnemy(Vector3 pos, float hp, float dmg, int exp)
    {
        GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
        Enemy_Health eh = enemy.GetComponent<Enemy_Health>();
        eh.maxHealth = Mathf.RoundToInt(hp);
        eh.currentHealth = eh.maxHealth;
        eh.expReward = exp;

        Enemy_Combat ec = enemy.GetComponent<Enemy_Combat>();
        ec.damage = Mathf.RoundToInt(dmg);
    }

    void OnEnemyDefeated(int exp)
    {
        enemiesAlive--;
        if (enemiesAlive <= 0)
        {
            // Chuyển trạng thái
            if (!spawningBoss)
            {
                // Kết thúc 3 nhỏ → spawn boss
                spawningBoss = true;
            }
            else
            {
                // Kết thúc boss → tăng chỉ số, quay lại 3 nhỏ
                spawningBoss = false;
                currentHP *= 1.1f;
                currentDamage *= 1.1f;
                currentExp = Mathf.RoundToInt(currentExp * 1.5f);
            }

            SpawnWave();
        }
    }
}
