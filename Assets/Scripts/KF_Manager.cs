using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KF_Manager : MonoBehaviour
{
    public Root data;
    public SB_Manager mSB;

    private IEnumerator spawn_KillFeed;
    private IEnumerator spawn_DB;

    private int fixedWeapon = 0;

    [SerializeField]
    private GameObject p_KillFeed;

    [SerializeField]
    private Transform kfParent;
    [SerializeField]
    private Toggle kfBack;

    [SerializeField]
    private Sprite[] weaponIcon;

    [SerializeField]
    private GameObject[] deathBars;

    public Color colorBlue = new Color32(8, 135, 255, 255);
    public Color colorRed = new Color32(240, 14, 52, 255);
    public Color killerColor;
    public Color victimColor;

    public bool killHappened = false;

    [SerializeField]
    private Toggle swapKFColors;

    public string cutKillerName = "";
    public string cutVictimName = "";

    private void Start()
    {
        mSB = mSB.GetComponent<SB_Manager>();
    }

    private void Update()
    {
        if (killHappened)
        {
            killHappened = false;
            StartKFSequence();
            StartDeathBar();
        }
    }

    public void GetShortName(string _type, string _name)
    {
        if (_type == "Killer")
        {
            for (int i = 0; i < mSB.pIG.Count; i++)
            {
                // check is name is == killer
                if (mSB.pIG[i].Name == data.Data.Killer)
                {
                    cutKillerName = mSB.pIG[i].ShortName;
                }
            }
        }
        if (_type == "Victim")
        {
            for (int i = 0; i < mSB.pIG.Count; i++)
            {
                // check is name is == killer
                if (mSB.pIG[i].Name == data.Data.Victum)
                {
                    cutVictimName = mSB.pIG[i].ShortName;
                }
            }
        }
    }

    public void StartKFSequence()
    {
        //Debug.Log("starting kf sequence");
        spawn_KillFeed = SpawnKillFeed();
        StartCoroutine(spawn_KillFeed);
    }

    IEnumerator SpawnKillFeed()
    { 
        CheckColors(data.Data.Killer, data.Data.Victum);
        CheckWeaponIcon(data.Data.WeaponsType);

        // instantiate kill feed
        GameObject kfTextGO = Instantiate(p_KillFeed, kfParent);
        kfTextGO.name = data.Data.Killer + "_kf";

        // check if player is streaking. If so, set the streaking parent to active
        for (int i = 0; i < mSB.pIG.Count; i++)
        {
            GetShortName("Killer", data.Data.Killer);
            GetShortName("Victim", data.Data.Victum);

            // check is name is == killer
            if (mSB.pIG[i].Name == data.Data.Killer)
            {
                // for player name in list of pIG
                if (mSB.sortingPlayers)
                {
                    mSB.pIG[i].hasKilled.Add(cutVictimName);
                }
                // check if streaking
                if (mSB.pIG[i].isStreaking)
                {
                    kfTextGO.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = mSB.pIG[i].CurrentKillStreak.ToString();
                }
                else
                {
                    kfTextGO.transform.GetChild(0).gameObject.SetActive(false);
                }
            }

            // find the victum
            if (mSB.pIG[i].Name == data.Data.Victum)
            {
                if (mSB.sortingPlayers)
                {                        
                    mSB.pIG[i].killedBy.Add(cutKillerName);             
                }            
            }
        }



        // setup prefab
        kfTextGO.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = cutKillerName;
        if (swapKFColors.isOn)
        {
            kfTextGO.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = victimColor;
        }
        else
        {
            kfTextGO.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = killerColor;
        }
        kfTextGO.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = weaponIcon[fixedWeapon];
        kfTextGO.transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().text = cutVictimName;
        if (swapKFColors.isOn)
        {
            kfTextGO.transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().color = killerColor;
        }
        else
        {
            kfTextGO.transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().color = victimColor;
        }


        // if its a headshot make the gun red
        if (data.Data.HeadShot)
        {
            kfTextGO.transform.GetChild(1).GetChild(2).GetComponent<Image>().color = Color.red;

            for (int i = 0; i < mSB.pIG.Count; i++)
            {
                if (mSB.pIG[i].Name == data.Data.Killer)
                {
                    mSB.pIG[i].headShots++;
                }
            }
        }

        // toggle the background
        if (!kfBack.isOn)
        {
            kfTextGO.GetComponent<Image>().enabled = false;
        }

        yield return new WaitForSeconds(2f);
        //Debug.Log("I watied");
        Destroy(kfTextGO);
    }

    public void CheckColors(string _killer, string _victim)
    {
        int arraySize = mSB.pIG.Count;
        //Debug.Log("Checking colors and the count is " + arraySize.ToString());

        for (int i = 0; i < arraySize; i++)
        {
            if (mSB.pIG[i].Name == _killer)
            {
                //Debug.Log("killer = " + _killer + ", used a " + data.Data.WeaponsType.ToString());
                if (mSB.pIG[i].Team == 0)
                {
                    killerColor = colorRed;
                }
                if (mSB.pIG[i].Team == 1)
                {
                    killerColor = colorBlue;
                }
            }

            if (mSB.pIG[i].Name == _victim)
            {
                if (mSB.pIG[i].Team == 0)
                {
                    victimColor = colorRed;
                }
                if (mSB.pIG[i].Team == 1)
                {
                    victimColor = colorBlue;
                }
            }
        }
    }

    public void CheckWeaponIcon(int _gunType)
    {
        switch (_gunType)
        {
            // pistol
            case 0:
                fixedWeapon = 0;
                break;
            // smg
            case 1:
                fixedWeapon = 6;
                break;
            // rocket
            case 2:
                fixedWeapon = 4;
                break;
            // shotgun
            case 7:
                fixedWeapon = 5;
                break;
            // staples
            case 8:
                fixedWeapon = 8;
                break;
            // sniper
            case 9:
                fixedWeapon = 7;
                break;
            // shock
            case 10:
                fixedWeapon = 2;
                break;

        }
    }

    public void StartDeathBar()
    {
        spawn_DB = SpawnDeathBar();
        StartCoroutine(spawn_DB);
    }

    IEnumerator SpawnDeathBar()
    {
        int playeri = 0;
        bool didSpawn = false;
        // check if player is streaking. If so, set the streaking parent to active
        for (int i = 0; i < mSB.pIG.Count; i++)
        {
            if (mSB.pIG[i].Name == data.Data.Victum)
            {
                didSpawn = true;
                deathBars[i].SetActive(true);
                playeri = i;
            }
        }
        yield return new WaitForSeconds(4f);
        if (didSpawn)
        {
            deathBars[playeri].SetActive(false);
            didSpawn = false;
        }
    }
}
