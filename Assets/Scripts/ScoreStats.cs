using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreStats : MonoBehaviour
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
public class ScoreTrackingStats
{
    public int Score = 0;
    public float Time = 0.0f;

    public ScoreTrackingStats(float _time, int _score) //, string _hk, string _kb)
    {
        Time = _time;
        Score = _score;
    }
}
