using System;
using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class Domination_Manager : MonoBehaviour
{
    public Root data;
    public SB_Manager mSB;

    public int[] domButtonValue = new int[3];
    public bool[] isHoldingButton = new bool[3];

    public static event Action<Root> DominationUpdateMM;

    public int redScore = 0;
    public int blueScore = 0;

    public int redCountDowns = 0;
    public int blueCountDowns = 0;

    public int[] buttontaps = new int[2];

    public bool isDom = false;
    public bool isWW = false;
    public bool isQuarry = false;
    public bool isBlueCountDown = false;
    public bool isRedCountDown = false;
    public bool isCountingDown = false;

    int currentRBT = 0;
    int currentBBT = 0;

    public List<DominationButtonTaps> domBTStats = new List<DominationButtonTaps>();

    public int lastIntToHit = 0;

    // Start is called before the first frame update
    void Start()
    {
        SocketServer.DominationEvent += DominationUpdate;
        mSB = mSB.GetComponent<SB_Manager>();


        for (int i = 0; i < 3; i++)
        {
            domButtonValue[i] = -1;
        }
        for (int i = 0; i < 2; i++)
        {
            buttontaps[i] = 0;
        }
    }

    // Update is called once per frame
    void DominationUpdate(Root _data)
    {
        isDom = true;

        this.data = _data;
        CheckButtonTapValues();

        redScore = data.Data.RedScore;
        blueScore = data.Data.BlueScore;
        mSB.UpdateFromDomination(redScore, blueScore);

        for (int i = 0; i < 3; i++)
        {
            domButtonValue[i] = data.Data.ButtonInfo[i].Team;
        }

        CheckForCountDown();
    }



    public void CheckButtonTapValues()
    {
        for (int i = 0; i < 3; i++)
        {
            if (domButtonValue[i] != data.Data.ButtonInfo[i].Team)
            {
                if (domButtonValue[i] == -1)
                {
                    if (data.Data.ButtonInfo[i].Team == 0)
                    {
                        buttontaps[0]++;
                        Debug.Log("hit a button: " + mSB.pIG[lastIntToHit].ShortName);
                        mSB.pIG[lastIntToHit].totalButtonTaps++;
                    }
                    if (data.Data.ButtonInfo[i].Team == 1)
                    {
                        buttontaps[1]++;
                        Debug.Log("hit a button: " + mSB.pIG[lastIntToHit].ShortName);
                        mSB.pIG[lastIntToHit].totalButtonTaps++;
                    }            
                }

                if (domButtonValue[i] == 0)
                {
                    buttontaps[1]++;
                    Debug.Log("hit a button: " + mSB.pIG[lastIntToHit].ShortName);
                    mSB.pIG[lastIntToHit].totalButtonTaps++;
                }
                if (domButtonValue[i] == 1)
                {
                    buttontaps[0]++;
                    Debug.Log("hit a button: " + mSB.pIG[lastIntToHit].ShortName);
                    mSB.pIG[lastIntToHit].totalButtonTaps++;
                }
            }
        }

        domBTStats.Add(new DominationButtonTaps(data.TimeStamp, buttontaps[0], buttontaps[1]));
    }

    public void CheckForCountDown()
    {
        int rCD = 0;
        int bCD = 0;

        for (int i = 0; i < 3; i++)
        {
            if (domButtonValue[i] == 0)
            {
                rCD++;
            }
            if (domButtonValue[i] == 1)
            {
                bCD++;
            }
        }

        if (bCD > 2)
        {
            if (currentBBT != buttontaps[1])
            {
                blueCountDowns++;
                mSB.pIG[lastIntToHit].countDownsStarted++;

            }

            currentBBT = buttontaps[1];

        }
        else
        {
            isBlueCountDown = false;
        }
        if (rCD > 2)
        {
            if (currentRBT != buttontaps[0])
            {
                redCountDowns++;
                mSB.pIG[lastIntToHit].countDownsStarted++;
            }

            currentRBT = buttontaps[0];
        }
        else
        {
            isRedCountDown = false;
        }
    }
}