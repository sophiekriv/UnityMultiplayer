using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinServer : MonoBehaviour 
{
    // join server as host
    public void StartHost() {
        NetworkManager.Singleton.StartHost();
    }

    // join server as client
    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
