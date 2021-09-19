using System;
using UnityEngine;

public class Domination_Stats : MonoBehaviour
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
public class DominationButtonTaps
{
    public int ButtonTapsBlue = 0;
    public int ButtonTapsRed = 0;
    public float Time = 0.0f;

    public DominationButtonTaps(float _time, int _BTR, int _BTB) //, string _hk, string _kb)
    {
        Time = _time;
        ButtonTapsRed = _BTR;
        ButtonTapsBlue = _BTB;
    }
}
