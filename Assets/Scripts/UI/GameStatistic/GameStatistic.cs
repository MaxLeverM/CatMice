using System;
using System.Collections.Generic;
using Lever.Networking;
using UnityEngine;
using Zenject;

namespace Lever.UI.GameStatistic
{
    public class GameStatistic : MonoBehaviour
    {
        [SerializeField] private Transform content;
        [SerializeField] private StatisticObject prefab;
        private StatisticManager statisticManager;
        private List<StatisticObject> createdObjects;
        
        [Inject]
        private void Construct(StatisticManager statisticManager)
        {
            this.statisticManager = statisticManager;
            createdObjects = new List<StatisticObject>();
            statisticManager.OnLeaderBoardCreated += InitLeaderboard;
            statisticManager.OnLeaderBoardUpdated += UpdateLeaderBoard;
        }

        private void UpdateLeaderBoard()
        {
            foreach (var createdObject in createdObjects)
            {
                var score = statisticManager.LeaderBoard[createdObject.NickNmae];
                createdObject.Write(score);
            }
        }

        private void InitLeaderboard()
        {
            foreach (var statistic in statisticManager.LeaderBoard)
            {
                var created = Instantiate(prefab, content);
                createdObjects.Add(created);
                created.Init(statistic.Key, statistic.Value);
            }
        }
        
        
    }
}