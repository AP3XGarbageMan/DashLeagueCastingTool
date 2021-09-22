using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KF_spawner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SocketServer.KillFeedEvent += StartKfSequence;
    }

    private void StartKfSequence(Root _data)
    {

    }    
}


