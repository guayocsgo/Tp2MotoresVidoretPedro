using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyDamagePlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("El enemigo toc� al jugador - GAME OVER");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
        }
    }
}
