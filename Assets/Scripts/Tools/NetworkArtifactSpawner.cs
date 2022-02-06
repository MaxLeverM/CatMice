using System.Collections;
using Lever.Models;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkArtifactSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] artifactPrefabs;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] teleportPoints;
    [SerializeField] private float spawnFrequency = 10;
    
    private void Start()
    {
        if(PhotonNetwork.IsMasterClient)
            StartCoroutine(SpawnArtifacts());
    }

    private IEnumerator SpawnArtifacts()
    {
        while (true)
        {
            foreach (var point in spawnPoints)
            {
                var artifactIndex = Random.Range(0, artifactPrefabs.Length);
                var data = GetInstantiationData();
                PhotonNetwork.Instantiate(artifactPrefabs[artifactIndex].name, point.position,
                    Quaternion.identity, 0, data);
            }

            yield return new WaitForSeconds(spawnFrequency);
        }
    }

    private object[] GetInstantiationData()
    {
        var teleportIndex = Random.Range(0, teleportPoints.Length);
        var point = teleportPoints[teleportIndex].position;
        object[] data = new[] {(object)point};
        return data;
    }
}