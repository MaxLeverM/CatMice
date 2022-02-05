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

    private PhotonView photonView;
    
    private void Start()
    {
        photonView = PhotonView.Get(this);
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
                var artifact = PhotonNetwork.Instantiate(artifactPrefabs[artifactIndex].name, point.position,
                    Quaternion.identity);
                CatArtifact catArtifact;
                if (artifact.TryGetComponent(out catArtifact))
                {
                    var teleportIndex = Random.Range(0, teleportPoints.Length);
                    photonView.RPC(nameof(SetCatArtifactTeleportationPoint), RpcTarget.All, 
                        catArtifact, teleportPoints[teleportIndex].position);
                    catArtifact.id
                }
            }

            yield return new WaitForSeconds(spawnFrequency);
        }
    }

    [PunRPC]
    private void SetCatArtifactTeleportationPoint(CatArtifact artifact, Vector3 teleportationPosition)
    {
        artifact.TeleportPoint = teleportationPosition;
    }
}