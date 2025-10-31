
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public Transform player;
    public int enemiesToSpawn = 2; 
    public float spawnRadius = 10f; 
    public float minDistanceFromPlayer = 5f; 
    public float spawnHeight = 0.5f;

    public GameObject victoryPanel; 

    private int totalEnemiesAlive = 0;
    private bool firstEnemyKilled = false;

    private void Start()
    {
        
        totalEnemiesAlive = 1;

        
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    
    public void OnEnemyDeath(Vector3 deathPosition, bool canSpawn)
    {
        totalEnemiesAlive--;
        Debug.Log("Enemigo eliminado. Enemigos restantes: " + totalEnemiesAlive);

        
        if (!firstEnemyKilled && canSpawn)
        {
            firstEnemyKilled = true;
            SpawnNewEnemies();
        }

        
        Invoke("CheckVictory", 0.1f);
    }

    void SpawnNewEnemies()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("ERROR: enemyPrefab NO está asignado en el SpawnManager!");
            return;
        }

        if (player == null)
        {
            Debug.LogError("ERROR: player NO está asignado en el SpawnManager!");
            return;
        }

        Debug.Log("Spawneando " + enemiesToSpawn + " nuevos enemigos...");

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy(false); 
        }
    }

    void SpawnEnemy(bool canSpawn)
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();

        Debug.Log("Intentando spawnear enemigo en: " + spawnPosition);

       
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        if (newEnemy == null)
        {
            Debug.LogError("ERROR: No se pudo instanciar el enemigo!");
            return;
        }

        Debug.Log("Enemigo instanciado exitosamente: " + newEnemy.name);

       
        totalEnemiesAlive++;

        
        EnemyLife enemyLife = newEnemy.GetComponent<EnemyLife>();
        if (enemyLife != null)
        {
            enemyLife.spawnManager = this;
            enemyLife.canSpawnEnemies = canSpawn; 
            Debug.Log("EnemyLife configurado - CanSpawn: " + canSpawn);
        }
        else
        {
            Debug.LogWarning("El prefab del enemigo no tiene el componente EnemyLife!");
        }

        EnemyAi enemyAi = newEnemy.GetComponent<EnemyAi>();
        if (enemyAi != null)
        {
            enemyAi.player = player;
            Debug.Log("EnemyAi configurado con player: " + player.name);
        }
        else
        {
            Debug.LogWarning("El prefab del enemigo no tiene el componente EnemyAi!");
        }

        Debug.Log("✅ Enemigo completamente spawneado en: " + spawnPosition);
    }

    void CheckVictory()
    {
        Debug.Log("Verificando victoria. Enemigos vivos: " + totalEnemiesAlive);

        if (totalEnemiesAlive <= 0)
        {
            ShowVictory();
        }
    }

    void ShowVictory()
    {
        Debug.Log("🎉 ¡VICTORIA! Todos los enemigos han sido eliminados");

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Victory Panel no está asignado!");
        }

        
        Time.timeScale = 0f;
    }

    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPos = Vector3.zero;
        int attempts = 0;
        int maxAttempts = 30;

        
        while (attempts < maxAttempts)
        {
           
            Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minDistanceFromPlayer, spawnRadius);
            spawnPos = player.position + new Vector3(randomCircle.x, spawnHeight, randomCircle.y);

            
            float distanceToPlayer = Vector3.Distance(new Vector3(spawnPos.x, player.position.y, spawnPos.z), player.position);

            if (distanceToPlayer >= minDistanceFromPlayer)
            {
                return spawnPos;
            }

            attempts++;
        }

        
        return player.position + player.forward * minDistanceFromPlayer + player.right * Random.Range(-3f, 3f);
    }

  
    public void SpawnInitialEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnEnemy(true); 
        }
    }
}