using UnityEngine;
using System.Collections;

public class BossWeaponController : MonoBehaviour
{
    public GameObject shot;
    public Transform shotSpawn;
    public int burstCount = 3;
    public float burstDelay = 0.15f;
    public float timeBetweenBursts = 2f;
    public float startDelay = 1f;

    private void Start()
    {
        StartCoroutine(BurstFireLoop());
    }

    private IEnumerator BurstFireLoop()
    {
        yield return new WaitForSeconds(startDelay);

        while (true)
        {
            for (int i = 0; i < burstCount; i++)
            {
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                GetComponent<AudioSource>().Play();
                yield return new WaitForSeconds(burstDelay);
            }

            yield return new WaitForSeconds(timeBetweenBursts);
        }
    }
}