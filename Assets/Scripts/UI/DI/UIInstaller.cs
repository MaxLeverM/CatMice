using System.Collections;
using System.Collections.Generic;
using Lever.Networking;
using Lever.UI.Windows.Behaviours;
using UnityEngine;
using Zenject;

public class UIInstaller : MonoInstaller
{
    [SerializeField] private UIManager uiManager;
    [SerializeField] private LoadingScreenBehaviour loadingScreenBehaviour;
    
    public override void InstallBindings()
    {
        Container.Bind<UIManager>().FromInstance(uiManager).AsSingle();
        Container.Bind<LoadingScreenBehaviour>().FromInstance(loadingScreenBehaviour).AsSingle();
    }
}
