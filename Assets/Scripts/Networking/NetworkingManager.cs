using Lever.Constants;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lever.Networking
{
    public class NetworkingManager : MonoBehaviourPunCallbacks
    {
        public void Leave()
        {
            PhotonNetwork.LeaveRoom();
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(SceneNames.Menu);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"Player {newPlayer.NickName} entered room");
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log($"Player {otherPlayer.NickName} has left the room");
        }
    }
}