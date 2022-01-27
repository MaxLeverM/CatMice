using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArtifactSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] artifactPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnFrequency = 10;

    private void Start()
    {
        StartCoroutine(SpawnArtifacts());
    }

    private IEnumerator SpawnArtifacts()
    {
        while (true)
        {
            foreach (var point in spawnPoints)
            {
                var artifactIndex = Random.Range(0, artifactPrefabs.Length);
                Instantiate(artifactPrefabs[artifactIndex], point.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(spawnFrequency);
        }
    }
}
