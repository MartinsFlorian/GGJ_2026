using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_WaveManager : MonoBehaviour
{
    [Header("Settings")]
    private float speedMultiplier = 1f;
    [SerializeField] private float amountMultiplier = 0.1f;
    [SerializeField] private List<S_ClassPattern> listPattern;

    [Header("References")]
    [SerializeField] private RSO_ProjectileSpeed projectileSpeed;

    //[Header("Inputs")]

    //[Header("Outputs")]
    private Coroutine spawnWaveCoroutine;
    private void Start()
    {
        if (spawnWaveCoroutine != null)
        {
            StopCoroutine(spawnWaveCoroutine);
            spawnWaveCoroutine = null;
        }
        spawnWaveCoroutine = StartCoroutine(SpawnWave());
    }
    private IEnumerator SpawnWave()
    {
        foreach (var pattern in listPattern)
        {
            yield return new WaitForSeconds(pattern.waitTime * speedMultiplier);
            foreach (var spawner in pattern.listSpawners)
            {
                for (int i = 0; i < spawner.numProjectiles; i++)
                {
                    Instantiate(spawner.projectilePrefab, spawner.spawner.transform.position, spawner.spawner.transform.rotation);
                    yield return new WaitForSeconds(spawner.spawnInterval * speedMultiplier);
                }
            }
        }
        speedMultiplier += amountMultiplier;
        projectileSpeed.Value*= speedMultiplier;
        if (spawnWaveCoroutine != null)
        {
            StopCoroutine(spawnWaveCoroutine);
            spawnWaveCoroutine = null;
        }
        spawnWaveCoroutine = StartCoroutine(SpawnWave());
    }
}