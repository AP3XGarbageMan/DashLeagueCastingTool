using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HDPlayer : MonoBehaviour
{
    public Root data;
    public PP_Manager mPP;

    private IEnumerator spawn_Orbs;

    public Vector3[] newFeetPos = new Vector3[10];

    private void Start()
    {
        mPP = mPP.GetComponent<PP_Manager>();
    }

    public void SetupPP()
    {
        int pc = mPP.playerCount;
        Debug.Log("setting up coroutine " + pc.ToString());

        for (int i = 0; i < pc; i++)
        {
            if (i < 5)
                newFeetPos[i] = mPP.feetPos[i];
            if (i > 4 && i < 10)
                newFeetPos[i] = mPP.feetPos[i];
        }
    }
}




