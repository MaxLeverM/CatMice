using UnityEngine;

public class TeleportationPoints : MonoBehaviour
{
    [SerializeField] private Transform[] teleportPoints;
    
    public Transform[] TeleportPoints => teleportPoints;
}
