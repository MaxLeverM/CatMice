using Photon.Realtime;

namespace Lever.UI.Windows.Interfaces
{
    public interface IUIManager
    {
        public void OpenLogin();

        public void OpenRoomBrowser(string playerName);

        public void UnsubscribeOnCloseRoomBrowser();

        public void CreateRoom(string name, int maxPlayers = 4);

        public void OpenLobby(RoomInfo roomInfo);

        public void UnsubscribeOnCloseLobby();

        public void OpenLevelLoadScreen();

        public void OpenLoadingScreen(bool shoudOpen);
    }
}