using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
    public GameObject explosion;
    public GameObject playerExplosion;
    public int scoreValue;

    // Tag this object should pass through instead of destroying.
    // Set to "Enemy" on the enemy bolt so it ignores the enemy that fired it
    // (and other enemies) while still hitting the Player. Leave empty on the
    // player bolt so it can still destroy asteroids and enemies.
    public string ignoreTag = "";

    private GameController gameController;

    private void Start()
    {
        GameObject gameControllerObject =
            GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find GameController script");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary"))
        {
            return;
        }
        // Pass through the configured tag (e.g. enemy bolt ignores "Enemy")
        // so enemies don't destroy themselves or each other.
        if (ignoreTag != "" && other.CompareTag(ignoreTag))
        {
            return;
        }
        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
        if (other.CompareTag("Player"))
        {
            if (playerExplosion != null)
            {
                Instantiate(playerExplosion, other.transform.position,
                    other.transform.rotation);
            }
            if (gameController != null)
            {
                gameController.GameOver();
            }
        }
        else
        {
            if (gameController != null)
            {
                gameController.AddScore(scoreValue);
            }
        }
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
