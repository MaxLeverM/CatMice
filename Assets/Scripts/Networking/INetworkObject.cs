namespace Lever.Networking
{
    public interface INetworkObject
    {
        public string ID { get; set; }

        public void OnNetworkEventCallback(object[] customData);
    }
}