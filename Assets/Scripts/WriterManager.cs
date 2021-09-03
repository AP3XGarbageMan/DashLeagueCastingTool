using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class WriterManager : MonoBehaviour
{
    public Root data;
    public SB_Manager mSB;

    [SerializeField]
    public TextMeshProUGUI pathTMP;
    [SerializeField]
    public TextMeshProUGUI dropDownText;
    [SerializeField]
    public TextMeshProUGUI playerLabelText;
    [SerializeField]
    public TextMeshProUGUI[] graphLabelText;
    public TextMeshProUGUI[] playerDeathsLabel;

    public List<List<int>> hasKilledList = new List<List<int>>();

    public GameObject[] dataBars;

    public GameObject miniMapParent;
    public GameObject ktParent;

    public Toggle swapMiniKT;

    private void Start()
    {
        mSB = mSB.GetComponent<SB_Manager>();
    }

    private void Update()
    {
        if (swapMiniKT.isOn)
        {
            miniMapParent.SetActive(false);
            ktParent.SetActive(true);
            CleanUpData();
            PopulateBarGraph();
        }
        else
        {
            ktParent.SetActive(false);
            miniMapParent.SetActive(true);
        }
    }
    public void CleanUpData()
    {
        if (hasKilledList != null)
        {
            hasKilledList.Clear();
        }

        for (int i = 0; i < mSB.pIG.Count; i++)
        {
            List<int> dataLine = new List<int>();
            for (int j = 0; j < mSB.pIG.Count; j++)
            {
                int killCount = 0;
                foreach (string _victim in mSB.pIG[i].hasKilled)
                {
                    // add the killer first
                    if (mSB.pIG[j].ShortName == _victim)
                    {
                        killCount++;
                    }
                }
                dataLine.Add(killCount);
            }
            hasKilledList.Add(dataLine);
            //Debug.Log(toPrint);
        }
    }

    public void PopulateBarGraph()
    {
        // get the player we are interested in
        string selPlayer = dropDownText.text;
        playerLabelText.text = selPlayer;

        // loop though player short names until we find the player we care about
        for (int i = 0; i < mSB.pIG.Count; i++)
        {
            graphLabelText[i].text = mSB.pIG[i].ShortName;

            if (mSB.pIG[i].ShortName == selPlayer)
            {
                List<int> newList = new List<int>();
                newList = hasKilledList[i];

                // make each bar bigger based on kill count
                for (int j = 0; j < mSB.pIG.Count; j++)
                {
                    playerDeathsLabel[j].text = newList[j].ToString();
                }

            }
        }
    }


}
