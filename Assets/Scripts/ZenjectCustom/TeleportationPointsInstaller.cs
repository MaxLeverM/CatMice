using UnityEngine;
using Zenject;

public class TeleportationPointsInstaller : MonoInstaller
{
    [SerializeField] private TeleportationPoints teleportationPoints;
    
    public override void InstallBindings()
    {
        Container.Bind<TeleportationPoints>().FromInstance(teleportationPoints).AsSingle();
    }
}
