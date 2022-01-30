using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Lever.Networking
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private string catPrefabName;
        [SerializeField] private string mousePrefabName;
        [SerializeField] private List<Transform> spawnPoints;
        [SerializeField] private float safeRadius = 2f;

        private PhotonView photonView;
        private int connectedPlayers = 0;
        private const int TryToSpawn = 10;
        private GameObject player;
        private PlayerControlNetworking playerControlNetworking;
        private Player hunter;

        private void Start()
        {
            player = PhotonNetwork.Instantiate(catPrefabName, spawnPoints[0].position, spawnPoints[0].rotation);
            playerControlNetworking = player.GetComponent<PlayerControlNetworking>();
            playerControlNetworking.OnTransformToCat += ChangeHunter;
            playerControlNetworking.OnRespawn += ()=> RespawnPlayer(player.transform);
            
            photonView = PhotonView.Get(this);
            photonView.RPC("OnLevelLoaded", RpcTarget.MasterClient);
        }

        [PunRPC]
        private void OnLevelLoaded()
        {
            connectedPlayers++;
            if (connectedPlayers >= PhotonNetwork.PlayerList.Length)
                SpawnPlayers();
        }

        private void SpawnPlayers()
        {
            var playerList = PhotonNetwork.PlayerList;
            var usedPoints = new List<int>();
            for (int i = 0; i < playerList.Length; i++)
            {
                int index = -1;
                for (int j = 0; j < spawnPoints.Count; j++)
                {
                    var pointIndex = Random.Range(0, spawnPoints.Count);
                    if (usedPoints.Contains(pointIndex))
                        continue;
                    usedPoints.Add(pointIndex);
                    index = pointIndex;
                    break;
                }
                photonView.RPC("SpawnPlayer", playerList[i], index);
            }

            int whoIsCat = Random.Range(0, playerList.Length);
            hunter = playerList[whoIsCat];
            photonView.RPC("TransformToCat", hunter , false);
        }

        public void ChangeHunter()
        {
            photonView.RPC("ChangeHunterNetwork", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.NickName);
        }

        [PunRPC]
        private void ChangeHunterNetwork(string nickName)
        {
            photonView.RPC("TransformToMouse", hunter);
            hunter = PhotonNetwork.PlayerList.FirstOrDefault(x=>x.NickName == nickName);
        }

        [PunRPC]
        private void TransformToCat(bool withCallback)
        {
            playerControlNetworking.TransformToCat(withCallback);
        }
        
        [PunRPC]
        private void TransformToMouse()
        {
            playerControlNetworking.TransformToMouse();
        }

        [PunRPC]
        private void SpawnPlayer(int index)
        {
            player.transform.position = spawnPoints[index].position;
            player.transform.rotation = spawnPoints[index].rotation;
        }

        private void RespawnPlayer(Transform player)
        {
            for (int i = 0; i < TryToSpawn; i++)
            {
                int positionIndex = Random.Range(0, spawnPoints.Count);
                Collider[] colliders = Physics.OverlapSphere(spawnPoints[positionIndex].position, safeRadius);
                bool hasEnemy = false;
                foreach (var otherCollider in colliders)
                {
                    hasEnemy = otherCollider.TryGetComponent(out PlayerControl playerControl);
                    if (hasEnemy) // if is hunter needed
                        break;
                }

                if (hasEnemy && i != TryToSpawn - 1)
                    continue;

                player.position = spawnPoints[positionIndex].position;
                player.rotation = spawnPoints[positionIndex].rotation;
                break;
            }
        }
    }
}