using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupPlayerNames : MonoBehaviour
{
    public string[] playerNames = new string[10];

    public Root data;

    public void SetupPlayerInfo()
    {
        for (int i = 0; i < data.Data.Names.Length; i++)
        {
            playerNames[i] = data.Data.Names[i];
        }
    }

}


