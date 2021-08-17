using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Linq;

public class KillFeed : MonoBehaviour
{
    private int[] previousKills = new int[10];
    private int[] previousDeaths = new int[10];
    private int[] currentKills = new int[10];
    private int[] currentDeaths = new int[10];
    private int[] currentKillStreak = new int[10];

    public int[] teamNumList = new int[10];
    public int[] teamHSList = new int[10];
    public int[] teamKillsList = new int[10];
    public int[] teamDeathsList = new int[10];

    public string[] playerNames = new string[10];

    private int[] HighestKillStreak = new int[10];

    private bool[] isStreaking = new bool[10];

    private IEnumerator coroutineKillFeed;
    //private IEnumerator coroutineDeathBar;


    private int numActiveKillFeeds = 0;

    [SerializeField]
    private GameObject kfGO;

    [SerializeField]
    private GameObject[] verticalSBPlayers;

    [SerializeField]
    private Transform kfParent;

    [SerializeField]
    private List<Sprite> weaponIcon = new List<Sprite>();

    //private Color colorBlue = new Color32(8, 135, 255, 255);
    //private Color colorRed = new Color32(240, 14, 52, 255);
    private Color killerColor;
    private Color victumColor;

    [SerializeField]
    private Toggle kfBack;

    private void Start()
    {
        // populate lists for first maths
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("Setting up lists");

            currentKillStreak[i] = 0;
            currentKills[i] = 0;
            currentDeaths[i] = 0;
            previousKills[i] = 0;
            previousDeaths[i] = 0;
            HighestKillStreak[i] = 0;
            teamNumList[i] = 0;
            teamHSList[i] = 0;
            playerNames[i] = ("name");
            Debug.Log("player name is = " + playerNames[i]);
        }
        for (int i = 0; i < 10; i++)
        {
            isStreaking[i] = (false);
        }
        for (int i = 0; i < 2; i++)
        {
            teamDeathsList[i] = 0;
            teamKillsList[i] = 0;
            teamHSList[i] = 0;
        }
    }


    // from SB event
    public void GetSBEvent(Root data)
    {
        Debug.Log("size of data pool = " + data.Data.Names.Length.ToString());
        Debug.Log("data.Data.Name[j] = " + data.Data.Names[2]);
        // fill with data before we update
        //SetPreviousDeaths();
        Debug.Log("data.Data.Name[j] = " + data.Data.Names[4]);
        for (int j = 0; j < 10; j++)
        {
            Debug.Log("data.Data.Name[j] = " + data.Data.Names[j]);
            Debug.Log("playerName[j] = " + playerNames[j]);
            playerNames[j] = data.Data.Names[j];
            Debug.Log("data.Data.Name[j] again = " + data.Data.Names[j]);
            Debug.Log("playerNames[j] = " + playerNames[j]);
            teamKillsList[j] = data.Data.Kills[j];
            teamDeathsList[j] = data.Data.Deaths[j];
            teamNumList[j] = data.Data.Teams[j];
        }
    }

    // set previous kills/deaths list from current
    public void SetPreviousDeaths()
    {
        Debug.Log("Made it to set previous deaths in kf ");
        Debug.Log("current kills count = " + currentKills.Length.ToString());
        for (int i = 0; i < 10; i++)
        {
            Debug.Log("previous deaths = " + previousDeaths[i]);
            previousDeaths[i] = teamDeathsList[i];
            previousKills[i] = teamKillsList[i];
        }
    }


    //IEnumerator SpawnDB(float _wait, int _pos)
    //{
    //    yield return new WaitForSeconds(_wait);
    //    isDead[_pos] = false;
    //}

    //public void GetDeath()
    //{
    //    // setup kill streak
    //    // check if a player died. If so set kill streak to 0 and turn off bool for streaking
    //    for (int i = 0; i < SocketServer.staticPlayerDeathList.Length; i++)
    //    {
    //        if (SocketServer.staticPlayerDeathList[i] > previousDeaths[i])
    //        {
    //            currentKillStreak[i] = 0;
    //            isStreaking[i] = false;
    //        }
    //    }

    //    // check if a kill happened. If so, spawn a kill feed
    //    if (SocketServer.staticKillHappened)
    //    {
    //        // reset kill happened bool
    //        SocketServer.staticKillHappened = false;
    //        // setup kill feed prefab
    //        SpawnKF();
    //    }
    //}


    public void SpawnKF(Root data)
    {
        Debug.Log("Made it to spawn kf ");

        float wait = 2.0f;
        int fixedWeapon = 0;

        if (data.Data.WeaponsType == 0)
        {
            fixedWeapon = 0;
        }
        if (data.Data.WeaponsType == 1)
        {
            fixedWeapon = 6;
        }
        if (data.Data.WeaponsType == 2)
        {
            fixedWeapon = 4;
        }
        if (data.Data.WeaponsType == 7)
        {
            fixedWeapon = 5;
        }
        if (data.Data.WeaponsType == 8)
        {
            fixedWeapon = 8;
        }
        if (data.Data.WeaponsType == 9)
        {
            fixedWeapon = 7;
        }
        if (data.Data.WeaponsType == 11)
        {
            fixedWeapon = 2;
        }

        Debug.Log("setting up kf coroutine");
        string _killer = data.Data.Killer;
        string _victim = data.Data.Victum;
        coroutineKillFeed = SpawnKF(wait, fixedWeapon, _killer, _victim);
        Debug.Log("starting kf coroutine");


        StartCoroutine(coroutineKillFeed);
    }

    IEnumerator SpawnKF(float _wait, int _weaponType, string _killer, string _victim)
    {
        Debug.Log("Made it to spawn kf numerator");

        CheckColors(_killer, _victim);
        //CheckColors(_data.Data.Killer, _data.Data.Victum);
        CheckKillStreak();


        if (numActiveKillFeeds == 4)
        {
            numActiveKillFeeds = 0;
        }

        // instantiate kill feed
        GameObject kfTextGO = Instantiate(kfGO, kfParent);
        kfTextGO.name = _killer + "_kf";

        // check if player is streaking. If so, set the streaking parent to active
        for (int i = 0; i < isStreaking.Length; i++)
        {
            if (playerNames[i] == _killer)
            {
                if (isStreaking[i])
                {
                    kfTextGO.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = currentKillStreak[i].ToString();
                }
                else
                {
                    kfTextGO.transform.GetChild(0).gameObject.SetActive(false);
                }
            }
        }

        kfTextGO.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _killer;
        kfTextGO.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = killerColor;
        kfTextGO.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = weaponIcon[_weaponType];
        kfTextGO.transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().text = _victim;
        kfTextGO.transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().color = victumColor;

        //if (_data.Data.HeadShot)
        //{
        //    kfTextGO.transform.GetChild(1).GetChild(2).GetComponent<Image>().color = Color.red;
        //}

        if (!kfBack.isOn)
        {
            kfTextGO.GetComponent<Image>().enabled = false;
        }

        yield return new WaitForSeconds(_wait);
        Destroy(kfTextGO);
        numActiveKillFeeds++;
    }


    public void CheckColors(string _killer, string _victum)
    {
        Debug.Log("Made it to check colors ");

        for (int i = 0; i < playerNames.Length; i++)
        {
            if (playerNames[i] == _killer)
            {
                if (i < 5)
                {
                    killerColor = Color.red;
                }
                if (i > 4)
                {
                    killerColor = Color.blue;
                }
            }

            if (playerNames[i] == _victum)
            {
                if (i < 5)
                {
                    victumColor = Color.red;
                }
                if (i > 4)
                {
                    victumColor = Color.blue;
                }
            }
        }
    }

    public void CheckKillStreak()
    {
        for (int i = 0; i < currentKills.Length; i++)
        {
            if (currentKills[i] > previousKills[i])
            {
                int math = (currentKillStreak[i] + (currentKills[i] - previousKills[i]));

                currentKillStreak[i] = math;

                if (currentKillStreak[i] > 2)
                {
                    Debug.Log(playerNames[i] + " is on a streak = " + math.ToString());
                    isStreaking[i] = true;
                }

                SetHighKillStreak(i);
            }
        }

    }

    public void SetHighKillStreak(int _i)
    {
        if (isStreaking[_i])
        {
            if (currentKillStreak[_i] > HighestKillStreak[_i])
            {
                HighestKillStreak[_i] = currentKillStreak[_i];
            }
        }

    }

    public void ResetDataKF()
    {

        // populate lists for first maths
        for (int i = 0; i < 10; i++)
        {
            currentKillStreak[i] = 0;
            currentKills[i] = 0;
            currentDeaths[i] = 0;
            previousKills[i] = 0;
            previousDeaths[i] = 0;
            HighestKillStreak[i] = 0;

        }
        for (int i = 0; i < 10; i++)
        {
            isStreaking[i] = (false);
        }
    }
}
