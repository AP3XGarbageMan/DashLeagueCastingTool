using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopulateMainScoreboard : MonoBehaviour
{
    public SB_Manager mSB;
    public Domination_Manager mD;

    [SerializeField]
    private GameObject[] verticalSBBackgroundL;
    [SerializeField]
    private GameObject[] verticalSBBackgroundR;

    [SerializeField]
    private TextMeshProUGUI[] mainSBTeamNames;
    [SerializeField]
    private TextMeshProUGUI[] verticalSBTeamNames;
    [SerializeField]
    private GameObject[] verticalTeamBoxes;

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
    private TextMeshProUGUI[] mainSBPlayerScore;
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
    private TextMeshProUGUI[] highestKS;

    [SerializeField]
    private Toggle isPayload;

    [SerializeField]
    private Toggle lvR;
    [SerializeField]
    private Toggle lvB;
    [SerializeField]
    private Toggle rvR;
    [SerializeField]
    private Toggle rvB;

    public Color colorBlue = new Color32(8, 135, 255, 255);
    public Color colorRed = new Color32(240, 14, 52, 255);

    // Start is called before the first frame update
    void Start()
    {
        mSB = mSB.GetComponent<SB_Manager>();
    }

    private void Update()
    {
        if (mSB.sortingPlayers)
        {
            for (int i = 0; i < mSB.pIG.Count; i++)
            {
                mainSBNames[i].text = mSB.pIG[i].ShortName;
                mainSBKills[i].text = mSB.pIG[i].Kills.ToString();
                mainSBDeaths[i].text = mSB.pIG[i].Deaths.ToString();
                mainSBKD[i].text = mSB.playerKD[i].ToString("0.0");
                mainSBHS[i].text = mSB.pIG[i].headShots.ToString();
                mainSBPlayerScore[i].text = mSB.pIG[i].Score.ToString();
                verticalSBNames[i].text = mSB.pIG[i].ShortName;
                verticalSBKills[i].text = mSB.pIG[i].Kills.ToString();
                verticalSBDeaths[i].text = mSB.pIG[i].Deaths.ToString();
                highestKS[i].text = mSB.pIG[i].HighestKillStreak.ToString();
            }
            for (int i = 0; i < 2; i++)
            {
                teamSBKills[i].text = mSB.teamKills[i].ToString();
                teamSBDeaths[i].text = mSB.teamDeaths[i].ToString();
                teamSBKD[i].text = mSB.teamKD[i].ToString("0.0");
                teamSBHS[i].text = mSB.teamHS[i].ToString();
                teamSBScore[i].text = mSB.teamScore[i].ToString();
            }
            if (isPayload.isOn)
            {
                mainSBTeamNames[0].text = mSB.pIG[0].TeamName;
                verticalSBTeamNames[0].text = mSB.pIG[0].TeamName;
                mainSBTeamNames[1].text = mSB.pIG[9].TeamName;
                verticalSBTeamNames[1].text = mSB.pIG[9].TeamName;
            }
            else
            {
                mainSBTeamNames[0].text = mSB.pIG[9].TeamName;
                verticalSBTeamNames[0].text = mSB.pIG[9].TeamName;
                mainSBTeamNames[1].text = mSB.pIG[0].TeamName;
                verticalSBTeamNames[1].text = mSB.pIG[0].TeamName;
            }
            if (lvR.isOn)
            {
                ChangeVSColor(verticalSBBackgroundL, colorRed, lvR);
                for (int i = 5; i < 10; i++)
                {
                    verticalTeamBoxes[i].GetComponent<Image>().color = colorRed;
                }
            }
            if (lvB.isOn)
            {
                ChangeVSColor(verticalSBBackgroundL, colorBlue, lvB);
                for (int i = 5; i < 10; i++)
                {
                    verticalTeamBoxes[i].GetComponent<Image>().color = colorBlue;
                }
            }
            if (rvR.isOn)
            {
                ChangeVSColor(verticalSBBackgroundR, colorRed, rvR);
                for (int i = 0; i < 5; i++)
                {
                    verticalTeamBoxes[i].GetComponent<Image>().color = colorRed;
                }
            }
            if (rvB.isOn)
            {
                ChangeVSColor(verticalSBBackgroundR, colorBlue, rvB);
                for (int i = 0; i < 5; i++)
                {
                    verticalTeamBoxes[i].GetComponent<Image>().color = colorBlue;
                }
            }
        }
    }

    public void ChangeVSColor(GameObject[] _vs, Color _color, Toggle _isOnBool)
    {
        for (int i = 0; i < 7; i++)
        {
            _vs[i].GetComponent<Image>().color = _color;
        }
        _isOnBool.isOn = false;
    }
}
