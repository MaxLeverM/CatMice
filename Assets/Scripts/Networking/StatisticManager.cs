﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lever.Networking
{
    public class StatisticManager : MonoBehaviour
    {
        [SerializeField] private List<PlayerControlNetworking> playerControlNetworkings;
        private Dictionary<string, int> leaderBoard;

        public Dictionary<string, int> LeaderBoard => leaderBoard;
        public event Action OnLeaderBoardUpdated;
        public event Action OnLeaderBoardCreated;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(3f);
            leaderBoard = new Dictionary<string, int>();
            playerControlNetworkings = FindObjectsOfType<PlayerControlNetworking>().ToList();
            foreach (var playerControlNetworking in playerControlNetworkings)
            {
                leaderBoard.Add(playerControlNetworking.GetPhotonView.Owner.NickName, 0);
                playerControlNetworking.OnKillVictim += KillVictim;
            }
            OnLeaderBoardCreated?.Invoke();
        }

        private void KillVictim(string nickName)
        {
            leaderBoard[nickName]++;
            OnLeaderBoardUpdated?.Invoke();
            Debug.Log($"{nickName}: {leaderBoard[nickName]}");
        }
    }
}