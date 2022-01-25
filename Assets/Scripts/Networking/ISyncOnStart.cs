namespace Lever.Networking
{
    public interface ISyncOnStart
    {
        public void RaiseSync(int viewID);
        public void OnSyncCallback(object[] customData);
    }
}