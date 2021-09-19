using System;
using UnityEngine;

public class Payload_Stats : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}



[Serializable]
public class PayloadPercentTracker
{
    public int bluePayloadPercent = 0;
    public int redPayloadPercent = 0;
    public float Time = 0.0f;

    public PayloadPercentTracker(float _time, int _RPP, int _BPP) //, string _hk, string _kb)
    {
        Time = _time;
        redPayloadPercent = _RPP;
        bluePayloadPercent = _BPP;
    }
}