using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CP_Manager : MonoBehaviour
{
    public Root data;
    public SB_Manager mSB;
    public bool isCP = false;

    [SerializeField]
    private TextMeshProUGUI[] teamTopPanelScore;
    [SerializeField]
    private TextMeshProUGUI[] teamSBMapScore;

    public bool isRedCountDown = false;
    public bool isBlueCountDown = false;

    public Color colorBlue = new Color32(8, 135, 255, 255);
    public Color colorRed = new Color32(240, 14, 52, 255);

    [SerializeField]
    private GameObject domTeamColorFlash;
    [SerializeField]
    private GameObject miniMap;

    public Toggle isCPToggle;

    // Start is called before the first frame update
    void Start()
    {
        mSB = mSB.GetComponent<SB_Manager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isBlueCountDown)
        {
            isRedCountDown = false;
            domTeamColorFlash.gameObject.SetActive(true);
            domTeamColorFlash.GetComponent<Image>().color = colorBlue;
        }
        if (isRedCountDown)
        {
            isBlueCountDown = false;
            domTeamColorFlash.gameObject.SetActive(true);
            domTeamColorFlash.GetComponent<Image>().color = colorRed;
        }

        if (isCP)
        {
            miniMap.SetActive(true);

            teamTopPanelScore[0].text = data.Data.RedScore.ToString();
            teamSBMapScore[0].text = data.Data.RedScore.ToString();
            teamTopPanelScore[1].text = data.Data.BlueScore.ToString();
            teamSBMapScore[1].text = data.Data.BlueScore.ToString();

            if (data.Data.ButtonInfo[0].Team == -1)
            {
                isRedCountDown = false;
                isBlueCountDown = false;
            }
            if (data.Data.ButtonInfo[0].Team == 0)
            {
                isBlueCountDown = true;
            }
            if (data.Data.ButtonInfo[0].Team == 1)
            {
                isRedCountDown = true;

            }
        }
        if (!isCPToggle.isOn)
        {
            miniMap.SetActive(false);
        }

    }
}

