using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SB_Manager : MonoBehaviour
{
    public Root data;
    public PopulateMainScoreboard pmsb;

    public int[] previousKills = new int[10];
    public int[] previousDeaths = new int[10];
    public int[] currentKills = new int[10];
    public int[] currentDeaths = new int[10];
    public int[] currentScore = new int[10];
    public int[] currentHS = new int[10];
    public int[] teamList = new int[10];

    public int[] teamKills = new int[2];
    public int[] teamDeaths = new int[2];
    public int[] teamScore = new int[2];
    public int[] teamHS = new int[2];
    public int[] teamMapScore = new int[2];

    public int[] currentKillStreak = new int[10];
    public int[] highestKillStreak = new int[10];

    public float[] playerKD = new float[10];
    public float[] teamKD = new float[2];

    public bool[] isDead = new bool[10];
    public bool[] isStreaking = new bool[10];

    public string[] playerNamesWithSpaces = new string[10];

    public string[] teamNames = new string[2];

    private void Start()
    {
        pmsb = pmsb.GetComponent<PopulateMainScoreboard>();

        for (int i = 0; i < data.Data.Names.Length; i++)
        {
            previousKills[i] = 0;
            previousDeaths[i] = 0;
            currentKills[i] = 0;
            currentDeaths[i] = 0;
            currentScore[i] = 0;
            currentHS[i] = 0;
            teamList[i] = 0;
            currentKillStreak[i] = 0;
            highestKillStreak[i] = 0;
            playerKD[i] = 0;

            isDead[i] = false;
            isStreaking[i] = false;

            playerNamesWithSpaces[i] = "name";
        }
        for (int i = 0; i < 2; i++)
        {
            teamKills[i] = 0;
            teamDeaths[i] = 0;
            teamHS[i] = 0;
            teamKD[i] = 0;
            teamScore[i] = 0;
            teamMapScore[i] = 0;
        }   
    }

    public void SetScoreBoard()
    {
        teamKills[0] = 0;
        teamKills[1] = 0;
        teamDeaths[0] = 0;
        teamDeaths[1] = 0;

        Debug.Log("setting up previous data");
        for (int i = 0; i < data.Data.Names.Length; i++)
        {
            previousDeaths[i] = currentDeaths[i];
            previousKills[i] = currentKills[i];
        }
        for (int i = 0; i < data.Data.Names.Length; i++)
        {
            currentKills[i] = data.Data.Kills[i];
            currentDeaths[i] = data.Data.Deaths[i];
            currentScore[i] = data.Data.Scores[i];
            teamList[i] = data.Data.Teams[i];
        }
        for (int i = 0; i < data.Data.Names.Length; i++)
        {
            if (data.Data.Teams[i] == 0)
            {
                teamKills[0] += data.Data.Kills[i];
                teamDeaths[0] += data.Data.Deaths[i];
            }
            if (data.Data.Teams[i] == 1)
            {
                teamKills[1] += data.Data.Kills[i];
                teamDeaths[1] += data.Data.Deaths[i];
            }
        }

        for (int i = 0; i < data.Data.Names.Length; i++)
        {
            // setup for [team] [name name]
            string[] splitNames = data.Data.Names[i].Split();

            if (splitNames.Length > 1)
            {
                if (i < 5)
                {
                    teamNames[0] = splitNames[0];
                }
                // blue team
                if (i > 4)
                {
                    teamNames[1] = splitNames[0];
                }


                // [name with spaces] setup
                string nameWithSpaces = "";
                for (int j = 1; j < splitNames.Length; j++)
                {
                    nameWithSpaces += splitNames[j];
                }

                playerNamesWithSpaces[i] = nameWithSpaces;
            }
            else
            {
                playerNamesWithSpaces[i] = data.Data.Names[i];

            }
        }

        Debug.Log("calculate kd");
        CalculateKD();
        Debug.Log("calculate ks");
        CheckIfOnStreak();
    }

    public void CalculateKD()
    {
        for (int i = 0; i < data.Data.Names.Length; i++)
        {
            float k = currentKills[i];
            float d = currentDeaths[i];
            if (k != 0 && d != 0)
            {
                playerKD[i] = (k / d);
            }
            else
            {
                playerKD[i] = 0f;
            }
        }
        for (int i = 0; i < 2; i++)
        {
            float tk = teamKills[i];
            float td = teamDeaths[i];
            
            if (tk != 0 && td != 0)
            {
                teamKD[i] = (tk / td);
            }
            else
            {
                teamKD[i] = 0f;
            }
        }
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
