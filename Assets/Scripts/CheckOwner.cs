using Unity.Netcode;
using UnityEngine;

public class CheckOwner : NetworkBehaviour
{
    public bool checkForOwner()
    {
        return IsOwner;
    }
    
}
