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
//    private TextMeshProUGUI[] playerKillsTotTMP;
//    [SerializeField]
//    private TextMeshProUGUI[] playerDeathsTotTMP;
//    [SerializeField]
//    private TextMeshProUGUI[] playerKDTotTMP;
//    [SerializeField]
//    private TextMeshProUGUI[] playerHSTotTMP;



//    private List<int> kTot = new List<int>();
//    private List<int> dTot = new List<int>();
//    private List<int> hTot = new List<int>();



//    public string[] cutTeamNames;



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





//    public void ResetNameInfo()
//    {
//        playerNamesWithSpaces.Clear();
//        kTot.Clear();
//        dTot.Clear();
//        hTot.Clear();

//        for (int i = 0; i < 10; i++)
//        {
//            playerNamesWithSpaces.Add("not name");
//        }
//        for (int i = 0; i < 2; i++)
//        {
//            kTot.Add(0);
//            dTot.Add(0);
//            hTot.Add(0);
//        }
//    }
//}