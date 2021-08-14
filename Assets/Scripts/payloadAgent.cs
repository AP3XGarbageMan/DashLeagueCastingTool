using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class payloadAgent : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform self;
    [SerializeField]
    private Transform[] payloadPath;



    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(payloadPath[PayloadTrackPos.trackPosCounter].position);
        Debug.Log("track pos counter = " + PayloadTrackPos.trackPosCounter.ToString());
    }
}
