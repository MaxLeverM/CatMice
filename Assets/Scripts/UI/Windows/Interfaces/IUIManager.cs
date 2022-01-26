namespace Lever.UI.Windows.Interfaces
{
    public interface IUIManager
    {
        public void OpenLogin();

        public void OpenRoomBrowser(string playerName);

        public void OpenLobby(string roomName);

        public void OpenLevelLoadScreen();

        public void OpenLoadingScreen(bool shoudOpen);
    }
}