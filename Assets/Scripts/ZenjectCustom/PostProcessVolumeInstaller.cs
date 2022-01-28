using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

public class PostProcessVolumeInstaller : MonoInstaller
{
    [SerializeField] private Volume postProcessVolume;
    
    public override void InstallBindings()
    {
        Container
            .Bind<Volume>()
            .FromInstance(postProcessVolume)
            .AsSingle();
    }
}
