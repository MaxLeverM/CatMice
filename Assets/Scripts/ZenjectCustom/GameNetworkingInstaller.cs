using Lever.Networking;
using UnityEngine;
using Zenject;

namespace Lever.ZenjectCustom
{
    public class GameNetworkingInstaller : MonoInstaller
    {
        [SerializeField] private PlayerSpawner playerSpawner;
        public override void InstallBindings()
        {
            Container.Bind<GameNetworking>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
            Container.Bind<PlayerSpawner>().FromInstance(playerSpawner).AsSingle().NonLazy();
        }
    }
}