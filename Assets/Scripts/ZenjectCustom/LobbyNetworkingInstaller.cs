using Lever.Networking;
using UnityEngine;
using Zenject;

namespace Lever.ZenjectCustom
{
    public class LobbyNetworkingInstaller : MonoInstaller
    {
        [SerializeField] private LobbyNetworking lobbyNetworking;
        public override void InstallBindings()
        {
            Container.Bind<ILobbyNetworking>().FromInstance(lobbyNetworking).AsSingle().NonLazy();
        }
    }
}