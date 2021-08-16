using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Linq;

public class KillFeed : MonoBehaviour
{

    private List<int> previousKills = new List<int>();
    private List<int> previousDeaths = new List<int>();
    private List<int> currentKills = new List<int>();
    private List<int> currentDeaths = new List<int>();
    private List<int> currentKillStreak = new List<int>();

    private List<int> teamNumList = new List<int>();
    private List<int> teamHSList = new List<int>();
    private List<int> teamKillsList = new List<int>();
    private List<int> teamDeathsList = new List<int>();

    private List<string> playerNames = new List<string>();

    public static List<int> HighestKillStreak = new List<int>();

    private List<bool> isStreaking = new List<bool>();

    private IEnumerator coroutineKillFeed;
    private IEnumerator coroutineDeathBar;


    private int numActiveKillFeeds = 0;

    [SerializeField]
    private GameObject kfGO;

    [SerializeField]
    private GameObject[] verticalSBPlayers;

    public GameObject stashGO;

    [SerializeField]
    private Transform kfParent;

    [SerializeField]
    private List<Sprite> weaponIcon = new List<Sprite>();

    private Color colorBlue = new Color32(8, 135, 255, 255);
    private Color colorRed = new Color32(240, 14, 52, 255);
    private Color killerColor;
    private Color victumColor;

    [SerializeField]
    private Toggle kfBack;

    // inialize everything with 0 or name
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
            teamNumList.Add(0);
            teamHSList.Add(0);
            playerNames.Add("name");
        }
        for (int i = 0; i < 10; i++)
        {
            isStreaking.Add(false);
        }
        for (int i = 0; i < 2; i++)
        {
            teamDeathsList.Add(0);
            teamKillsList.Add(0);
            teamHSList.Add(0);
        }
    }

    // from SB event
    public void GetSBEvent(Root data)
    {
        // fill with data before we update
        SetPreviousDeaths();

        for (int j = 0; j < data.Data.Names.Length; j++)
        {
            playerNames[j] = data.Data.Names[j];
            teamKillsList[j] = data.Data.Kills[j];
            teamDeathsList[j] = data.Data.Deaths[j];
            teamNumList[j] = data.Data.Teams[j];
        }
    }

    // set previous kills/deaths list from current
    public void SetPreviousDeaths()
    {
        for (int i = 0; i < currentKills.Count; i++)
        {
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
        float wait = 2.0f;
        int fixedWeapon = 0;

        if (SocketServer.staticGunKillInt == 0)
        {
            fixedWeapon = 0;
        }
        if (SocketServer.staticGunKillInt == 1)
        {
            fixedWeapon = 6;
        }
        if (SocketServer.staticGunKillInt == 2)
        {
            fixedWeapon = 4;
        }
        if (SocketServer.staticGunKillInt == 7)
        {
            fixedWeapon = 5;
        }
        if (SocketServer.staticGunKillInt == 8)
        {
            fixedWeapon = 8;
        }
        if (SocketServer.staticGunKillInt == 9)
        {
            fixedWeapon = 7;
        }
        if (SocketServer.staticGunKillInt == 11)
        {
            fixedWeapon = 2;
        }


        coroutineKillFeed = SpawnKF(wait, fixedWeapon, data);
        StartCoroutine(coroutineKillFeed);
    }

    IEnumerator SpawnKF(float _wait, int _weaponType, Root _data)
    {

        CheckColors(_data.Data.Killer, _data.Data.Victum);
        CheckKillStreak();


        if (numActiveKillFeeds == 4)
        {
            numActiveKillFeeds = 0;
        }

        // instantiate kill feed
        GameObject kfTextGO = Instantiate(kfGO, kfParent);
        kfTextGO.name = _data.Data.Killer + "_kf";

        // check if player is streaking. If so, set the streaking parent to active
        for (int i = 0; i < isStreaking.Count; i++)
        {
            if (playerNames[i] == _data.Data.Killer)
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

        kfTextGO.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _data.Data.Killer;
        kfTextGO.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = killerColor;
        kfTextGO.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = weaponIcon[_weaponType];
        kfTextGO.transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().text = _data.Data.Victum;
        kfTextGO.transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().color = victumColor;

        if (_data.Data.HeadShot)
        {
            kfTextGO.transform.GetChild(1).GetChild(2).GetComponent<Image>().color = Color.red;
        }

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
        for (int i = 0; i < playerNames.Count; i++)
        {
            if (playerNames[i] == _killer)
            {
                if (i < 5)
                {
                    killerColor = colorRed;
                }
                if (i > 4)
                {
                    killerColor = colorBlue;
                }
            }

            if (playerNames[i] == _victum)
            {
                if (i < 5)
                {
                    victumColor = colorRed;
                }
                if (i > 4)
                {
                    victumColor = colorBlue;
                }
            }
        }
    }

    public void CheckKillStreak()
    {
        for (int i = 0; i < currentKills.Count; i++)
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
        currentKillStreak.Clear();
        currentKills.Clear();
        currentDeaths.Clear();
        previousKills.Clear();
        previousDeaths.Clear();
        HighestKillStreak.Clear();
        isStreaking.Clear();

        // populate lists for first maths
        for (int i = 0; i < 10; i++)
        {
            currentKillStreak.Add(0);
            currentKills.Add(0);
            currentDeaths.Add(0);
            previousKills.Add(0);
            previousDeaths.Add(0);
            HighestKillStreak.Add(0);

        }
        for (int i = 0; i < 10; i++)
        {
            isStreaking.Add(false);
        }
    }
}
