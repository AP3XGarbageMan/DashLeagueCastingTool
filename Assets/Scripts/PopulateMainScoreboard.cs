using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopulateMainScoreboard : MonoBehaviour
{
    public SB_Manager mSB;
    public Domination_Manager mD;

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
    private TextMeshProUGUI[] teamButtonTapsTMP;
    [SerializeField]
    private TextMeshProUGUI[] teamCountDownsTMP;

    [SerializeField] 
    private TextMeshProUGUI[] teamTopPanelScore;
    [SerializeField] 
    private TextMeshProUGUI[] teamSBMapScore;

    [SerializeField]
    private Toggle isPayload;

    [SerializeField]
    private Toggle swapPayloadColorsToggle;

    [SerializeField]
    private GameObject[] blueScoreboards;
    [SerializeField]
    private GameObject[] redScoreboards;
    [SerializeField]
    private GameObject[] blueDots;
    [SerializeField]
    private GameObject[] redDots;

    public Color colorBlue = new Color32(8, 135, 255, 255);
    public Color colorRed = new Color32(240, 14, 52, 255);

    public int colorSwitchCount = 0;
    public bool payloadSwapped = false;
    public float timeSwappedAt = 0f;

    // Start is called before the first frame update
    void Start()
    {
        mSB = mSB.GetComponent<SB_Manager>();
        mD = mD.GetComponent<Domination_Manager>();
    }

    private void Update()
    {
        for (int i = 0; i < mSB.playerNamesWithSpaces.Length; i++)
        {
            mainSBNames[i].text = mSB.playerNamesWithSpaces[i];
            mainSBKills[i].text = mSB.currentKills[i].ToString();
            mainSBDeaths[i].text = mSB.currentDeaths[i].ToString();
            mainSBKD[i].text = mSB.playerKD[i].ToString("0.0");
            mainSBHS[i].text = mSB.currentHS[i].ToString();
            mainSBScore[i].text = mSB.currentScore[i].ToString();
            verticalSBNames[i].text = mSB.playerNamesWithSpaces[i];
            verticalSBKills[i].text = mSB.currentKills[i].ToString();
            verticalSBDeaths[i].text = mSB.currentDeaths[i].ToString();
        }
        for (int i = 0; i < 2; i++)
        {
            teamSBKills[i].text = mSB.teamKills[i].ToString();
            teamSBDeaths[i].text = mSB.teamDeaths[i].ToString();
            teamSBKD[i].text = mSB.teamKD[i].ToString("0.0");
            teamSBHS[i].text = mSB.teamHS[i].ToString();
            teamSBScore[i].text = mSB.teamScore[i].ToString();
            teamTopPanelScore[i].text = mSB.teamMapScore[i].ToString();
            teamSBMapScore[i].text = mSB.teamMapScore[i].ToString();
            
        }
        if (isPayload.isOn)
        {
            mainSBTeamNames[0].text = mSB.teamNames[1];
            verticalSBTeamNames[0].text = mSB.teamNames[1];
            mainSBTeamNames[1].text = mSB.teamNames[0];
            verticalSBTeamNames[1].text = mSB.teamNames[0];
        }
        else
        {
            mainSBTeamNames[0].text = mSB.teamNames[0];
            verticalSBTeamNames[0].text = mSB.teamNames[0];
            mainSBTeamNames[1].text = mSB.teamNames[1];
            verticalSBTeamNames[1].text = mSB.teamNames[1];
        }

        if (swapPayloadColorsToggle.isOn)
        {
            if (colorSwitchCount == 2)
            {
                colorSwitchCount = 0;
            }

            swapPayloadColorsToggle.isOn = false;

            if (colorSwitchCount == 0)
            {
                payloadSwapped = true;
                timeSwappedAt = mSB.gameTime;
                //Debug.Log("should be changing colors");
                for (int i = 0; i < redScoreboards.Length; i++)
                {
                    //Debug.Log("should be changing colors like for real");
                    redScoreboards[i].gameObject.GetComponent<Image>().color = colorBlue;
                    if (i < 5)
                    {
                        redDots[i].GetComponent<MeshRenderer>().material.color = colorBlue;
                    }
                }
                //Debug.Log("should be changing colors like for real2");
                for (int i = 0; i < blueScoreboards.Length; i++)
                {
                    blueScoreboards[i].gameObject.GetComponent<Image>().color = colorRed;
                    if (i < 5)
                    {
                        blueDots[i].GetComponent<MeshRenderer>().material.color = colorRed;
                    }
                }
                //Debug.Log("changing color val to 1");
            }
            if (colorSwitchCount == 1)
            {
                payloadSwapped = false;
                for (int i = 0; i < redScoreboards.Length; i++)
                {
                    redScoreboards[i].gameObject.GetComponent<Image>().color = colorRed;
                    if (i < 5)
                    {
                        redDots[i].GetComponent<MeshRenderer>().material.color = colorRed;
                    }
                }
                for (int i = 0; i < blueScoreboards.Length; i++)
                {
                    blueScoreboards[i].gameObject.GetComponent<Image>().color = colorBlue;
                    if (i < 5)
                    {
                        blueDots[i].GetComponent<MeshRenderer>().material.color = colorBlue;
                    }
                }
                //Debug.Log("changing color val to 0");
            }

            colorSwitchCount++;
        }

        if (mD.isDom)
        {
            teamButtonTapsTMP[0].text = mD.buttontaps[0].ToString();
            teamButtonTapsTMP[1].text = mD.buttontaps[1].ToString();
            teamCountDownsTMP[0].text = mD.redCountDowns.ToString();
            teamCountDownsTMP[1].text = mD.blueCountDowns.ToString();
        }
    }
}
