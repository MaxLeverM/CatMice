namespace Lever.UI.Windows.Interfaces
{
    public interface IUIManager
    {
        public void OpenLogin();

        public void OpenRoomBrowser(string playerName);

        public void OpenLobby();

        public void OpenLevelLoadScreen();
    }
}