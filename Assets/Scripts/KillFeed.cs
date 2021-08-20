using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Linq;

public class KillFeed : MonoBehaviour
{

    public static List<int> previousKills = new List<int>();
    public static List<int> previousDeaths = new List<int>();
    public static List<int> currentKills = new List<int>();
    public static List<int> currentDeaths = new List<int>();
    public static List<int> currentKillStreak = new List<int>();
    public static List<int> HighestKillStreak = new List<int>();

    public static List<bool> isStreaking = new List<bool>();
    // get off my back
    public static List<bool> isDead = new List<bool>();

    public static List<string> playerNames = new List<string>();

    private IEnumerator coroutineKillFeed;
    private IEnumerator coroutineDeathBar;

    public int numActiveKillFeeds = 0;

    [SerializeField]
    private GameObject kfGO;

    public GameObject stashGO;

    [SerializeField]
    private Transform kfParent;

    [SerializeField]
    private List<Sprite> weaponIcon = new List<Sprite>();

    public static Color colorBlue = new Color32(8, 135, 255, 255);
    public static Color colorRed = new Color32(240, 14, 52, 255);
    public static Color killerColor;
    public static Color victimColor;
    public static string victim = "";
    public static string killer = "";
    public static bool wasHeadShot = false;
    public static int gunType = 0;

    [SerializeField]
    private Toggle kfBack;

    public void Start()
    {
        // populate lists for first maths
        for (int i = 0; i < 10; i++)
        {
            currentKillStreak.Add(0);
            currentKills.Add(0);
            currentDeaths.Add(0);
            previousKills.Add(0);
            previousDeaths.Add(0);
            HighestKillStreak.Add(0);
            playerNames.Add("name");
        }

        Debug.Log("previous deaths count start = " + previousDeaths.Count.ToString());

        for (int i = 0; i < 10; i++)
        {
            isStreaking.Add(false);
            isDead.Add(false);
        }
    }

    public void GetDataFromSocketServer(string _dataType, string _killer, string _victim, bool _isHeadShot, int _gunType)
    {
        Debug.Log("made it from SS to dead");
        // updates dead information
        // victum, killer, headshot, isaltfire, weaponstype, type, timestamp
        if (_dataType == "Dead")
        {
            Debug.Log("made it within the dead zone");
            victim = _victim;
            killer = _killer;
            wasHeadShot = _isHeadShot;
            gunType = _gunType;           
            SpawnKF(killer, victim, wasHeadShot, gunType);
        }
    }

    public void GetDataFromSocketServer(string _dataType, int[] _kills, int[] _deaths, string[] _playerNames)
    {
        Debug.Log("made it from SS to SB");
        // updates dead information
        // updates scoreboard information
        if (_dataType == "ScoreBoard")
        {
            SetPreviousDeaths();

            // ids, levels, names, teams, kills, deaths, scores, type, timestamp
            for (int i = 0; i < _playerNames.Length; i++)
            {
                currentKills[i] = _kills[i];
                currentDeaths[i] = _deaths[i];
                playerNames[i] = _playerNames[i];
            }

            CheckKillStreak();
        }
    }

    // called before updating kills/deaths
    public void SetPreviousDeaths()
    {
        for (int i = 0; i < playerNames.Count; i++)
        {
            previousDeaths[i] = currentDeaths[i];
            previousKills[i] = currentKills[i];
        }      
    }

    IEnumerator SpawnDB(float _wait, int _pos)
    {
        Debug.Log("in the spawn db coroutine");
        yield return new WaitForSeconds(_wait);
        //isDead[_pos] = false;
    }

    public void CheckKillStreak()
    {
        // setup kill streak
        // check if a player died. If so set kill streak to 0 and turn off bool for streaking
        for (int i = 0; i < currentKills.Count; i++)
        {
            if (currentDeaths[i] > previousDeaths[i])
            {
                Debug.Log("setting up is dead");
                SpawnKF skf = new SpawnKF();
                skf.StartTheCO();
                isDead[i] = true;
                currentKillStreak[i] = 0;
                isStreaking[i] = false;
                Debug.Log("should be starting a kill bar coroutine");
                coroutineDeathBar = SpawnDB(4.0f, i);
                StartCoroutine(coroutineDeathBar);
            }
            if (currentKills[i] > previousKills[i])
            {
                int math = (currentKillStreak[i] + (currentKills[i] - previousKills[i]));

                currentKillStreak[i] = math;

                if (currentKillStreak[i] > 2)
                {
                    isStreaking[i] = true;
                }
                SetHighKillStreak(i);
            }
        }
    }

    // check if streaking, set high ks
    public void SetHighKillStreak(int _i)
    {
        if (isStreaking[_i])
        {
            Debug.Log("xomone is streaking");
            if (currentKillStreak[_i] > HighestKillStreak[_i])
            {
                HighestKillStreak[_i] = currentKillStreak[_i];
            }
        }
        Debug.Log("finished set high kill streak");
    }

    public void SpawnKF(string _killer, string _victim, bool _isHeadShot, int _gunType)
    {
        Debug.Log("made it from first call");

        float wait = 2.0f;
        int fixedWeapon = 0;

        switch (_gunType)
        {
            case 0:
                fixedWeapon = 0;
                break;
            case 6:
                fixedWeapon = 1;
                break;
            case 4:
                fixedWeapon = 2;
                break;
            case 5:
                fixedWeapon = 7;
                break;
            case 8:
                fixedWeapon = 8;
                break;
            case 7:
                fixedWeapon = 9;
                break;
            case 2:
                fixedWeapon = 11;
                break;
        }

        Debug.Log("made it to second call");
        coroutineKillFeed = SpawnKFcr(fixedWeapon);
        Debug.Log("made it to third call");
        StartCoroutine(coroutineKillFeed);
        Debug.Log("made it to fourth call");
    }

    private IEnumerator SpawnKFcr(int _fixedWeapon)
    {
        
        Debug.Log("some one died and I should be showing you that");

        //CheckColors(killer, victim);

        //if (numActiveKillFeeds == 4)
        //{
        //    numActiveKillFeeds = 0;
        //}

        //// instantiate kill feed
        //GameObject kfTextGO = Instantiate(kfGO, kfParent);
        //kfTextGO.name = killer + "_kf";

        ////// check if player is streaking. If so, set the streaking parent to active
        ////for (int i = 0; i < isStreaking.Count; i++)
        ////{
        ////    if (playerNames[i] == killer)
        ////    {
        ////        if (isStreaking[i])
        ////        {
        ////            kfTextGO.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = currentKillStreak[i].ToString();
        ////        }
        ////        else
        ////        {
        ////            kfTextGO.transform.GetChild(0).gameObject.SetActive(false);
        ////        }
        ////    }
        ////}

        //// setup prefab
        //kfTextGO.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = killer;
        //kfTextGO.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = killerColor;
        //kfTextGO.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = weaponIcon[_fixedWeapon];
        //kfTextGO.transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().text = victim;
        //kfTextGO.transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().color = victimColor;

        //// if its a headshot make the gun red
        //if (wasHeadShot)
        //{
        //    kfTextGO.transform.GetChild(1).GetChild(2).GetComponent<Image>().color = Color.red;
        //}

        //// toggle the background
        //if (!kfBack.isOn)
        //{
        //    kfTextGO.GetComponent<Image>().enabled = false;
        //}

        yield return new WaitForSeconds(2.0f);

        //Destroy(kfTextGO);

        //// keep track to keep under 4 at a time on screen
        //numActiveKillFeeds++;
    }


    //public void CheckColors(string _killer, string _victim)
    //{
    //    for (int i = 0; i < SetupPlayerNames.staticPlayerNames.Length; i++)
    //    {
    //        if (SetupPlayerNames.staticPlayerNames[i] == _killer)
    //        {
    //            if (i < 5)
    //            {
    //                killerColor = colorRed;
    //            }
    //            if (i > 4)
    //            {
    //                killerColor = colorBlue;
    //            }
    //        }
    //    }

    //    for (int i = 0; i < SetupPlayerNames.staticPlayerNames.Length; i++)
    //    {
    //        if (SetupPlayerNames.staticPlayerNames[i] == _victim)
    //        {
    //            if (i < 5)
    //            {
    //                victimColor = colorRed;
    //            }
    //            if (i > 4)
    //            {
    //                victimColor = colorBlue;
    //            }
    //        }
    //    }
    //}

    //public void ResetDataKF()
    //{
    //    currentKillStreak.Clear();
    //    currentKills.Clear();
    //    currentDeaths.Clear();
    //    previousKills.Clear();
    //    previousDeaths.Clear();
    //    HighestKillStreak.Clear();
    //    isStreaking.Clear();

    //    // populate lists for first maths
    //    for (int i = 0; i < 10; i++)
    //    {
    //        currentKillStreak.Add(0);
    //        currentKills.Add(0);
    //        currentDeaths.Add(0);
    //        previousKills.Add(0);
    //        previousDeaths.Add(0);
    //        HighestKillStreak.Add(0);

    //    }
    //    for (int i = 0; i < 10; i++)
    //    {
    //        isStreaking.Add(false);
    //    }
    //}
}
