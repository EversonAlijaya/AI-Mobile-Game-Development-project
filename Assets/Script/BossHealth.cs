using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 25;
    public int scoreValue = 200;
    public GameObject explosion;
    public Slider healthBar;

    private int currentHealth;
    private GameController gameController;

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        if (gameController != null)
        {
            gameController.AddScore(scoreValue);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy"))
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            // Boss destroys the player on contact, same as a regular enemy would
            if (gameController != null)
            {
                gameController.GameOver();
            }
            return;
        }

        // Anything else hitting the boss is treated as a player bullet
        TakeDamage(1);
        Destroy(other.gameObject);
    }
}