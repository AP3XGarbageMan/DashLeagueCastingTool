using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Manager_KillFeedSpawner : MonoBehaviour
{
    public static Manager_KillFeedSpawner instance;

    private void Awake()
    {
        instance = null;

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private int[] previousKills;
    private int[] previousDeaths;
    private int[] currentKills;
    private int[] currentDeaths;
    private int[] HighestKillStreak;
    private int[] currentKillStreak;
    private int[] teamNumList;
    private int[] teamHSList;
    private int[] teamKillsList;
    private int[] teamDeathsList;

    private int numActiveKillFeeds;

    private string[] playerNames;

    private bool[] isStreaking;

    //private IEnumerator coroutineKillFeed;
    //private IEnumerator coroutineDeathBar;

    [SerializeField]
    private List<Sprite> weaponIcon;

    [SerializeField]
    private Toggle kfBack;

    private Color colorBlue;
    private Color colorRed;

    private Color killerColor;
    private Color victumColor;

    public KFManager kfm;

    private void Start()
    {
        
        isStreaking = new bool[10];
        playerNames = new string[10];
        numActiveKillFeeds = 0;
        previousKills = new int[10];
        previousDeaths = new int[10];
        currentKills = new int[10];
        currentDeaths = new int[10];
        HighestKillStreak = new int[10];
        currentKillStreak = new int[10];
        teamNumList = new int[10];
        teamHSList = new int[10];
        teamKillsList = new int[10];
        teamDeathsList = new int[10];
        weaponIcon = new List<Sprite>();
        colorBlue = new Color32(8, 135, 255, 255);
        colorRed = new Color32(240, 14, 52, 255);

        // populate lists for first maths
        for (int i = 0; i < 10; i++)
        {
            currentKillStreak[i] = 0;
            currentKills[i] = 0;
            currentDeaths[i] = 0;
            previousKills[i] = 0;
            previousDeaths[i] = 0;
            HighestKillStreak[i] = 0;
            teamNumList[i] = 0;
            teamHSList[i] = 0;
            playerNames[i] = ("name");
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

        kfm = gameObject.GetComponent<KFManager>();
    }



    // from SB event
    public void GetSBEvent(Root data)
    {
        // fill with data before we update
        SetPreviousDeaths();
        
        for (int j = 0; j < playerNames.Length; j++)
        {
            playerNames[j] = data.Data.Names[j];
            teamKillsList[j] = data.Data.Kills[j];
            teamDeathsList[j] = data.Data.Deaths[j];
            //Debug.Log("previous deaths = " + teamDeathsList[j].ToString());
            teamNumList[j] = data.Data.Teams[j];
        }
    }

    // set previous kills/deaths list from current
    public void SetPreviousDeaths()
    {
        for (int i = 0; i < playerNames.Length; i++)
        {
            //Debug.Log("previous deaths = " + previousDeaths[i].ToString() );
            previousDeaths[i] = teamDeathsList[i];
            previousKills[i] = teamKillsList[i];
        }
    }


    // check which gun was used, convert to how we have the icons in a list
    // could probably just put the damn icons in the order HD has it

    public void SpawnKF(Root data)
    {
        Debug.Log("Made it to spawn kf ");

        float _wait = 2.0f;
        int fixedWeapon = 0;
        string _killer = data.Data.Killer;
        string _victim = data.Data.Victum;
        bool _HS = data.Data.HeadShot;
        bool _streaking = false;

        switch (data.Data.WeaponsType)
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

        // check if player is streaking. If so, set the streaking parent to active
        for (int i = 0; i < isStreaking.Length; i++)
        {
            if (playerNames[i] == _killer)
            {
                if (isStreaking[i])
                {
                    if (currentKillStreak[i] > HighestKillStreak[i])
                    {
                        HighestKillStreak[i] = currentKillStreak[i];
                    }

                    _streaking = true;
                }
                else
                {
                    _streaking = false;
                }
            }
        }

        CheckColors(_killer, _victim);


        kfm.StartKF(_wait, weaponIcon[fixedWeapon], _killer, _victim, killerColor, victumColor, _HS, _streaking, kfBack.isOn);

        //coroutineKillFeed = SpawnKF(wait, fixedWeapon, _killer, _victim);
        //Debug.Log("starting kf coroutine");

        //StartCoroutine(coroutineKillFeed);
    }

    //IEnumerator SpawnKF(float _wait, int _weaponType, string _killer, string _victim)
    //{
    //    Debug.Log("Made it to spawn kf numerator");

    //    //CheckColors(_killer, _victim);
    //    //CheckColors(_data.Data.Killer, _data.Data.Victum);
    //    //CheckKillStreak();


    //    if (numActiveKillFeeds == 4)
    //    {
    //        numActiveKillFeeds = 0;
    //    }

    //    Debug.Log("instantiating a thing");
    //    // instantiate kill feed
    //    GameObject kfTextGO = Instantiate(kfGO, kfParent);
    //    kfTextGO.name = _killer + "_kf";

    //    // check if player is streaking. If so, set the streaking parent to active
    //    for (int i = 0; i < isStreaking.Length; i++)
    //    {
    //        if (playerNames[i] == _killer)
    //        {
    //            if (isStreaking[i])
    //            {
    //                kfTextGO.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = currentKillStreak[i].ToString();
    //            }
    //            else
    //            {
    //                kfTextGO.transform.GetChild(0).gameObject.SetActive(false);
    //            }
    //        }
    //    }

    //    kfTextGO.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = _killer;
    //    kfTextGO.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = killerColor;
    //    kfTextGO.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = weaponIcon[_weaponType];
    //    kfTextGO.transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().text = _victim;
    //    kfTextGO.transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().color = victumColor;

    //    //if (_data.Data.HeadShot)
    //    //{
    //    //    kfTextGO.transform.GetChild(1).GetChild(2).GetComponent<Image>().color = Color.red;
    //    //}

    //    if (!kfBack.isOn)
    //    {
    //        kfTextGO.GetComponent<Image>().enabled = false;
    //    }

    //    yield return new WaitForSeconds(_wait);
    //    Destroy(kfTextGO);
    //    numActiveKillFeeds++;
    //}

    // check which team the killer/victim were on then assign and set KF
    public void CheckColors(string _killer, string _victum)
    {
        Debug.Log("Made it to check colors ");

        for (int i = 0; i < playerNames.Length; i++)
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

    // check to see if player is on a kill streak, if so, add the kills to the streak
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

    // replace if current is higher than previous
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

}
