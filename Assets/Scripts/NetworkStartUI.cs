using UnityEngine;
using Unity.Netcode;

public class NetworkStartUI : MonoBehaviour
{
  

    public void startHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void startClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
