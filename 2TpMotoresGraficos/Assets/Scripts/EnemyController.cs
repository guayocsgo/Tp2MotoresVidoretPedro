using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 50;


    public void TakeDamage(int amount)
    {
        health -= amount;
        Debug.Log("Enemigo recibi� da�o. Salud restante: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemigo eliminado!");
        Destroy(gameObject);
    }
}
