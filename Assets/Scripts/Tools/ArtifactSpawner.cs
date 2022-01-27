using System.Collections;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class ArtifactSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] artifactPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnFrequency = 10;

    private DiContainer diContainer;

    [Inject]
    private void Construct(DiContainer diContainer)
    {
        this.diContainer = diContainer;
    }
    
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
                //Instantiate(artifactPrefabs[artifactIndex], point.position, Quaternion.identity);
                diContainer.InstantiatePrefab(artifactPrefabs[artifactIndex], point.position, Quaternion.identity,
                    null);
            }

            yield return new WaitForSeconds(spawnFrequency);
        }
    }
}
