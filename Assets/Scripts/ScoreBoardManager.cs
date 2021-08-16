using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreBoardManager : MonoBehaviour
{
    public List<int> teamNumList = new List<int>();
    public List<int> teamHSList = new List<int>();
    public List<int> teamKillsList = new List<int>();
    public List<int> teamDeathsList = new List<int>();
    public List<int> currentHSList = new List<int>();
    public List<int> currentKills = new List<int>();
    public List<int> currentDeaths = new List<int>();

    private List<string> playerNames = new List<string>();
    private List<string> playerNamesWithSpaces = new List<string>();

    public static List<bool> isDead = new List<bool>();
    private List<bool> isNameInfoShowing = new List<bool>();


    [SerializeField]
    private GameObject[] playerMainUIInfoHolder;

    [SerializeField]
    private TextMeshProUGUI[] playerNamesTMP;
    [SerializeField]
    private TextMeshProUGUI[] teamNamesTMP;
    [SerializeField]
    private TextMeshProUGUI[] playerKillsTMP;
    [SerializeField]
    private TextMeshProUGUI[] playerDeathsTMP;
    [SerializeField]
    private TextMeshProUGUI[] playerKDTMP;
    [SerializeField]
    private TextMeshProUGUI[] playerHSTMP;

    public Toggle isPayload;

    [SerializeField]
    private Transform[] verticalRosterHolder;

    private IEnumerator coroutinePInfo;

    // inialize everything with 0 or name
    public void Start()
    {
        // populate lists for first maths
        for (int i = 0; i < 10; i++)
        {
            currentKills.Add(0);
            currentDeaths.Add(0);
            currentHSList.Add(0);
            teamNumList.Add(0);
            teamHSList.Add(0);
            playerNames.Add("name");
        }
        for (int i = 0; i < 10; i++)
        {
            isDead.Add(false);
        }
        for (int i = 0; i < 2; i++)
        {
            teamDeathsList.Add(0);
            teamKillsList.Add(0);
            teamHSList.Add(0);
        }
    }

    public void GetSBEvent(Root data)
    {
        for (int j = 0; j < data.Data.Names.Length; j++)
        {
            playerNames[j] = data.Data.Names[j];
            teamKillsList[j] = data.Data.Kills[j];
            teamDeathsList[j] = data.Data.Deaths[j];
            teamNumList[j] = data.Data.Teams[j];
        }

        //SetHeadShots(data);
        SetPlayerNamesList(data);
        SetKillDeathsList(data);
    }

    public void SetHeadShots(Root data)
    {
        for (int i = 0; i < playerNames.Count; i++)
        {
            if (playerNames[i] == data.Data.Killer)
            {
                if (data.Data.HeadShot)
                {
                    //staticHeadShotCounter[i]++;
                    if (teamNumList[i] == 0)
                    {
                        // add one to red
                        teamHSList[0]++;
                    }
                    if (teamNumList[i] == 1)
                    {
                        // add one to blue
                        teamHSList[1]++;
                    }
                }

                if (teamNumList[i] == 0)
                {
                    teamKillsList[0]++;
                }
                if (teamNumList[i] == 1)
                {
                    teamKillsList[1]++;
                }
            }
        }
    }

    public void SetKillDeathsList(Root data)
    {
        for (int i = 0; i < playerNames.Count; i++)
        {
            if (playerNames[i] == data.Data.Victum)
            {
                if (teamNumList[i] == 0)
                {
                    teamDeathsList[0]++;
                }
                if (teamNumList[i] == 1)
                {
                    teamDeathsList[1]++;
                }
            }
        }
    }

    public void SetPlayerNamesList(Root data)
    {
        // setup player names
        for (int i = 0; i < playerNames.Count; i++)
        {
            // setup for [team] [name name]
            string[] splitNames = playerNames[i].Split();

            if (splitNames.Length > 1)
            {
                if (isPayload.isOn)
                {
                    if (i < 5)
                    {
                        teamNamesTMP[2].text = splitNames[0];
                        teamNamesTMP[3].text = splitNames[0];
                    }
                    // blue team
                    if (i > 4)
                    {
                        teamNamesTMP[0].text = splitNames[0];
                        teamNamesTMP[1].text = splitNames[0];
                    }
                }
                else
                {
                    // [team] setup
                    // red team
                    if (i < 5)
                    {
                        teamNamesTMP[0].text = splitNames[0];
                        teamNamesTMP[1].text = splitNames[0];
                    }
                    // blue team
                    if (i > 4)
                    {
                        teamNamesTMP[2].text = splitNames[0];
                        teamNamesTMP[3].text = splitNames[0];
                    }
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
                playerNamesWithSpaces[i] = playerNames[i];
            }
        }
    }

    public void SetupKDHSSB()
    {
        // calculate team K, D, HS
        for (int i = 0; i < playerNames.Count; i++)
        {
            if (teamNumList[i] == 0)
            {
                // sets up a coroutine to spawn the main overlay roster info            
                coroutinePInfo = SpawnPInfo(i, Color.red, 2f, 0);

                // display main vertical roster info on screen
                StartCoroutine(coroutinePInfo);
            }

            // Same thing as above just for the blue team
            if (teamNumList[i] == 1)
            {
                // sets up a coroutine to spawn the main overlay roster info       
                coroutinePInfo = SpawnPInfo(i, Color.blue, 2f, 1);
                // display main vertial roster info on screen
                StartCoroutine(coroutinePInfo);
            }

            //playerKDTMP[i].text = "0";
            playerKillsTMP[i].text = teamKillsList[i].ToString();
            playerDeathsTMP[i].text = teamDeathsList[i].ToString();
            playerHSTMP[i].text = teamHSList[i].ToString();
            playerNamesTMP[i].text = playerNamesWithSpaces[i];

            if (teamKillsList[i] != 0 && teamDeathsList[i] != 0)
            {
                float k = teamKillsList[i];
                float d = teamDeathsList[i];
                float killf = (k / d);

                playerKDTMP[i].text = killf.ToString("0.0");
            }
            else
            {
                playerKDTMP[i].text = "0";
            }
        }
    }

    // Populate player info for main screen rosters and top screen scoreboard. 
    IEnumerator SpawnPInfo(int i, Color _topBorder, float _waitTime, int _team)
    {
        //TODO Don't spawn new holders every 2 sec change the info on holder
        //TODO When we get new info from the socketstreamer we should update (event)

        foreach (Transform child in verticalRosterHolder[_team])
        {
            if (child.name == ("pInfo_R_" + i.ToString()) || child.name == ("pInfo_B_" + i.ToString()))
            {
                isNameInfoShowing[i] = true;
            }
        }

        if (isNameInfoShowing[i])
        {
            //Debug.Log("already showing");
        }
        else
        {
            GameObject pInfo = Instantiate(playerMainUIInfoHolder[_team], verticalRosterHolder[_team]);

            if (isDead[i])
            {
                pInfo.transform.GetChild(1).gameObject.SetActive(true);
            }

            pInfo.name = "pInfo_R" + i.ToString();
            if (_team == 0)
            {
                //pInfo.transform.GetChild(0).GetComponent<Image>().material = m_redRosterBackground;
                pInfo.name = "pInfo_R_" + i.ToString();
                pInfo.transform.GetChild(0).GetChild(7).GetComponent<TextMeshProUGUI>().text = playerNamesWithSpaces[i];
                pInfo.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = teamKillsList[i].ToString();
                pInfo.transform.GetChild(0).GetChild(6).GetComponent<TextMeshProUGUI>().text = teamDeathsList[i].ToString();
                //pInfo.transform.GetChild(0).GetChild(1)..GetComponent<TextMeshProUGUI>().text = SocketServer.staticHeadShotCounter[i].ToString();
            }
            else
            {
                //pInfo.transform.GetChild(0).GetComponent<Image>().material = m_blueRosterBackground;
                pInfo.name = "pInfo_B_" + i.ToString();
                pInfo.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = playerNamesWithSpaces[i];
                pInfo.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = teamKillsList[i].ToString();
                pInfo.transform.GetChild(0).GetChild(7).GetComponent<TextMeshProUGUI>().text = teamDeathsList[i].ToString();
                //pInfo.transform.GetChild(0).GetChild(1)..GetComponent<TextMeshProUGUI>().text = SocketServer.staticHeadShotCounter[i].ToString();
            }

            yield return new WaitForSeconds(_waitTime);
            Destroy(pInfo);
            isNameInfoShowing[i] = false;
        }
    }
}
