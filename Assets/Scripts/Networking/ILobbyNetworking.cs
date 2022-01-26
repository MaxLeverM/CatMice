using System;
using System.Collections.Generic;
using Lever.Constants;
using Photon.Pun;
using Photon.Realtime;

namespace Lever.Networking
{
    public interface ILobbyNetworking
    {
        public event Action<List<RoomInfo>> OnRoomListChanged;
        public event Action<Player[]> OnPlayerListChanged;
        public event Action OnJoinedToRoom;
        public event Action<Player> OnMasterChanged;

        public List<RoomInfo> Rooms
        {
            get;
            set;
        }

        public Player[] PlayersInRoom { get; }
        public int Ping { get; }
        public bool IsMasterClient { get; }

        public string NickName
        {
            get;
            set;
        }

        public void CreateRoom(string name, byte maxPlayers = 4);
        public void JoinRoom(string roomName);
        public void LeaveRoom();
        public void StartGame();
    }
}