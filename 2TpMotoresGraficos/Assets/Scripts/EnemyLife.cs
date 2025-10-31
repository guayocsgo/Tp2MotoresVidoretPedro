
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyLife : MonoBehaviour
{
    public int vidaEnemigo = 100;
    public Slider BarraVidaEnemigo;
    public GameObject bloodEffectPrefab;

    //[HideInInspector]
    public EnemySpawnManager spawnManager;

    //[HideInInspector]
    public bool canSpawnEnemies = true;

    private bool isDead = false;

    private void Update()
    {
        if (BarraVidaEnemigo != null)
        {
            BarraVidaEnemigo.value = vidaEnemigo;
        }

        if (vidaEnemigo <= 0 && !isDead)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        vidaEnemigo -= damage;
        Debug.Log("Enemigo recibió daño. Vida restante: " + vidaEnemigo);

        if (bloodEffectPrefab != null)
        {
            Vector3 bloodPosition = transform.position + Vector3.up * 1f;
            GameObject blood = Instantiate(bloodEffectPrefab, bloodPosition, Quaternion.identity);

            ParticleSystem ps = blood.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
            }

            Destroy(blood, 2f);
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Enemigo eliminado! Iniciando animación de muerte...");

        if (spawnManager != null)
        {
            Debug.Log("SpawnManager encontrado. CanSpawn: " + canSpawnEnemies);
            spawnManager.OnEnemyDeath(transform.position, canSpawnEnemies);
        }
        else
        {
            Debug.LogWarning("SpawnManager NO está asignado!");
        }

        if (bloodEffectPrefab != null)
        {
            GameObject blood = Instantiate(bloodEffectPrefab, transform.position + Vector3.up * 1f, Quaternion.identity);

            ParticleSystem ps = blood.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var emission = ps.emission;
                emission.SetBurst(0, new ParticleSystem.Burst(0, 30));
            }

            Destroy(blood, 2f);
        }

        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        if (BarraVidaEnemigo != null)
        {
            BarraVidaEnemigo.gameObject.SetActive(false);
        }

        StartCoroutine(DeathAnimation());
    }

    IEnumerator DeathAnimation()
    {
        float duration = 1.5f;
        float elapsed = 0f;

        Vector3 originalScale = transform.localScale;
        Vector3 originalPosition = transform.position;
        Quaternion originalRotation = transform.rotation;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            transform.position = Vector3.Lerp(originalPosition, originalPosition + Vector3.down * 0.5f, t);
            transform.rotation = Quaternion.Lerp(originalRotation, originalRotation * Quaternion.Euler(90, 0, 0), t);
            transform.localScale = Vector3.Lerp(originalScale, originalScale * 0.5f, t);

            yield return null;
        }

        Destroy(gameObject, 1f);
    }
}