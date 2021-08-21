using TMPro;
using UnityEngine;

public class PopulateMainScoreboard : MonoBehaviour
{
    public SB_Manager mSB;

    [SerializeField]
    private TextMeshProUGUI[] mainSBTeamNames;
    [SerializeField]
    private TextMeshProUGUI[] verticalSBTeamNames;


    [SerializeField]
    private TextMeshProUGUI[] mainSBNames;
    [SerializeField]
    private TextMeshProUGUI[] mainSBKills;
    [SerializeField]
    private TextMeshProUGUI[] mainSBDeaths;
    [SerializeField]
    private TextMeshProUGUI[] mainSBKD;
    [SerializeField]
    private TextMeshProUGUI[] mainSBHS;
    [SerializeField]
    private TextMeshProUGUI[] mainSBScore;

    [SerializeField]
    private TextMeshProUGUI[] verticalSBNames;
    [SerializeField]          
    private TextMeshProUGUI[] verticalSBKills;
    [SerializeField]          
    private TextMeshProUGUI[] verticalSBDeaths;

    [SerializeField]
    private TextMeshProUGUI[] teamSBKills;
    [SerializeField]          
    private TextMeshProUGUI[] teamSBDeaths;
    [SerializeField]          
    private TextMeshProUGUI[] teamSBKD;
    [SerializeField]          
    private TextMeshProUGUI[] teamSBHS;
    [SerializeField]          
    private TextMeshProUGUI[] teamSBScore;
    [SerializeField]
    private TextMeshProUGUI[] teamTopPanelScore;
    [SerializeField]
    private TextMeshProUGUI[] teamSBMapScore;


    // Start is called before the first frame update
    void Start()
    {
        mSB = mSB.GetComponent<SB_Manager>();
    }

    private void Update()
    {
        for (int i = 0; i < mSB.playerNamesWithSpaces.Length; i++)
        {
            mainSBNames[i].text = mSB.playerNamesWithSpaces[i];
            mainSBKills[i].text = mSB.currentKills[i].ToString();
            mainSBDeaths[i].text = mSB.currentDeaths[i].ToString();
            mainSBKD[i].text = mSB.playerKD[i].ToString("0.0");

            verticalSBNames[i].text = mSB.playerNamesWithSpaces[i];
            verticalSBKills[i].text = mSB.currentKills[i].ToString();
            verticalSBDeaths[i].text = mSB.currentDeaths[i].ToString();
        }
        for (int i = 0; i < 2; i++)
        {
            mainSBTeamNames[i].text = mSB.teamNames[i];
            verticalSBTeamNames[i].text = mSB.teamNames[i];
            teamSBKills[i].text = mSB.teamKills[i].ToString();
            teamSBDeaths[i].text = mSB.teamDeaths[i].ToString();
            teamSBKD[i].text = mSB.teamKD[i].ToString("0.0");
        }
    }
}
