using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestCoRoutine : MonoBehaviour
{

    private IEnumerator testCoroutine;
    private int testcCount = 0;
    private int fixedWeapon = 0;

    public TextMeshProUGUI testTMP; 
    public bool runIt = false;

    public Root data;

    public int totalPlayerNum = 10;

    [SerializeField]
    private GameObject p_KillFeed;
    [SerializeField]
    private Transform kfParent;
    [SerializeField]
    private Toggle kfBack;
    [SerializeField]
    private Sprite[] weaponIcon;

    public Color colorBlue = new Color32(8, 135, 255, 255);
    public Color colorRed = new Color32(240, 14, 52, 255);
    public Color killerColor;
    public Color victimColor;

    SetupPlayerNames spn;

    private void Start()
    {
        spn = GetComponent<SetupPlayerNames>();
    }

    private void Update()
    {
        if (runIt)
        {
            RunIt();
            runIt = false;
        }
    }

    public void RunIt()
    {
        IEnumerator testCoroutine = SendToDebig();
        StartCoroutine(testCoroutine);
       
    }

    IEnumerator SendToDebig()
    {
        Debug.Log("Checking colors and the count is " + spn.playerNames.Length.ToString());

        CheckColors(data.Data.Killer, data.Data.Victum);
        CheckWeaponIcon(data.Data.WeaponsType);
        
        // instantiate kill feed
        GameObject kfTextGO = Instantiate(p_KillFeed, kfParent);
        kfTextGO.name = data.Data.Killer + "_kf";

        //// check if player is streaking. If so, set the streaking parent to active
        //for (int i = 0; i < isStreaking.Count; i++)
        //{
        //    if (playerNames[i] == killer)
        //    {
        //        if (isStreaking[i])
        //        {
        //            kfTextGO.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = currentKillStreak[i].ToString();
        //        }
        //        else
        //        {
        //            kfTextGO.transform.GetChild(0).gameObject.SetActive(false);
        //        }
        //    }
        //}

        // setup prefab
        kfTextGO.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = data.Data.Killer;
        kfTextGO.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().color = killerColor;
        kfTextGO.transform.GetChild(1).GetChild(2).GetComponent<Image>().sprite = weaponIcon[fixedWeapon];
        kfTextGO.transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().text = data.Data.Victum;
        kfTextGO.transform.GetChild(1).GetChild(4).GetComponent<TextMeshProUGUI>().color = victimColor;

        // if its a headshot make the gun red
        if (data.Data.HeadShot)
        {
            kfTextGO.transform.GetChild(1).GetChild(2).GetComponent<Image>().color = Color.red;
        }

        // toggle the background
        if (!kfBack.isOn)
        {
            kfTextGO.GetComponent<Image>().enabled = false;
        }

        yield return new WaitForSeconds(2f);
        Destroy(kfTextGO);

    }


    public void CheckColors(string _killer, string _victim)
    {
        Debug.Log("Checking colors and the count is " + spn.playerNames.Length.ToString());

        for (int i = 0; i < totalPlayerNum; i++)
        {
            if (spn.playerNames[i] == _killer)
            {
                Debug.Log("killer = " + _killer + ", playerPos name = " + spn.playerNames[i] + ", count is " + i.ToString());
                if (i < 5)
                {
                    killerColor = colorRed;
                }
                if (i > 4)
                {
                    killerColor = colorBlue;
                }
            }
        }
        for (int i = 0; i < totalPlayerNum; i++)
        {
            if (spn.playerNames[i] == _victim)
            {
                if (i < 5)
                {
                    victimColor = colorRed;
                }
                if (i > 4)
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
    }
}
