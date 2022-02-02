using System.Collections;
using Lever.Models;
using Photon.Pun;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class ArtifactSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] artifactPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] teleportPoints;
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
                var artifact = Instantiate(artifactPrefabs[artifactIndex], point.position, Quaternion.identity);
                CatArtifact catArtifact;
                if (artifact.TryGetComponent(out catArtifact))
                {
                    var teleportIndex = Random.Range(0, teleportPoints.Length);
                    catArtifact.TeleportPoint = teleportPoints[teleportIndex].position;
                }
            }

            yield return new WaitForSeconds(spawnFrequency);
        }
    }
}
