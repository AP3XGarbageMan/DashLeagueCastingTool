using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class KillStreakCounter : MonoBehaviour
{
    private List<string> playerNamesWithSpaces = new List<string>();

    [SerializeField]
    private GameObject playerInfoHolder;

    [SerializeField]
    private TextMeshProUGUI[] playerGameScoreTMP; // red 0, 1 | blue 2, 3

    private List<int> kTot = new List<int>();
    private List<int> dTot = new List<int>();
    private List<int> hTot = new List<int>();

    public TextMeshProUGUI[] teamNamesTMP;
    public TextMeshProUGUI[] playerNamesTMP;
    public TextMeshProUGUI[] playerKillsTMP;
    public TextMeshProUGUI[] playerDeathsTMP;
    public TextMeshProUGUI[] playerKDTMP;
    public TextMeshProUGUI[] playerHSTMP;
    public TextMeshProUGUI[] playerKillsTotTMP;
    public TextMeshProUGUI[] playerDeathsTotTMP;
    public TextMeshProUGUI[] playerKDTotTMP;
    public TextMeshProUGUI[] playerHSTotTMP;


    private List<bool> isNameInfoShowing = new List<bool>();

    public string[] cutTeamNames;

    private IEnumerator coroutinePInfo;


    [SerializeField]
    private GameObject playerInfoHolderParent;
    [SerializeField]
    private GameObject playerEndScoreInfo;

    [SerializeField]
    private Material m_blueTopBorder;
    [SerializeField]
    private Material m_redTopBorder;
    [SerializeField]
    private Material m_concrete;



    [SerializeField]
    private GameObject[] wwMapParts;

    public void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            playerGameScoreTMP[i].text = "0";
        }

        for (int i = 0; i < 10; i++)
        {
            playerNamesWithSpaces.Add("not name");
        }
        for (int i = 0; i < 10; i++)
        {
            isNameInfoShowing.Add(false);
        }
        for (int i = 0; i < 2; i++)
        {
            kTot.Add(0);
            dTot.Add(0);
            hTot.Add(0);
        }
    }


    public void Update()
    {
        if (SocketServer.staticReadingData)
        {
            UpdateData();
        }
    }

    public void UpdateData()
    {
        // setup player names
        for (int i = 0; i < SocketServer.staticPlayerNamesList.Length; i++)
        {
            // setup for [team] [name name]
            string[] splitNames = SocketServer.staticPlayerNamesList[i].Split();

            if (splitNames.Length > 1)
            {
                // [team] setup
                // red team
                if (i < 5)
                {
                    teamNamesTMP[0].text = splitNames[0];
                }
                // blue team
                else
                {
                    teamNamesTMP[1].text = splitNames[0];
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
                playerNamesWithSpaces[i] = SocketServer.staticPlayerNamesList[i];
            }
        }

        // clear kill, death, headshot counters
        for (int i = 0; i < 2; i++)
        {
            kTot[i] = 0;
            dTot[i] = 0;
            hTot[i] = 0;
        }

        // update main scoreboard
        // send the right data to the right place for payload, dom, and CP
        if (SocketServer.staticIsPayload)
        {
            SocketServer.staticIsDomination = false;
            SocketServer.staticIsCP = false;

            // Red top score
            playerGameScoreTMP[0].text = SocketServer.staticRedPercent.ToString();
            // End value red
            playerGameScoreTMP[1].text = SocketServer.staticRedPercent.ToString();
            // Blue top score
            playerGameScoreTMP[2].text = SocketServer.staticBluePercent.ToString();
            // End value blue
            playerGameScoreTMP[3].text = SocketServer.staticBluePercent.ToString();
        }

        if (SocketServer.staticIsDomination)
        {
            SocketServer.staticIsCP = false;
            SocketServer.staticIsPayload = false;

            // Red top score
            playerGameScoreTMP[0].text = SocketServer.staticRedPointDom.ToString();
            // End value red
            playerGameScoreTMP[1].text = SocketServer.staticRedPointDom.ToString();
            // Blue top score
            playerGameScoreTMP[2].text = SocketServer.staticBluePointDom.ToString();
            // End value blue
            playerGameScoreTMP[3].text = SocketServer.staticBluePointDom.ToString();

            // need to add a toggle to switch between waterway and quarry
            for (int j = 0; j < 3; j++)
            {
                if (SocketServer.buttonInfoTeams[j] == -1)
                {
                    wwMapParts[j].gameObject.GetComponent<MeshRenderer>().material = m_concrete;
                }
                if (SocketServer.buttonInfoTeams[j] == 0)
                {
                    wwMapParts[j].gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                }
                if (SocketServer.buttonInfoTeams[j] == 1)
                {
                    wwMapParts[j].gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
                }
            }
        }
        if (SocketServer.staticIsCP)
        {
            SocketServer.staticIsPayload = false;
            SocketServer.staticIsDomination = false;

            // Red top score
            playerGameScoreTMP[0].text = SocketServer.staticRedPointCp.ToString();
            // End value red
            playerGameScoreTMP[1].text = SocketServer.staticRedPointCp.ToString();
            // Blue top score
            playerGameScoreTMP[2].text = SocketServer.staticBluePointCp.ToString();
            // End value blue
            playerGameScoreTMP[3].text = SocketServer.staticBluePointCp.ToString();
        }

        // calculate team K, D, HS
        for (int i = 0; i < SocketServer.staticPlayerNamesList.Length; i++)
        {
            if (SocketServer.staticTeamList[i] == 0)
            {
                // sets up a coroutine to spawn the main overlay roster info            
                coroutinePInfo = SpawnPInfo(i, Color.red, 2f);

                // sum up kills, deaths, hs for the team scoreboard
                kTot[0] += SocketServer.staticPlayerKillList[i];
                dTot[0] += SocketServer.staticPlayerDeathList[i];
                hTot[0] += SocketServer.staticHeadShotCounter[i];

                // display main roster info on screen
                StartCoroutine(coroutinePInfo);
            }

            // Same thing as above just for the blue team
            if (SocketServer.staticTeamList[i] == 1)
            {
                coroutinePInfo = SpawnPInfo(i, Color.blue, 2f);

                kTot[1] += SocketServer.staticPlayerKillList[i];
                dTot[1] += SocketServer.staticPlayerDeathList[i];
                hTot[1] += SocketServer.staticHeadShotCounter[i];

                StartCoroutine(coroutinePInfo);
            }

            //playerKDTMP[i].text = "0";
            playerNamesTMP[i].text = playerNamesWithSpaces[i];
            playerKillsTMP[i].text = SocketServer.staticPlayerKillList[i].ToString();
            playerDeathsTMP[i].text = SocketServer.staticPlayerDeathList[i].ToString();
            playerHSTMP[i].text = SocketServer.staticHeadShotCounter[i].ToString();

            if (SocketServer.staticPlayerKillList[i] != 0 && SocketServer.staticPlayerDeathList[i] != 0)
            {
                float k = SocketServer.staticPlayerKillList[i];
                float d = SocketServer.staticPlayerDeathList[i];
                float killf = (k / d);

                playerKDTMP[i].text = killf.ToString("0.0");
            }
            else
            {
                playerKDTMP[i].text = "0";
            }
        }


        for (int i = 0; i < 2; i++)
        {
            playerKillsTotTMP[i].text = kTot[i].ToString();
            playerDeathsTotTMP[i].text = dTot[i].ToString();
            playerHSTotTMP[i].text = hTot[i].ToString();

            if (kTot[i] != 0 && dTot[i] != 0)
            {
                float k = kTot[i];
                float d = dTot[i];
                float killf = (k / d);

                playerKDTotTMP[i].text = killf.ToString("0.0");
            }
            else
            {
                playerKDTotTMP[i].text = "0";
            }

        }
    }

    // Populate player info for main screen rosters and top screen scoreboard. 
    IEnumerator SpawnPInfo(int i, Color _topBorder, float _waitTime)
    {

        foreach (Transform child in playerInfoHolderParent.transform)
        {
            if (child.name == ("pInfo_" + i.ToString()))
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
            GameObject pInfo = Instantiate(playerInfoHolder, playerInfoHolderParent.transform);
            pInfo.name = "pInfo_" + i.ToString();
            pInfo.transform.GetChild(0).GetComponent<Image>().color = _topBorder;

            pInfo.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = SocketServer.staticPlayerNamesList[i];
            pInfo.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = SocketServer.staticPlayerKillList[i].ToString();
            pInfo.transform.GetChild(0).GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>().text = SocketServer.staticPlayerDeathList[i].ToString();
            pInfo.transform.GetChild(0).GetChild(1).GetChild(5).GetComponent<TextMeshProUGUI>().text = SocketServer.staticHeadShotCounter[i].ToString();

            yield return new WaitForSeconds(_waitTime);
            Destroy(pInfo);
            isNameInfoShowing[i] = false;

            // below is setup for the horizontal names list for prefab P_namesColorTop
            //GameObject pInfo = Instantiate(playerInfoHolder, playerInfoHolderParent.transform);
            //pInfo.name = "pInfo_" + i.ToString();
            //pInfo.transform.GetChild(0).GetComponent<Image>().color = _topBorder;

            //pInfo.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = SocketServer.staticPlayerNamesList[i];
            //pInfo.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = SocketServer.staticPlayerKillList[i].ToString();
            //pInfo.transform.GetChild(0).GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>().text = SocketServer.staticPlayerDeathList[i].ToString();
            //pInfo.transform.GetChild(0).GetChild(1).GetChild(5).GetComponent<TextMeshProUGUI>().text = SocketServer.staticHeadShotCounter[i].ToString();

            //yield return new WaitForSeconds(_waitTime);
            //Destroy(pInfo);
            //isNameInfoShowing[i] = false;
        }
    }

    public void ResetNameInfo()
    {
        playerNamesWithSpaces.Clear();
        isNameInfoShowing.Clear();
        kTot.Clear();
        dTot.Clear();
        hTot.Clear();

        for (int i = 0; i < 10; i++)
        {
            playerNamesWithSpaces.Add("not name");
        }
        for (int i = 0; i < 10; i++)
        {
            isNameInfoShowing.Add(false);
        }
        for (int i = 0; i < 2; i++)
        {
            kTot.Add(0);
            dTot.Add(0);
            hTot.Add(0);
        }
    }
}