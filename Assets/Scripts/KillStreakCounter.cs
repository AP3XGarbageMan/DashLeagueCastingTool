//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;
//using System.Linq;

//public class KillStreakCounter : MonoBehaviour
//{
//    private List<string> playerNamesWithSpaces = new List<string>();

//    [SerializeField]
//    private GameObject[] playerMainUIInfoHolder;
//    [SerializeField]
//    private GameObject[] wwMapParts;
//    [SerializeField]
//    private GameObject wwMapParent;
//    [SerializeField]
//    private GameObject[] quarryMapParts;
//    [SerializeField]
//    private GameObject quarryMapParent;
//    [SerializeField]
//    private GameObject domTeamColorFlash;

//    [SerializeField]
//    private GameObject[] payloadCartLights;
//    [SerializeField]
//    private GameObject payloadCartLightsHolder;

//    // red 0, 1 | blue 2, 3 || Fills out both main top scores then main scoreboard
//    [SerializeField]
//    private TextMeshProUGUI[] playerGameScoreTMP;
//    // red 0, 1 | blue 2, 3 || Fills out both main screen rosters and main scoreboard
//    [SerializeField]
//    private TextMeshProUGUI[] teamNamesTMP;

//    // all for main scoreboard... values are named numerically
//    // player names main scoreboard
//    [SerializeField]
//    private TextMeshProUGUI[] playerNamesScoreboardTMP;
//    [SerializeField]
//    private TextMeshProUGUI[] playerKillsTMP;
//    [SerializeField]
//    private TextMeshProUGUI[] playerDeathsTMP;
//    [SerializeField]
//    private TextMeshProUGUI[] playerKDTMP;
//    [SerializeField]
//    private TextMeshProUGUI[] playerHSTMP;
//    [SerializeField]
//    private TextMeshProUGUI[] playerKillsTotTMP;
//    [SerializeField]
//    private TextMeshProUGUI[] playerDeathsTotTMP;
//    [SerializeField]
//    private TextMeshProUGUI[] playerKDTotTMP;
//    [SerializeField]
//    private TextMeshProUGUI[] playerHSTotTMP;

//    [SerializeField]
//    private Transform[] verticalRosterHolder;

//    private List<int> kTot = new List<int>();
//    private List<int> dTot = new List<int>();
//    private List<int> hTot = new List<int>();

//    private List<bool> isNameInfoShowing = new List<bool>();

//    public string[] cutTeamNames;

//    private IEnumerator coroutinePInfo;

//    [SerializeField]
//    private Material m_blueBase;
//    [SerializeField]
//    private Material m_redBase;
//    [SerializeField]
//    private Material m_concrete;

//    private Color c_teamBlue = new Color32(8, 135, 255, 255);
//    private Color c_teamRed = new Color32(240, 14, 52, 255);


//    private bool isRedCountDown = false;
//    private bool isBlueCountDown = false;

//    [SerializeField]
//    private Toggle wwToggle;
//    [SerializeField]
//    private Toggle quarryToggle;
//    [SerializeField]
//    private Toggle payloadSwitchToggle;


//    public void Start()
//    {
//        payloadCartLightsHolder.SetActive(false);
//        wwMapParent.SetActive(false);
//        quarryMapParent.SetActive(false);
//        domTeamColorFlash.SetActive(false);

//        for (int i = 0; i < 4; i++)
//        {
//            playerGameScoreTMP[i].text = "0";
//        }

//        for (int i = 0; i < 10; i++)
//        {
//            playerNamesWithSpaces.Add("not name");
//        }
//        for (int i = 0; i < 10; i++)
//        {
//            isNameInfoShowing.Add(false);
//        }
//        for (int i = 0; i < 2; i++)
//        {
//            kTot.Add(0);
//            dTot.Add(0);
//            hTot.Add(0);
//        }
//    }

//    public void Update()
//    {
//        if (SocketServer.staticReadingData)
//        {
//            UpdateData();
//        }

//        if (isBlueCountDown)
//        {
//            domTeamColorFlash.gameObject.SetActive(true);
//            domTeamColorFlash.GetComponent<Image>().color = c_teamBlue;
//        }
//        if (isRedCountDown)
//        {
//            domTeamColorFlash.gameObject.SetActive(true);
//            domTeamColorFlash.GetComponent<Image>().color = c_teamRed;
//        }

//        if (!isBlueCountDown && !isRedCountDown)
//        {
//            domTeamColorFlash.gameObject.SetActive(false);
//        }
//    }

//    public void UpdateData()
//    {
//        // setup player names
//        for (int i = 0; i < SocketServer.staticPlayerNamesList.Length; i++)
//        {
//            // setup for [team] [name name]
//            string[] splitNames = SocketServer.staticPlayerNamesList[i].Split();

//            if (splitNames.Length > 1)
//            {
//                if (payloadSwitchToggle.isOn)
//                {
//                    if (i < 5)
//                    {
//                        teamNamesTMP[2].text = splitNames[0];
//                        teamNamesTMP[3].text = splitNames[0];
//                    }
//                    // blue team
//                    if (i > 4)
//                    {
//                        teamNamesTMP[0].text = splitNames[0];
//                        teamNamesTMP[1].text = splitNames[0];
//                    }
//                }
//                else
//                {
//                    // [team] setup
//                    // red team
//                    if (i < 5)
//                    {
//                        teamNamesTMP[0].text = splitNames[0];
//                        teamNamesTMP[1].text = splitNames[0];
//                    }
//                    // blue team
//                    if (i > 4)
//                    {
//                        teamNamesTMP[2].text = splitNames[0];
//                        teamNamesTMP[3].text = splitNames[0];
//                    }
//                }
//                // [name with spaces] setup
//                string nameWithSpaces = "";
//                for (int j = 1; j < splitNames.Length; j++)
//                {
//                    nameWithSpaces += splitNames[j];
//                }

//                playerNamesWithSpaces[i] = nameWithSpaces;
//            }
//            else
//            {
//                playerNamesWithSpaces[i] = SocketServer.staticPlayerNamesList[i];
//            }
//        }

//        // clear kill, death, headshot counters
//        for (int i = 0; i < 2; i++)
//        {
//            kTot[i] = 0;
//            dTot[i] = 0;
//            hTot[i] = 0;
//        }

//        // update main scoreboard
//        // send the right data to the right place for payload, dom, and CP
//        if (SocketServer.staticIsPayload)
//        {
//            SocketServer.staticIsDomination = false;
//            wwMapParent.SetActive(false);
//            SocketServer.staticIsCP = false;
//            payloadCartLightsHolder.SetActive(true);
//            // Red top score
//            playerGameScoreTMP[0].text = SocketServer.staticRedPercent.ToString();
//            // End value red
//            playerGameScoreTMP[1].text = SocketServer.staticRedPercent.ToString();
//            // Blue top score
//            playerGameScoreTMP[2].text = SocketServer.staticBluePercent.ToString();
//            // End value blue
//            playerGameScoreTMP[3].text = SocketServer.staticBluePercent.ToString();


//            if (SocketServer.staticPlayerOnCart == 0)
//            {
//                for (int i = 0; i < 3; i++)
//                {
//                    payloadCartLights[i].GetComponent<Image>().color = Color.black;
//                }
//            }
//            if (SocketServer.staticPlayerOnCart == 1)
//            {
//                payloadCartLights[0].GetComponent<Image>().color = Color.white;
//                payloadCartLights[1].GetComponent<Image>().color = Color.black;
//                payloadCartLights[2].GetComponent<Image>().color = Color.black;
//            }
//            if (SocketServer.staticPlayerOnCart == 2)
//            {
//                for (int i = 0; i < 2; i++)
//                {
//                    payloadCartLights[i].GetComponent<Image>().color = Color.white;
//                }
//                payloadCartLights[2].GetComponent<Image>().color = Color.black;
//            }
//            if (SocketServer.staticPlayerOnCart == 3)
//            {
//                for (int i = 0; i < 3; i++)
//                {
//                    payloadCartLights[i].GetComponent<Image>().color = Color.white;
//                }
//            }

//        }

//        if (SocketServer.staticIsDomination)
//        {
//            if (wwToggle.isOn)
//            {
//                wwMapParent.SetActive(true);
//                quarryMapParent.SetActive(false);
//            }
//            if (quarryToggle.isOn)
//            {
//                quarryMapParent.SetActive(true);
//                wwMapParent.SetActive(false);

//            }

//            SocketServer.staticIsCP = false;
//            SocketServer.staticIsPayload = false;
//            payloadCartLightsHolder.SetActive(false);

//            // Red top score
//            playerGameScoreTMP[0].text = SocketServer.staticRedPointDom.ToString();
//            // End value red
//            playerGameScoreTMP[1].text = SocketServer.staticRedPointDom.ToString();
//            // Blue top score
//            playerGameScoreTMP[2].text = SocketServer.staticBluePointDom.ToString();
//            // End value blue
//            playerGameScoreTMP[3].text = SocketServer.staticBluePointDom.ToString();

//            int domButtonCountRed = 0;
//            int domButtonCountBlue = 0;

//            for (int j = 0; j < 3; j++)
//            {
//                if (wwMapParent.activeInHierarchy == true)
//                {
//                    if (SocketServer.buttonInfoTeams[j] == -1)
//                    {
//                        isRedCountDown = false;
//                        isBlueCountDown = false;

//                        wwMapParts[j].gameObject.GetComponent<MeshRenderer>().material = m_concrete;
//                    }
//                    if (SocketServer.buttonInfoTeams[j] == 0)
//                    {
//                        isBlueCountDown = false;
//                        wwMapParts[j].gameObject.GetComponent<MeshRenderer>().material.color = c_teamRed;
//                        domButtonCountRed++;
//                    }
//                    if (SocketServer.buttonInfoTeams[j] == 1)
//                    {
//                        isRedCountDown = false;
//                        wwMapParts[j].gameObject.GetComponent<MeshRenderer>().material.color = c_teamBlue;
//                        domButtonCountBlue++;
//                    }
//                }
//                if (quarryMapParent.activeInHierarchy == true)
//                {
//                    if (SocketServer.buttonInfoTeams[j] == -1)
//                    {
//                        isRedCountDown = false;
//                        isBlueCountDown = false;

//                        quarryMapParts[j].gameObject.GetComponent<MeshRenderer>().material = m_concrete;
//                    }
//                    if (SocketServer.buttonInfoTeams[j] == 0)
//                    {
//                        isBlueCountDown = false;
//                        quarryMapParts[j].gameObject.GetComponent<MeshRenderer>().material.color = c_teamRed;
//                        domButtonCountRed++;
//                    }
//                    if (SocketServer.buttonInfoTeams[j] == 1)
//                    {
//                        isRedCountDown = false;
//                        quarryMapParts[j].gameObject.GetComponent<MeshRenderer>().material.color = c_teamBlue;
//                        domButtonCountBlue++;
//                    }
//                }

//                if (domButtonCountRed == 3)
//                {
//                    isRedCountDown = true;
//                }
//                if (domButtonCountBlue == 3)
//                {
//                    isBlueCountDown = true;
//                }
//            }
//        }
//        if (SocketServer.staticIsCP)
//        {
//            SocketServer.staticIsPayload = false;
//            payloadCartLightsHolder.SetActive(false);
//            SocketServer.staticIsDomination = false;
//            wwMapParent.SetActive(false);
      

//            // Red top score
//            playerGameScoreTMP[0].text = SocketServer.staticRedPointCp.ToString();
//            // End value red
//            playerGameScoreTMP[1].text = SocketServer.staticRedPointCp.ToString();
//            // Blue top score
//            playerGameScoreTMP[2].text = SocketServer.staticBluePointCp.ToString();
//            // End value blue
//            playerGameScoreTMP[3].text = SocketServer.staticBluePointCp.ToString();
//        }

//        // calculate team K, D, HS
//        for (int i = 0; i < SocketServer.staticPlayerNamesList.Length; i++)
//        {
//            if (SocketServer.staticTeamList[i] == 0)
//            {
//                // sets up a coroutine to spawn the main overlay roster info            
//                coroutinePInfo = SpawnPInfo(i, Color.red, 2f, 0);

//                // sum up kills, deaths, hs for the team scoreboard
//                kTot[0] += SocketServer.staticPlayerKillList[i];
//                dTot[0] += SocketServer.staticPlayerDeathList[i];
//                hTot[0] += SocketServer.staticHeadShotCounter[i];

//                // display main roster info on screen
//                StartCoroutine(coroutinePInfo);
//            }

//            // Same thing as above just for the blue team
//            if (SocketServer.staticTeamList[i] == 1)
//            {
//                coroutinePInfo = SpawnPInfo(i, Color.blue, 2f, 1);

//                kTot[1] += SocketServer.staticPlayerKillList[i];
//                dTot[1] += SocketServer.staticPlayerDeathList[i];
//                hTot[1] += SocketServer.staticHeadShotCounter[i];

//                StartCoroutine(coroutinePInfo);
//            }

//            //playerKDTMP[i].text = "0";
//            playerNamesScoreboardTMP[i].text = playerNamesWithSpaces[i];
//            playerKillsTMP[i].text = SocketServer.staticPlayerKillList[i].ToString();
//            playerDeathsTMP[i].text = SocketServer.staticPlayerDeathList[i].ToString();
//            playerHSTMP[i].text = SocketServer.staticHeadShotCounter[i].ToString();

//            if (SocketServer.staticPlayerKillList[i] != 0 && SocketServer.staticPlayerDeathList[i] != 0)
//            {
//                float k = SocketServer.staticPlayerKillList[i];
//                float d = SocketServer.staticPlayerDeathList[i];
//                float killf = (k / d);

//                playerKDTMP[i].text = killf.ToString("0.0");
//            }
//            else
//            {
//                playerKDTMP[i].text = "0";
//            }
//        }


//        for (int i = 0; i < 2; i++)
//        {
//            playerKillsTotTMP[i].text = kTot[i].ToString();
//            playerDeathsTotTMP[i].text = dTot[i].ToString();
//            playerHSTotTMP[i].text = hTot[i].ToString();

//            if (kTot[i] != 0 && dTot[i] != 0)
//            {
//                float k = kTot[i];
//                float d = dTot[i];
//                float killf = (k / d);

//                playerKDTotTMP[i].text = killf.ToString("0.0");
//            }
//            else
//            {
//                playerKDTotTMP[i].text = "0";
//            }

//        }
//    }

//    // Populate player info for main screen rosters and top screen scoreboard. 
//    IEnumerator SpawnPInfo(int i, Color _topBorder, float _waitTime, int _team)
//    {

//        foreach (Transform child in verticalRosterHolder[_team])
//        {
//            if (child.name == ("pInfo_R_" + i.ToString()) || child.name == ("pInfo_B_" + i.ToString()))
//            {
//                isNameInfoShowing[i] = true;
//            }
//        }

//        if (isNameInfoShowing[i])
//        {
//            //Debug.Log("already showing");
//        }
//        else
//        {
//            GameObject pInfo = Instantiate(playerMainUIInfoHolder[_team], verticalRosterHolder[_team]);

//            if (KillFeed.isDead[i])
//            {
//                pInfo.transform.GetChild(1).gameObject.SetActive(true);
//            }

//            pInfo.name = "pInfo_R" + i.ToString();
//            if (_team == 0)
//            {
//                //pInfo.transform.GetChild(0).GetComponent<Image>().material = m_redRosterBackground;
//                pInfo.name = "pInfo_R_" + i.ToString();
//                pInfo.transform.GetChild(0).GetChild(7).GetComponent<TextMeshProUGUI>().text = playerNamesWithSpaces[i];
//                pInfo.transform.GetChild(0).GetChild(3).GetComponent<TextMeshProUGUI>().text = SocketServer.staticPlayerKillList[i].ToString();
//                pInfo.transform.GetChild(0).GetChild(6).GetComponent<TextMeshProUGUI>().text = SocketServer.staticPlayerDeathList[i].ToString();
//                //pInfo.transform.GetChild(0).GetChild(1)..GetComponent<TextMeshProUGUI>().text = SocketServer.staticHeadShotCounter[i].ToString();
//            }
//            else
//            {
//                //pInfo.transform.GetChild(0).GetComponent<Image>().material = m_blueRosterBackground;
//                pInfo.name = "pInfo_B_" + i.ToString();
//                pInfo.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = playerNamesWithSpaces[i];
//                pInfo.transform.GetChild(0).GetChild(4).GetComponent<TextMeshProUGUI>().text = SocketServer.staticPlayerKillList[i].ToString();
//                pInfo.transform.GetChild(0).GetChild(7).GetComponent<TextMeshProUGUI>().text = SocketServer.staticPlayerDeathList[i].ToString();
//                //pInfo.transform.GetChild(0).GetChild(1)..GetComponent<TextMeshProUGUI>().text = SocketServer.staticHeadShotCounter[i].ToString();
//            }

//            yield return new WaitForSeconds(_waitTime);
//            Destroy(pInfo);
//            isNameInfoShowing[i] = false;

//            // below is somewhat setup for the horizontal names list for prefab P_namesColorTop

//            //GameObject pInfo = Instantiate(playerInfoHolder, playerInfoHolderParent.transform);
//            //pInfo.name = "pInfo_" + i.ToString();
//            //pInfo.transform.GetChild(0).GetComponent<Image>().color = _topBorder;

//            //pInfo.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = SocketServer.staticPlayerNamesList[i];
//            //pInfo.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = SocketServer.staticPlayerKillList[i].ToString();
//            //pInfo.transform.GetChild(0).GetChild(1).GetChild(3).GetComponent<TextMeshProUGUI>().text = SocketServer.staticPlayerDeathList[i].ToString();
//            //pInfo.transform.GetChild(0).GetChild(1).GetChild(5).GetComponent<TextMeshProUGUI>().text = SocketServer.staticHeadShotCounter[i].ToString();

//            //yield return new WaitForSeconds(_waitTime);
//            //Destroy(pInfo);
//            //isNameInfoShowing[i] = false;
//        }
//    }


//    public void ResetNameInfo()
//    {
//        playerNamesWithSpaces.Clear();
//        isNameInfoShowing.Clear();
//        kTot.Clear();
//        dTot.Clear();
//        hTot.Clear();

//        for (int i = 0; i < 10; i++)
//        {
//            playerNamesWithSpaces.Add("not name");
//        }
//        for (int i = 0; i < 10; i++)
//        {
//            isNameInfoShowing.Add(false);
//        }
//        for (int i = 0; i < 2; i++)
//        {
//            kTot.Add(0);
//            dTot.Add(0);
//            hTot.Add(0);
//        }
//    }
//}