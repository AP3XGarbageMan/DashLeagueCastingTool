using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class DataToWrite_Manager : MonoBehaviour
{
    public Root data;
    public SB_Manager mSB;
    public SocketServer ss;
    public Domination_Manager mD;
    public Payload_Manager mP;
    public DataGraph dG;

    public List<List<int>> hasKilledList = new List<List<int>>();
    public List<List<int>> killedByList = new List<List<int>>();

    [SerializeField]
    public TextMeshProUGUI pathTeam1TMP;
    [SerializeField]
    public TextMeshProUGUI pathTeam2TMP;
    [SerializeField]
    public TextMeshProUGUI pathMapTMP;
    [SerializeField]
    public TextMeshProUGUI filePathTMP;

    [SerializeField]
    public TextMeshProUGUI dropDownText;

    public int teamNum = 0;
    public int playerI = 0;

    private void Start()
    {
        mD = mD.GetComponent<Domination_Manager>();
        mP = mP.GetComponent<Payload_Manager>();
        dG = dG.GetComponent<DataGraph>();
    }

    // tally up the list
    public void CleanUpWKWData()
    {
        if (hasKilledList != null)
        {
            hasKilledList.Clear();
        }

        for (int i = 0; i < mSB.pIG.Count; i++)
        {
            List<int> dataLine = new List<int>();
            for (int j = 0; j < mSB.pIG.Count; j++)
            {
                int killCount = 0;
                foreach (string _victim in mSB.pIG[i].hasKilled)
                {
                    // add the killer first
                    if (mSB.pIG[j].ShortName == _victim)
                    {
                        killCount++;
                    }
                }
                dataLine.Add(killCount);
            }
            hasKilledList.Add(dataLine);

            //Debug.Log("length of data line = " + dataLine.Count.ToString() );
        }

    }

    // tally up the list
    public void CleanUpWKBWData()
    {
        if (killedByList != null)
        {
            killedByList.Clear();
        }

        for (int i = 0; i < mSB.pIG.Count; i++)
        {
            List<int> dataLineB = new List<int>();
            for (int j = 0; j < mSB.pIG.Count; j++)
            {
                int deathCount = 0;
                foreach (string _killer in mSB.pIG[i].killedBy)
                {
                    // add the killer first
                    if (mSB.pIG[j].ShortName == _killer)
                    {
                        deathCount++;
                    }
                }
                dataLineB.Add(deathCount);
            }
            killedByList.Add(dataLineB);

            //Debug.Log("length of data line = " + dataLine.Count.ToString() );
        }
    }

    public void WriteWhoKilledWhoList()
    {
        string fileName = filePathTMP.text + pathTeam1TMP.text + "_" + pathTeam2TMP.text + "_" + pathMapTMP.text + "whoKilledWho.txt";

        StreamWriter sw = new StreamWriter(fileName);

        // start with each player as i
        for (int i = 0; i < mSB.pIG.Count; i++)
        {
            string listString = "";
            foreach (int val in hasKilledList[i])
            {
                listString += val + ", ";
            }

            string[] splitString = listString.Split(',');

            Debug.Log("List string for " + mSB.pIG[i].ShortName + " = " + listString);

            // check which team they are on, and only print the other values
            if (mSB.pIG[i].Team == 0 && i < 5)
            {
                Debug.Log("normal conditions killer was red, opposing team is blue");
                for (int j = 5; j < mSB.pIG.Count; j++)
                {
                    sw.WriteLine(mSB.pIG[i].TeamName + " " + mSB.pIG[i].ShortName + " killed " + mSB.pIG[j].TeamName + " " + mSB.pIG[j].ShortName + " " + splitString[j] + " times.");
                }

            }
            if (teamNum == 1 && i > 4)
            {
                Debug.Log("normal conditions killer was blue, opposing team is red");
                for (int j = 0; j < 5; j++)
                {
                    sw.WriteLine(mSB.pIG[i].TeamName + " " + mSB.pIG[i].ShortName + " killed " + mSB.pIG[j].TeamName + " " + mSB.pIG[j].ShortName + " " + splitString[j] + " times.");
                }
            }
            if (teamNum == 0 && i > 4)
            {
                Debug.Log("flipped payload, killer is red, blue team is now j < 5");
                for (int j = 0; j < 5; j++)
                {
                    sw.WriteLine(mSB.pIG[i].TeamName + " " + mSB.pIG[i].ShortName + " killed " + mSB.pIG[j].TeamName + " " + mSB.pIG[j].ShortName + " " + splitString[j] + " times.");
                }

            }
            if (teamNum == 1 && i < 5)
            {
                Debug.Log("flipped payload, killer is blue, red team is j > 4");
                for (int j = 5; j < mSB.pIG.Count; j++)
                {
                    sw.WriteLine(mSB.pIG[i].TeamName + " " + mSB.pIG[i].ShortName + " killed " + mSB.pIG[j].TeamName + " " + mSB.pIG[j].ShortName + " " + splitString[j] + " times.");
                }
            }
        }
        sw.Close();

    }

    public void WriteWhoKilledByWhoList()
    {
        string fileName = filePathTMP.text + pathTeam1TMP.text + "_" + pathTeam2TMP.text + "_" + pathMapTMP.text + "WhoKilledByWho.txt";

        StreamWriter sw = new StreamWriter(fileName);

        // start with each player as i
        for (int i = 0; i < mSB.pIG.Count; i++)
        {
            string listString = "";
            foreach (int val in killedByList[i])
            {
                listString += val + ", ";
            }

            string[] splitString = listString.Split(',');

            Debug.Log("List string for " + mSB.pIG[i].ShortName + " = " + listString);

            // check which team they are on, and only print the other values
            if (teamNum == 0 && i < 5)
            {
                // Debug.Log("normal conditions killer was red, opposing team is blue");
                for (int j = 5; j < mSB.pIG.Count; j++)
                {
                    sw.WriteLine(mSB.pIG[i].TeamName + " " + mSB.pIG[i].ShortName + " killed by" + mSB.pIG[j].TeamName + " " + mSB.pIG[j].ShortName + " " + splitString[j] + " times.");
                }

            }
            if (teamNum == 1 && i > 4)
            {
                // Debug.Log("normal conditions killer was blue, opposing team is red");
                for (int j = 0; j < 5; j++)
                {
                    sw.WriteLine(mSB.pIG[i].TeamName + " " + mSB.pIG[i].ShortName + " killed by" + mSB.pIG[j].TeamName + " " + mSB.pIG[j].ShortName + " " + splitString[j] + " times.");
                }
            }
            if (teamNum == 0 && i > 4)
            {
                // Debug.Log("flipped payload, killer is red, blue team is now i < 5");
                for (int j = 0; j < 5; j++)
                {
                    sw.WriteLine(mSB.pIG[i].TeamName + " " + mSB.pIG[i].ShortName + " killed by" + mSB.pIG[j].TeamName + " " + mSB.pIG[j].ShortName + " " + splitString[j] + " times.");
                }

            }
            if (teamNum == 1 && i < 5)
            {
                // Debug.Log("flipped payload, killer is blue, red team is not i > 4");
                for (int j = 5; j < mSB.pIG.Count; j++)
                {
                    sw.WriteLine(mSB.pIG[i].TeamName + " " + mSB.pIG[i].ShortName + " killed by" + mSB.pIG[j].TeamName + " " + mSB.pIG[j].ShortName + " " + splitString[j] + " times.");
                }
            }
        }
        sw.Close();

    }

    public void WriteAllData()
    {
        CleanUpWKWData();
        CleanUpWKBWData();

        WriteWhoKilledWhoList();
        WriteWhoKilledByWhoList();

        WriteSPG();
        WriteBTDom();
        WritePayloadProgress();
    }

    public void WriteSPG()
    {
        WriteScorePerGameRed();
        WriteScorePerGameBlue();
    }

    public void WriteScorePerGameRed()
    {
        string fileName = filePathTMP.text + pathTeam1TMP.text + "_" + pathTeam2TMP.text + "_" + pathMapTMP.text + "scorePerGameRed.txt";

        StreamWriter sw = new StreamWriter(fileName);

        // start with each player as i
        for (int i = 0; i < mSB.scoreStatsRed.Count; i++)
        {
            sw.WriteLine(mSB.scoreStatsRed[i].Time.ToString() + ", " + mSB.scoreStatsRed[i].Score.ToString());
        }
        sw.Close();

    }

    public void WriteScorePerGameBlue()
    {
        string fileName = filePathTMP.text + pathTeam1TMP.text + "_" + pathTeam2TMP.text + "_" + pathMapTMP.text + "scorePerGameBlue.txt";

        StreamWriter sw = new StreamWriter(fileName);

        // start with each player as i
        for (int i = 0; i < mSB.scoreStatsBlue.Count; i++)
        {
            sw.WriteLine(mSB.scoreStatsBlue[i].Time.ToString() + ", " + mSB.scoreStatsBlue[i].Score.ToString());
        }
        sw.Close();

    }

    public void WriteJSON()
    {
        string fileName = filePathTMP.text + pathTeam1TMP.text + "_" + pathTeam2TMP.text + "_" + pathMapTMP.text + "jsonOut.txt";

        StreamWriter sw = new StreamWriter(fileName);

        // start with each player as i
        for (int i = 0; i < ss.jsonOut.Count; i++)
        {
            sw.WriteLine(ss.jsonOut[i]);
        }
        sw.Close();

    }

    public void WritePlayerData()
    {
        string fileName = filePathTMP.text + pathTeam1TMP.text + "_" + pathTeam2TMP.text + "_" + pathMapTMP.text + "playerStats.txt";

        StreamWriter sw = new StreamWriter(fileName);

        sw.WriteLine("Name, team, kills, deaths, score, head shots,");

        // start with each player as i
        for (int i = 0; i < mSB.playerName.Length; i++)
        {
            sw.WriteLine(mSB.pIG[i].TeamName + ", " + mSB.pIG[i].ShortName + ", " + mSB.pIG[i].Team + ", " + mSB.pIG[i].Kills.ToString() + ", " + mSB.pIG[i].Deaths.ToString() + ", " + mSB.pIG[i].Score.ToString() + ", " + mSB.pIG[i].headShots.ToString());
        }

        sw.Close();
    }

    public void WriteBTDom()
    {
        string fileName = filePathTMP.text + pathTeam1TMP.text + "_" + pathTeam2TMP.text + "_" + pathMapTMP.text + "BTDomRed.txt";

        StreamWriter sw = new StreamWriter(fileName);

        // start with each player as i
        for (int i = 0; i < mD.domBTStats.Count; i++)
        {
            sw.WriteLine(mD.domBTStats[i].Time.ToString() + ", " + mD.domBTStats[i].ButtonTapsRed.ToString() + ", " + mD.domBTStats[i].ButtonTapsBlue.ToString());
        }
        sw.Close();

    }


    public void WritePayloadProgress()
    {
        string fileName = filePathTMP.text + pathTeam1TMP.text + "_" + pathTeam2TMP.text + "_" + pathMapTMP.text + "PayloadProgress.txt";

        StreamWriter sw = new StreamWriter(fileName);

        sw.WriteLine("Payload swap time = " + dG.whereCartChanged.ToString() );
        // start with each player as i
        for (int i = 0; i < mD.domBTStats.Count; i++)
        {
            sw.WriteLine(mP.payloadCartTrackerList[i].Time.ToString() + ", " + mP.payloadCartTrackerList[i].redPayloadPercent.ToString() + ", " + mP.payloadCartTrackerList[i].bluePayloadPercent.ToString());
        }
        sw.Close();

    }
}
