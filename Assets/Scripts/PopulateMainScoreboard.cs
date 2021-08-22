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
        for (int i = 0; i < mSB.playerNamesWithSpaces.Length; i++)
        {
            mainSBNames[i].text = mSB.playerNamesWithSpaces[i];
            mainSBKills[i].text = mSB.currentKills[i].ToString();
            mainSBDeaths[i].text = mSB.currentDeaths[i].ToString();
            mainSBKD[i].text = mSB.playerKD[i].ToString("0.0");
            mainSBHS[i].text = mSB.currentHS[i].ToString();
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
        if (lvR.isOn)
        {
            ChangeVSColor(verticalSBBackgroundL, colorRed, lvR);
        }
        if (lvB.isOn)
        {
            ChangeVSColor(verticalSBBackgroundL, colorBlue, lvB);
        }
        if (rvR.isOn)
        {
            ChangeVSColor(verticalSBBackgroundR, colorRed, rvR);
        }
        if (rvB.isOn)
        {
            ChangeVSColor(verticalSBBackgroundR, colorBlue, rvB);
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
