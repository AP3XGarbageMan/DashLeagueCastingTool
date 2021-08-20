using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupPlayerNames : MonoBehaviour
{

    //public int[] playerKills = new int[10];
    //public int[] playerDeaths = new int[10];

    //public int[] teamList = new int[10];
    //public int[] teamKills = new int[2];
    //public int[] teamDeaths = new int[2];

    public string[] playerNames = new string[10];

    public bool setupPI = false;

    public Root data;

    private void Update()
    {
        if (setupPI)
        {

            SetupPlayerInfo();
            setupPI = false;
        }
    }

    public void SetupPlayerInfo()
    {
        for (int i = 0; i < data.Data.Names.Length; i++)
        {
            playerNames[i] = data.Data.Names[i];
        }
        //TestCoRoutine.totalPlayerNum = staticPlayerNames.Length;
    }

}


