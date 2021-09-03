using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SB_Manager : MonoBehaviour
{
    public Root data;
    public PopulateMainScoreboard pmsb;

    public TMP_Dropdown playerDropDown;

    public int[] teamKills = new int[2];
    public int[] teamDeaths = new int[2];
    public int[] teamScore = new int[2];
    public int[] teamHS = new int[2];
    public int[] teamMapScore = new int[2];

    public float[] playerKD = new float[10];
    public float[] teamKD = new float[2];

    public List<PlayersInGame> pIG = new List<PlayersInGame>();
    public List<string> playerNamesDropDown = new List<string>();


    public bool sortingPlayers = false;

    private int startUpCount = 0;

    private void Start()
    {
        pmsb = pmsb.GetComponent<PopulateMainScoreboard>();

        for (int i = 0; i < 10; i++)
        {
            playerKD[i] = 0;
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
        for (int i = 0; i < 2; i++)
        {
            teamKills[i] = 0;
            teamDeaths[i] = 0;
            teamHS[i] = 0;
            teamScore[i] = 0;
        }

        // Debug.Log("setting up previous data");
        // also sets up team scores
        for (int i = 0; i < data.Data.Names.Length; i++)
        {
            // once we find the name, we add the victum to our has killed list
            if (pIG[i].Name == data.Data.Names[i])
            {
                pIG[i].PreviousDeaths = pIG[i].Deaths;
                pIG[i].PreviousKills = pIG[i].Kills;
            }

            if (data.Data.Teams[i] == 0)
            {
                teamKills[0] += pIG[i].Kills;
                teamDeaths[0] += pIG[i].Deaths;
                teamHS[0] += pIG[i].headShots;
                teamScore[0] += pIG[i].Score;
            }
            if (data.Data.Teams[i] == 1)
            {
                teamKills[1] += pIG[i].Kills;
                teamDeaths[1] += pIG[i].Deaths;
                teamHS[1] += pIG[i].headShots;
                teamScore[1] += pIG[i].Score;
            }
        }

        if (sortingPlayers)
        {
            PutPlayerDataWhereItShouldBe();
        }

        Debug.Log("calculate kd");
        CalculateKD();
        Debug.Log("calculate ks");
        CheckIfOnStreak();
    }
    public void SetInitialScoreBoard()
    {
        Debug.Log("setting up sb");
        startUpCount++;
        if (startUpCount == 1)
        {
            for (int i = 0; i < data.Data.Names.Length; i++)
            {
                if (data.Data.Teams[i] == 0)
                pIG.Add(new PlayersInGame(data.Data.Names[i], data.Data.Teams[i], data.Data.Scores[i], data.Data.Kills[i], data.Data.Deaths[i]));
            }
            for (int i = 0; i < data.Data.Names.Length; i++)
            {
                if (data.Data.Teams[i] == 1)
                    pIG.Add(new PlayersInGame(data.Data.Names[i], data.Data.Teams[i], data.Data.Scores[i], data.Data.Kills[i], data.Data.Deaths[i]));
            }

            startUpCount++;
        }

        // cut down names
        SetupPlayerTeamShortNames();

        sortingPlayers = true;
    }

    public void SetupPlayerTeamShortNames()
    {
        // put red first
        for (int i = 0; i < data.Data.Names.Length; i++)
        {
            if (data.Data.Teams[i] == 0)
            {
                // setup for [team] [name name]
                string[] splitNames = pIG[i].Name.Split();

                if (splitNames.Length > 1)
                {
                    if (pIG[i].Team == 0)
                    {
                        pIG[i].TeamName = splitNames[0];
                    }

                    // [name with spaces] setup
                    string nameWithSpaces = "";
                    for (int j = 1; j < splitNames.Length; j++)
                    {
                        nameWithSpaces += splitNames[j];
                    }

                    pIG[i].ShortName = nameWithSpaces;
                }
                else
                {
                    pIG[i].ShortName = data.Data.Names[i];

                }
            }
            // then blue
            if (data.Data.Teams[i] == 1)
            {
                // setup for [team] [name name]
                string[] splitNames = pIG[i].Name.Split();

                if (splitNames.Length > 1)
                {
                    if (pIG[i].Team == 1)
                    {
                        pIG[i].TeamName = splitNames[0];
                    }

                    // [name with spaces] setup
                    string nameWithSpaces = "";
                    for (int j = 1; j < splitNames.Length; j++)
                    {
                        nameWithSpaces += splitNames[j];
                    }

                    pIG[i].ShortName = nameWithSpaces;
                }
                else
                {
                    pIG[i].ShortName = data.Data.Names[i];

                }
            }
        }

        for (int i = 0; i < pIG.Count; i++)
        {
            playerNamesDropDown.Add(pIG[i].ShortName);
        }
        playerDropDown.AddOptions(playerNamesDropDown);

    }

    public void PutPlayerDataWhereItShouldBe()
    {
        for (int i = 0; i < pIG.Count; i++)
        {
            for (int j = 0; j < pIG.Count; j++)
            {
                if (pIG[i].Name == data.Data.Names[j])
                {
                    pIG[i].Kills = data.Data.Kills[j];
                    pIG[i].Deaths = data.Data.Deaths[j];
                    pIG[i].Score = data.Data.Scores[j];
                    pIG[i].Team = data.Data.Teams[j];
                }
            }
        }
    }

    public void CalculateKD()
    {
        for (int i = 0; i < pIG.Count; i++)
        {
            float k = pIG[i].Kills;
            float d = pIG[i].Deaths;

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
        for (int i = 0; i < pIG.Count; i++)
        {
            // died
            if (pIG[i].Deaths > pIG[i].PreviousDeaths)
            {
                pIG[i].isDead = true;
                pIG[i].CurrentKillStreak = 0;
                pIG[i].isStreaking = false;
            }
            // got a kill
            if (pIG[i].Kills > pIG[i].PreviousKills)
            {
                int math = (pIG[i].CurrentKillStreak + (pIG[i].Kills - pIG[i].PreviousKills));

                pIG[i].CurrentKillStreak = math;

                if (pIG[i].CurrentKillStreak > 2)
                {
                    pIG[i].isStreaking = true;
                }
            }
            SetHighKillStreak(i);
        }
    }

    // check if streaking, set high ks
    public void SetHighKillStreak(int _i)
    {
        if (pIG[_i].isStreaking)
        {
            if (pIG[_i].CurrentKillStreak > pIG[_i].HighestKillStreak)
            {
                pIG[_i].HighestKillStreak = pIG[_i].CurrentKillStreak;
            }
        }
    }

    public void ResetData()
    {
        for (int i = 0; i < data.Data.Names.Length; i++)
        {
            playerKD[i] = 0;
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

        pIG.Clear();
        sortingPlayers = false;
        startUpCount = 0;
    }
}


