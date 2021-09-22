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

[Serializable]
public class DominationBUttonStats
{
    public string name = "";
    public int cd = 0;
    public int bt = 0;

    public DominationBUttonStats(string _name, int _bt, int _cd) //, string _hk, string _kb)
    {
        name = _name;
        bt = _bt;
        cd = _cd;
    }
}