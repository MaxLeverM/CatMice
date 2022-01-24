using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace Lever.Networking
{
    public class NetworkObjectsBehaviour : MonoBehaviour
    {
        private Dictionary<string, INetworkObject> networkObjects = new Dictionary<string, INetworkObject>();
        public Dictionary<string, INetworkObject> NetworkObjects => networkObjects;
        
        private void Awake()
        {
            FindObjects();
        }

        private void Start()
        {
            SynchronizeScene(0);
        }

        public void FindObjects()
        {
            var objectsInTheScene = FindObjectsOfType<MonoBehaviour>().OfType<INetworkObject>().ToList();

            InitNetworkObjectsDictionary(objectsInTheScene);
        }

        private void InitNetworkObjectsDictionary(List<INetworkObject> objects)
        {
            networkObjects = new Dictionary<string, INetworkObject>();

            foreach (var networkObject in objects)
            {
                networkObjects.Add(networkObject.ID, networkObject);
            }
        }

        public void SynchronizeScene(int viewID)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            StartCoroutine(SynchronizeSceneRoutine(viewID));
        }

        private IEnumerator SynchronizeSceneRoutine(int viewID)
        {
            var syncObjects = NetworkObjects.Values.OfType<ISyncOnStart>();
            
            yield return new WaitForEndOfFrame();
            
            foreach (var syncObject in syncObjects)
            {
                yield return new WaitForEndOfFrame();
                syncObject.RaiseSync(viewID);
                yield return new WaitForEndOfFrame();
            }
        }
    }
}