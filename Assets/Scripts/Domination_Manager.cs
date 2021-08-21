using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Domination_Manager : MonoBehaviour
{
    public Root data;
    public SB_Manager mSB;

    public bool isDomination = false;
    public bool isRedCountDown = false;
    public bool isBlueCountDown = false;
    public int[] buttonInfoTeams = new int[3];

    [SerializeField]
    private TextMeshProUGUI[] teamTopPanelScore;
    [SerializeField]
    private TextMeshProUGUI[] teamSBMapScore;

    [SerializeField]
    private Toggle wwToggle;
    [SerializeField]
    private Toggle quarryToggle;

    [SerializeField]
    private GameObject wwMapParent;
    [SerializeField]
    private GameObject quarryMapParent;
    [SerializeField]
    private GameObject payloadCartLightsHolder;
    [SerializeField]
    private GameObject domTeamColorFlash;

    [SerializeField]
    private GameObject[] wwMapParts;
    [SerializeField]
    private GameObject[] quarryMapParts;

    [SerializeField]
    private Material m_concrete;

    public Color colorBlue = new Color32(8, 135, 255, 255);
    public Color colorRed = new Color32(240, 14, 52, 255);

    // Start is called before the first frame update
    void Start()
    {
        mSB = mSB.GetComponent<SB_Manager>();

        for (int i = 0; i < 3; i++)
        {
            buttonInfoTeams[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isBlueCountDown)
        {
            domTeamColorFlash.gameObject.SetActive(true);
            domTeamColorFlash.GetComponent<Image>().color = colorBlue;
        }
        if (isRedCountDown)
        {
            domTeamColorFlash.gameObject.SetActive(true);
            domTeamColorFlash.GetComponent<Image>().color = colorRed;
        }

        if (!isBlueCountDown && !isRedCountDown)
        {
            domTeamColorFlash.gameObject.SetActive(false);
        }

        if (isDomination)
        {
            teamTopPanelScore[0].text = data.Data.RedScore.ToString();
            teamSBMapScore[0].text = data.Data.RedScore.ToString();
            teamTopPanelScore[1].text = data.Data.BlueScore.ToString();
            teamSBMapScore[1].text = data.Data.BlueScore.ToString();

            if (wwToggle.isOn)
            {
                wwMapParent.SetActive(true);
                quarryMapParent.SetActive(false);
            }
            if (quarryToggle.isOn)
            {
                quarryMapParent.SetActive(true);
                wwMapParent.SetActive(false);

            }

            payloadCartLightsHolder.SetActive(false);

            int domButtonCountRed = 0;
            int domButtonCountBlue = 0;

            for (int j = 0; j < 3; j++)
            {
                if (wwMapParent.activeInHierarchy == true)
                {
                    if (data.Data.ButtonInfo[j].Team == -1)
                    {
                        isRedCountDown = false;
                        isBlueCountDown = false;

                        wwMapParts[j].gameObject.GetComponent<MeshRenderer>().material = m_concrete;
                    }
                    if (data.Data.ButtonInfo[j].Team == 0)
                    {
                        isBlueCountDown = false;
                        wwMapParts[j].gameObject.GetComponent<MeshRenderer>().material.color = colorRed;
                        domButtonCountRed++;
                    }
                    if (data.Data.ButtonInfo[j].Team == 1)
                    {
                        isRedCountDown = false;
                        wwMapParts[j].gameObject.GetComponent<MeshRenderer>().material.color = colorBlue;
                        domButtonCountBlue++;
                    }
                }
                if (quarryMapParent.activeInHierarchy == true)
                {
                    if (data.Data.ButtonInfo[j].Team == -1)
                    {
                        isRedCountDown = false;
                        isBlueCountDown = false;

                        quarryMapParts[j].gameObject.GetComponent<MeshRenderer>().material = m_concrete;
                    }
                    if (data.Data.ButtonInfo[j].Team == 0)
                    {
                        isBlueCountDown = false;
                        quarryMapParts[j].gameObject.GetComponent<MeshRenderer>().material.color = colorRed;
                        domButtonCountRed++;
                    }
                    if (data.Data.ButtonInfo[j].Team == 1)
                    {
                        isRedCountDown = false;
                        quarryMapParts[j].gameObject.GetComponent<MeshRenderer>().material.color = colorBlue;
                        domButtonCountBlue++;
                    }
                }

                if (domButtonCountRed == 3)
                {
                    isRedCountDown = true;
                }
                if (domButtonCountBlue == 3)
                {
                    isBlueCountDown = true;
                }
            }
        }
    }
}
