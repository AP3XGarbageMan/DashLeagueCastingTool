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

    public int[] buttontaps = new int[2];

    private Color colorBlue = new Color32(8, 135, 255, 255);
    private Color colorRed = new Color32(240, 14, 52, 255);
    private Color colorWhite = new Color32(255, 255, 255, 255);

    public bool isDom = false;

    public List<DominationButtonTaps> domBTStats = new List<DominationButtonTaps>();

    // Start is called before the first frame update
    void Start()
    {
        SocketServer.DominationEvent += DominationUpdate;
        mSB = mSB.GetComponent<SB_Manager>();

        buttontaps[0] = 0;
        buttontaps[1] = 0;
        for (int i = 0; i < 3; i++)
        {
            domButtonValue[i] = -1;
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
                    }
                    if (data.Data.ButtonInfo[i].Team == 1)
                    {
                        buttontaps[1]++;
                    }            
                }

                if (domButtonValue[i] == 0)
                {
                    buttontaps[1]++;
                }
                if (domButtonValue[i] == 1)
                {
                    buttontaps[0]++;
                }
            }
        }

        domBTStats.Add(new DominationButtonTaps(data.TimeStamp, buttontaps[0], buttontaps[1]));
    }
}