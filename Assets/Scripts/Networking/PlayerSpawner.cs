using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Lever.Networking
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private string catPrefabName;
        [SerializeField] private string mousePrefabName;
        [SerializeField] private List<Transform> spawnPoints;
        private void Start()
        {
            PhotonNetwork.Instantiate(catPrefabName, spawnPoints[0].position, spawnPoints[0].rotation);
        }
    }
}