using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDamagePlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("El enemigo tocó al jugador - GAME OVER");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
        }
    }
}
