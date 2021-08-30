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

    //IEnumerator SpawnOrbs(int _pc)
    //{
    //    Debug.Log("length is " + _pc.ToString());


    //    Debug.Log("Will you wait for me");
    //    yield return new WaitForSeconds(2.0f);
    //    Debug.Log("I waited");

    //}

    //    private void Start()
    //    {
    //        mSB = mSB.GetComponent<SB_Manager>();
    //    }

    //    public void SetupNewPlayer()
    //    {
    //        for (int i = 0; i< data.Data.Names.Length; i++)
    //        {
    //            Player newPlayer = new Player();
    //            newPlayer.Name = mSB.data.Data.Names[i];
    //            newPlayer.Team = mSB.data.Data.Teams[i];
    //            newPlayer.Kills = mSB.data.Data.Kills[i];
    //            newPlayer.Deaths = mSB.data.Data.Deaths[i];
    //            newPlayer.Score = mSB.data.Data.Scores[i];
    //        }
    //    }

}

//[Serializable]
//public class Player
//{
//    public string Name;
//    public int Team;
//    public int Kills;
//    public int Deaths;
//    public int Score;
//}

//public class Players
//{
//    public string[] Names;
//    public int[] Teams;
//    public int[] Kills;
//    public int[] Deaths;
//    public int[] Scores;
//}



