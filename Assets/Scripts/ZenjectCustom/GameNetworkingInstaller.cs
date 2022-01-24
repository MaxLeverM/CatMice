using Lever.Networking;
using Zenject;

namespace Lever.ZenjectCustom
{
    public class GameNetworkingInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<NetworkingManager>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }
    }
}