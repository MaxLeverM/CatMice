using Lever.Networking;
using Zenject;

namespace Lever.ZenjectCustom
{
    public class LobbyNetworkingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<LobbyNetworking>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }
    }
}