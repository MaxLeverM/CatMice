using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace Lever.Networking.Events
{
    public class NetworkEventController : MonoBehaviour, IOnEventCallback
    {
        private NetworkObjectsBehaviour _networkObjectsBehaviour;
        
        [Inject]
        private void Construct(NetworkObjectsBehaviour networkObjectsBehaviour)
        {
            this._networkObjectsBehaviour = networkObjectsBehaviour;
        }
        
        private void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        private void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnEvent(EventData photonEvent)
        {
            var eventCode = photonEvent.Code;

            var customData = photonEvent.CustomData as object[];

            var networkObjects = _networkObjectsBehaviour.NetworkObjects;

            switch (eventCode)
            {
                case (byte)NetworkEventCode.OnPlayerSpawned:
                    OnPlayerSpawned(customData);
                    break;
                // It is only just for example
               /* case (byte)NetworkEventCode.HeadSync:
                    HeadSync(customData);
                    break;
                case (byte)NetworkEventCode.GrabStart:
                    GetObject<GrabbableNetworkObject>(networkObjects, photonEvent, x => x.GrabStart(customData));
                    break;
                case (byte)NetworkEventCode.ButtonOnClick:
                case (byte)NetworkEventCode.InteractWithChair:
                case (byte)NetworkEventCode.PaintSync:
                case (byte)NetworkEventCode.StandSync:
                case (byte)NetworkEventCode.SetupQuiz:
                case (byte)NetworkEventCode.TooltipSync:
                    GetObject<INetworkObject>(networkObjects, photonEvent,x=>x.OnNetworkEventCallback(customData));
                    break;
                case (byte)NetworkEventCode.QuizSyncOnStart:
                case (byte)NetworkEventCode.PaintSyncOnStart:
                case (byte)NetworkEventCode.GrabbableObjectSyncOnStart:
                case (byte)NetworkEventCode.NetworkButtonsSyncOnStart:
                case (byte)NetworkEventCode.TVSyncOnStart:
                    GetObject<ISyncOnStart>(networkObjects, photonEvent,x=>x.OnSyncCallback(customData));
                    break; */
            }
        }

        private static void GetObject<T>(Dictionary<string, INetworkObject> networkObjects, EventData photonEvent, Action<T> onFind) where T : class
        {
            if (networkObjects.TryGetValue((string)((object[])photonEvent.CustomData)[0], out var controller))
                if (controller is T unpack)
                {
                    onFind?.Invoke(unpack);
                    return;
                }
            Debug.LogError($"Can't find object with id {(string)((object[])photonEvent.CustomData)[0]} . Event code {photonEvent.Code}");
        }

        private void OnPlayerSpawned(object[] customData)
        {
            var id = (int)customData[0];
            _networkObjectsBehaviour.SynchronizeScene(id);
        }
        
        //IT is only just for example
        /*
        private void HandsSync(object[] customData)
        {
            var photonID = (int)customData[0];
            var rotationL = (Quaternion)customData[1];
            var rotationR = (Quaternion)customData[2];
            var positionL = (Vector3)customData[3];
            var positionR = (Vector3)customData[4];
            var rootPlayer = PhotonNetwork.GetPhotonView(photonID);

            if (rootPlayer != null)
            {
                rootPlayer.GetComponent<AvatarIK>().NetworkSyncHands(rotationL, rotationR, positionL, positionR);
            }
        }*/
    }
}