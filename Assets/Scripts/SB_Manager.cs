using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SB_Manager : MonoBehaviour
{
    public Root data;

    public int[] previousKills = new int[10];
    public int[] previousDeaths = new int[10];
    public int[] currentKills = new int[10];
    public int[] currentDeaths = new int[10];
    public int[] teamList = new int[10];
    public int[] teamKills = new int[2];
    public int[] teamDeaths = new int[2];
    public int[] currentKillStreak = new int[10];
    public int[] highestKillStreak = new int[10];

    public float[] playerKD = new float[10];

    public bool[] isDead = new bool[10];
    public bool[] isStreaking = new bool[10];

    SetupPlayerNames spn;

    private void Start()
    {
        spn = GetComponent<SetupPlayerNames>();
        for (int i = 0; i < spn.playerNames.Length; i++)
        {
            previousKills[i] = 0;
            previousDeaths[i] = 0;
            currentKills[i] = 0;
            currentDeaths[i] = 0;
        }        
    }

    public void SetKDScores()
    {
        Debug.Log("starting sb");
        for (int i = 0; i < spn.playerNames.Length; i++)
        {
            previousDeaths[i] = currentDeaths[i];
            previousKills[i] = currentKills[i];
        }
            for (int i = 0; i < spn.playerNames.Length; i++)
        {
            currentKills[i] = data.Data.Kills[i];
            currentDeaths[i] = data.Data.Deaths[i];
            teamList[i] = data.Data.Teams[i];
        }
        Debug.Log("starting kd");
        for (int i = 0; i < spn.playerNames.Length; i++)
        {
            if (data.Data.Teams[i] == 0)
            {
                currentKills[0] += data.Data.Kills[i];
                currentDeaths[0] += data.Data.Deaths[i];
            }
            if (data.Data.Teams[i] == 1)
            {
                currentKills[1] += data.Data.Kills[i];
                currentDeaths[1] += data.Data.Deaths[i];
            }

        }

        Debug.Log("calculate kd");
        CalculateKD();
        Debug.Log("calculate ks");
        CheckIfOnStreak();
    }

    public void CalculateKD()
    {
        for (int i = 0; i < spn.playerNames.Length; i++)
        {
            float k = currentKills[i];
            float d = currentDeaths[i];
            playerKD[i] = (k / d);
        }

        CheckIfOnStreak();
    }

    public void CheckIfOnStreak()
    {
        // check if a player died. If so set kill streak to 0 and turn off bool for streaking
        for (int i = 0; i < currentKills.Length; i++)
        {
            if (currentDeaths[i] > previousDeaths[i])
            {
                isDead[i] = true;
                currentKillStreak[i] = 0;
                isStreaking[i] = false;
                //Debug.Log("should be starting a kill bar coroutine");
                //coroutineDeathBar = SpawnDB(4.0f, i);
                //StartCoroutine(coroutineDeathBar);
            }
            if (currentKills[i] > previousKills[i])
            {
                int math = (currentKillStreak[i] + (currentKills[i] - previousKills[i]));

                currentKillStreak[i] = math;

                if (currentKillStreak[i] > 2)
                {
                    isStreaking[i] = true;
                }
            }
            SetHighKillStreak(i);
        }     
    }

    // check if streaking, set high ks
    public void SetHighKillStreak(int _i)
    {
        if (isStreaking[_i])
        {
            if (currentKillStreak[_i] > highestKillStreak[_i])
            {
                highestKillStreak[_i] = currentKillStreak[_i];
            }
        }
    }
}
