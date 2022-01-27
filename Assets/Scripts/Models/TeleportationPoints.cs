using UnityEngine;

public class TeleportationPoints : MonoBehaviour, ITeleportationPoints
{
    [SerializeField] private Transform[] teleportPoints;
    
    public Transform[] TeleportPoints => teleportPoints;
    
    public Transform[] GetPoints()
    {
        return teleportPoints;
    }
}
