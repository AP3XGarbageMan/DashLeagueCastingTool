using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SB_Manager : MonoBehaviour
{
    public Root data;
    public PopulateMainScoreboard pmsb;
    public DataToWrite_Manager mDTW;
    public Domination_Manager mD;

    public float gameTime = 0.0f;
    public int[] previousKills = new int[10];
    public int[] previousDeaths = new int[10];
    public int[] currentKills = new int[10];
    public int[] currentDeaths = new int[10];
    public int[] currentScore = new int[10];
    public int[] currentHS = new int[10];
    public int[] currentHealth = new int[10];
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

    public string[] playerName = new string[10];
    public string[] playerNamesWithSpaces = new string[10];

    public string[] teamNames = new string[2];

    public string mapName = "";
    private int startUpCount = 0;

    public bool sortingPlayers = false;

    public List<PlayerStatData> pIG = new List<PlayerStatData>();
    public List<ScoreTrackingStats> scoreStatsRed = new List<ScoreTrackingStats>();
    public List<ScoreTrackingStats> scoreStatsBlue = new List<ScoreTrackingStats>();
    public List<DominationBUttonStats> dBS = new List<DominationBUttonStats>();

    public List<string> playerNamesDropDown = new List<string>();

    public TMP_Dropdown playerDropDown;

    private void Start()
    {
        SocketServer.ScoreBoardEvent += SetScoreBoard;
        Payload_Manager.PayloadUpdateOut += UpdateFromPayload;

        pmsb = pmsb.GetComponent<PopulateMainScoreboard>();
        mDTW = mDTW.GetComponent<DataToWrite_Manager>();
        mD = mD.GetComponent<Domination_Manager>();

        for (int i = 0; i < data.Data.Names.Length; i++)
        {
            previousKills[i] = 0;
            previousDeaths[i] = 0;
            currentKills[i] = 0;
            currentDeaths[i] = 0;
            currentScore[i] = 0;
            currentHS[i] = 0;
            currentHealth[i] = 100;
            teamList[i] = 0;
            currentKillStreak[i] = 0;
            highestKillStreak[i] = 0;
            playerKD[i] = 0;

            isDead[i] = false;
            isStreaking[i] = false;

            playerNamesWithSpaces[i] = "name";
            playerName[i] = "FullName";
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


    public void UpdateFromPayload(Root _PayloadUpdateData)
    {
        teamMapScore[0] = _PayloadUpdateData.Data.RedPercent;
        teamMapScore[1] = _PayloadUpdateData.Data.BluePercent;
    }
    public void UpdateFromDomination(int _red, int _blue)
    {
        teamMapScore[0] = _red;
        teamMapScore[1] = _blue;
    }
    public void UpdateFromCP(int _red, int _blue)
    {
        teamMapScore[0] = _red;
        teamMapScore[1] = _blue;
    }


    public void SetScoreBoard(Root _data)
    {
        this.data = _data;

        for (int i = 0; i < 2; i++)
        {
            teamKills[i] = 0;
            teamDeaths[i] = 0;
            teamHS[i] = 0;
            teamScore[i] = 0;
        }

        gameTime = data.TimeStamp;

        //Debug.Log("setting up previous data");
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
            currentHealth[i] = data.Data.Health[i];
            teamList[i] = data.Data.Teams[i];
        }
        for (int i = 0; i < data.Data.Names.Length; i++)
        {
            if (data.Data.Teams[i] == 0)
            {
                teamKills[0] += data.Data.Kills[i];
                teamDeaths[0] += data.Data.Deaths[i];
                teamScore[0] += data.Data.Scores[i];
                teamHS[0] += currentHS[i];
            }
            if (data.Data.Teams[i] == 1)
            {
                teamKills[1] += data.Data.Kills[i];
                teamDeaths[1] += data.Data.Deaths[i];
                teamScore[1] += data.Data.Scores[i];
                teamHS[1] += currentHS[i];
            }
        }
        for (int i = 0; i < data.Data.Names.Length; i++)
        {
            playerName[i] = data.Data.Names[i];
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

        if (sortingPlayers)
        {
            for (int i = 0; i < pIG.Count; i++)
            {
                for (int j = 0; j < pIG.Count; j++)
                {
                    if (playerNamesWithSpaces[j] == pIG[i].ShortName)
                    {
                        pIG[i].Kills = currentKills[j];
                        pIG[i].Deaths = currentDeaths[j];
                        pIG[i].Score = currentScore[j];
                        pIG[i].headShots = currentHS[j];
                        pIG[i].Team = teamList[j];
                    }
                }
            }

            
        }

        //Debug.Log("calculate kd");
        CalculateKD();
        SetupDataToSave();
    }

    public void SetupDataToSave()
    {
        scoreStatsBlue.Add(new ScoreTrackingStats(data.TimeStamp, teamScore[1]));
        scoreStatsRed.Add(new ScoreTrackingStats(data.TimeStamp, teamScore[0]));
    }

    public void SetInitialScoreBoard()
    {
        //Debug.Log("setting up initial sb");
        startUpCount++;
        if (startUpCount == 1)
        {
            for (int i = 0; i < data.Data.Names.Length; i++)
            {
                if (data.Data.Teams[i] == 0)
                    pIG.Add(new PlayerStatData(data.Data.Names[i], data.Data.Teams[i], data.Data.Scores[i], data.Data.Kills[i], data.Data.Deaths[i]));
            }
            for (int i = 0; i < data.Data.Names.Length; i++)
            {
                if (data.Data.Teams[i] == 1)
                    pIG.Add(new PlayerStatData(data.Data.Names[i], data.Data.Teams[i], data.Data.Scores[i], data.Data.Kills[i], data.Data.Deaths[i]));
            }

            startUpCount++;
        }

        // cut down names
        SetupPlayerTeamShortNames();
        sortingPlayers = true;
    }


    public void SetupPlayerTeamShortNames()
    {
        for (int i = 0; i < data.Data.Names.Length; i++)
        {

            // setup for [team] [name name]
            string[] splitNames = pIG[i].Name.Split();

            if (splitNames.Length > 1)
            {
                pIG[i].TeamName = splitNames[0];

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
                pIG[i].TeamName = "Team";
            }
        }

        for (int i = 0; i < pIG.Count; i++)
        {
            playerNamesDropDown.Add(pIG[i].ShortName);
        }
        playerDropDown.AddOptions(playerNamesDropDown);

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

    public void ResetGameData()
    {
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
            playerName[i] = "FullName";
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
        scoreStatsBlue.Clear();
        scoreStatsRed.Clear();
        mD.domBTStats.Clear();
        
    }

}
