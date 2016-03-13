using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DominoSync : NetworkBehaviour
{
    [SyncVar]
    [HideInInspector]
    public Quaternion serverRotation;

    void Start()
    {
        transform.rotation = serverRotation;
    }
}
