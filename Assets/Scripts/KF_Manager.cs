using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KF_Manager : MonoBehaviour
{
    public Root data;
    public SB_Manager mSB;

    [SerializeField] GameObject p_KillFeed;
    [SerializeField] Transform kfParent;
    [SerializeField] Sprite[] weaponIcon;

    public Color colorBlue = new Color32(8, 135, 255, 255);
    public Color colorRed = new Color32(240, 14, 52, 255);
    public Color killerColor;
    public Color victimColor;

    [SerializeField] TextMeshProUGUI killerText;
    [SerializeField] TextMeshProUGUI victimText;
    [SerializeField] Image weaponSprite;



    public string cutKillerName = "";
    public string cutVictimName = "";

    private IEnumerator kfCoroutine;

    private void Start()
    {
        mSB = mSB.GetComponent<SB_Manager>();

        // This way the SocketServer doesn't need to know there is a KF_Mannager
        SocketServer.KillFeedEvent += StartKfSequence;
    }

    private void StartKfSequence(Root _data)
    {
        this.data = _data;

        if (data.Data.HeadShot)
        {
            for (int i = 0; i < mSB.playerNamesWithSpaces.Length; i++)
            {
                if (data.Data.Killer == mSB.playerName[i])
                {
                    mSB.currentHS[i] += 1;
                }
            }
        }
        for (int i = 0; i < mSB.playerNamesWithSpaces.Length; i++)
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

       // int typetoImage = GetWeaponIcon(data.Data.WeaponsType);
        Debug.Log("checking weapon");
        killerText.text = cutKillerName;
        killerText.color = killerColor;
        Debug.Log("checking weapon1");
        //weaponSprite.sprite = weaponIcon[0];
        Debug.Log("checking weaponw");
        weaponSprite.color = data.Data.HeadShot ? colorRed : Color.white;
        Debug.Log("checking weapon2w");
        victimText.text = cutVictimName;
        victimText.color = victimColor;
    }

    IEnumerator SpawnKillFeed()
    {
        Debug.Log("checking weapon");
        // CheckColors(data.Data.Killer, data.Data.Victum);
        //int typetoImage = GetWeaponIcon(data.Data.WeaponsType);
        //Debug.Log("instantiate kill feed");
        ////// instantiate kill feed
        //GameObject kfPrefab = Instantiate(p_KillFeed, kfParent);
        //kfPrefab.name = cutKillerName + "_kf";
        //KF_Blok kfBlok = kfPrefab.GetComponent<KF_Blok>();

        //////// check if player is streaking. If so, set the streaking parent to active
        //// for (int i = 0; i < spn.playerNames.Length; i++)
        //// {
        ////     if (spn.playerNames[i] == data.Data.Killer)
        ////     {
        ////         if (sbm.isStreaking[i])
        ////         {
        ////             kfTextGO.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = sbm.currentKillStreak[i].ToString();
        ////         }
        ////         else
        ////         {
        ////             kfTextGO.transform.GetChild(0).gameObject.SetActive(false);
        ////         }
        ////     }
        //// }
        //Debug.Log("setup prefab");
        ////// setup prefab
        //kfBlok.killerText.text = cutKillerName;
        //kfBlok.killerText.color = killerColor;

        //kfBlok.weaponIcon.sprite = weaponIcon[typetoImage];
        //kfBlok.weaponIcon.color = data.Data.HeadShot ? colorRed : Color.white;

        //kfBlok.victumText.text = cutVictimName;
        //kfBlok.victumText.color = victimColor;
        //Debug.Log("before wait");
        yield return new WaitForSeconds(2f);
        Debug.Log("checking weapon");
        ////float opacty = 1;
        ////while (opacty > 0)
        ////{
        ////    yield return new WaitForEndOfFrame();
        ////    opacty -= Time.deltaTime * 1;

        ////    // No global alpha so this is the best we have
        ////    kfBlok.backGround.color = changeOpatcity(kfBlok.backGround.color, opacty);
        ////    kfBlok.weaponIcon.color = changeOpatcity(kfBlok.weaponIcon.color, opacty);
        ////    kfBlok.killerText.color = changeOpatcity(kfBlok.killerText.color, opacty);
        ////    kfBlok.victumText.color = changeOpatcity(kfBlok.victumText.color, opacty);
        ////    kfBlok.killStreakCount.color = changeOpatcity(kfBlok.killStreakCount.color, opacty);
        ////    kfBlok.killStreakInfoText.color = changeOpatcity(kfBlok.killStreakInfoText.color, opacty);
        ////}

        //Destroy(kfPrefab);
    }

    private Color changeOpatcity(Color oldColor, float opaticty)
    {
        return new Color(oldColor.r, oldColor.g, oldColor.b, opaticty);
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
                    if (mSB.pIG[i].Team == 0)
                    {
                        killerColor = colorRed;
                    }
                    else
                    {
                        killerColor = colorBlue;
                    }
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
                    if (mSB.pIG[i].Team == 0)
                    {
                        victimColor = colorRed;
                    }
                    else
                    {
                        victimColor = colorBlue;
                    }
                }
            }
        }
    }
    private int GetWeaponIcon(int gunType)
    {
        switch (gunType)
        {
            case 0:
                return 0;
            case 6:
                return 1;
            case 4:
                return 2;
            case 5:
                return 7;
            case 8:
                return 8;
            case 7:
                return 9;
            case 2:
                return 11;
        }

        return 0;
    }

}

