using Lever.Networking;
using UnityEngine;
using Zenject;

namespace Lever.ZenjectCustom
{
    public class StatisticManagerInstaller : MonoInstaller
    {
        [SerializeField] private StatisticManager statisticManager;
        public override void InstallBindings()
        {
            Container.Bind<StatisticManager>().FromInstance(statisticManager).AsSingle().NonLazy();
        }
    }
}