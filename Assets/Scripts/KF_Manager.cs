using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KF_Manager : MonoBehaviour
{
    public Root data;
    
    [SerializeField] GameObject p_KillFeed;
    [SerializeField] Transform kfParent;
    [SerializeField] Sprite[] weaponIcon;

    public Color colorBlue = new Color32(8, 135, 255, 255);
    public Color colorRed = new Color32(240, 14, 52, 255);
    public Color killerColor;
    public Color victimColor;

    private void Start()
    {
        // This way the SocketServer doesn't need to know there is a KF_Mannager
        SocketServer.KillFeedEvent += StartKfSequence;
    }

    private void StartKfSequence(Root data)
    {
        this.data = data;
        StartCoroutine(SpawnKillFeed());
    }

    IEnumerator SpawnKillFeed()
    {
        // CheckColors(data.Data.Killer, data.Data.Victum);
        int typetoImage = GetWeaponIcon(data.Data.WeaponsType);

        //// instantiate kill feed
        GameObject kfPrefab = Instantiate(p_KillFeed, kfParent);
        kfPrefab.name = data.Data.Killer + "_kf";
        KF_Blok kfBlok = kfPrefab.GetComponent<KF_Blok>();

        ////// check if player is streaking. If so, set the streaking parent to active
        // for (int i = 0; i < spn.playerNames.Length; i++)
        // {
        //     if (spn.playerNames[i] == data.Data.Killer)
        //     {
        //         if (sbm.isStreaking[i])
        //         {
        //             kfTextGO.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = sbm.currentKillStreak[i].ToString();
        //         }
        //         else
        //         {
        //             kfTextGO.transform.GetChild(0).gameObject.SetActive(false);
        //         }
        //     }
        // }

        //// setup prefab
        kfBlok.killerText.text = data.Data.Killer;
        kfBlok.killerText.color = colorRed;
        
        kfBlok.weaponIcon.sprite = weaponIcon[typetoImage];
        kfBlok.weaponIcon.color = data.Data.HeadShot ? Color.red : Color.white;
        
        kfBlok.victumText.text = data.Data.Victum;
        kfBlok.victumText.color = colorBlue;

        yield return new WaitForSeconds(2f);
        
        float opacty = 1;
        while (opacty > 0)
        {
            yield return new WaitForEndOfFrame();
            opacty -= Time.deltaTime * 1;
            
            // No global alpha so this is the best we have
            kfBlok.backGround.color = changeOpatcity(kfBlok.backGround.color, opacty);
            kfBlok.weaponIcon.color = changeOpatcity(kfBlok.weaponIcon.color, opacty);
            kfBlok.killerText.color = changeOpatcity(kfBlok.killerText.color, opacty);
            kfBlok.victumText.color = changeOpatcity(kfBlok.victumText.color, opacty);
            kfBlok.killStreakCount.color = changeOpatcity(kfBlok.killStreakCount.color, opacty);
            kfBlok.killStreakInfoText.color = changeOpatcity(kfBlok.killStreakInfoText.color, opacty);
        }
        
        Destroy(kfPrefab);
    }

    private Color changeOpatcity(Color oldColor, float opaticty)
    {
        return new Color(oldColor.r, oldColor.g, oldColor.b, opaticty);
    }

    //public void CheckColors(string _killer, string _victim)
    //{
    //    Debug.Log("Checking colors and the count is " + spn.playerNames.Length.ToString());
    //    for (int i = 0; i < spn.playerNames.Length; i++)
    //    {
    //        if (spn.playerNames[i] == _killer)
    //        {
    //            Debug.Log("killer = " + _killer + ", playerPos name = " + spn.playerNames[i] + ", count is " + i.ToString());
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
    //    for (int i = 0; i < spn.playerNames.Length; i++)
    //    {
    //        if (spn.playerNames[i] == _victim)
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
