using System;
using System.Collections;
using Lever.Models;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class NetworkArtifactSpawner : MonoBehaviour
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
                var artifact = PhotonNetwork.Instantiate(artifactPrefabs[artifactIndex].name, point.position,
                    Quaternion.identity);
                CatArtifact catArtifact;
                if(artifact.TryGetComponent(out catArtifact))
                    catArtifact.TeleportPoint = spawnPoints[0].position;
            }

            yield return new WaitForSeconds(spawnFrequency);
        }
    }
}