using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Domination_Manager : MonoBehaviour
{
    public Root data;
    public SB_Manager mSB;

    public bool isDomination = false;
    public bool isRedCountDown = false;
    public bool isBlueCountDown = false;
    public bool isRedCountingDown = false;
    public bool isBlueCountingDown = false;
    public int[] buttonInfoTeams = new int[3];

    [SerializeField]
    private TextMeshProUGUI[] teamTopPanelScore;
    [SerializeField]
    private TextMeshProUGUI[] teamSBMapScore;
    [SerializeField]
    private TextMeshProUGUI[] teamCD;

    [SerializeField]
    private Toggle wwToggle;
    [SerializeField]
    private Toggle quarryToggle;
    [SerializeField]
    private Toggle shutDownDomToggle;

    [SerializeField]
    private GameObject wwMMParent;
    [SerializeField]
    private GameObject quarryMMParent;
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

    private int countDownTimerIntR = 0;
    private int countDownTimerIntB = 0;

    private int cdR = 0;
    private int cdB = 0;

    private IEnumerator rcd;
    private IEnumerator bcd;

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
        if (shutDownDomToggle.isOn)
        {
            ShutDownDomination();
        }

        if (isBlueCountDown)
        {
            domTeamColorFlash.gameObject.SetActive(true);
            domTeamColorFlash.GetComponent<Image>().color = colorBlue;
        }
        if (!isBlueCountDown)
        {
            countDownTimerIntB = 5;
        }

        if (isRedCountDown)
        {
            domTeamColorFlash.gameObject.SetActive(true);
            domTeamColorFlash.GetComponent<Image>().color = colorRed;
        }
        if (!isRedCountDown)
        {
            countDownTimerIntR = 5;
        }

        if (!isBlueCountDown && !isRedCountDown)
        {
            domTeamColorFlash.gameObject.SetActive(false);
            countDownTimerIntR = 5;
            countDownTimerIntB = 5;
        }


        if (isDomination)
        {
            
            teamCD[0].text = cdR.ToString();
            teamCD[1].text = cdB.ToString();
            teamTopPanelScore[0].text = data.Data.RedScore.ToString();
            teamSBMapScore[0].text = data.Data.RedScore.ToString();
            teamTopPanelScore[1].text = data.Data.BlueScore.ToString();
            teamSBMapScore[1].text = data.Data.BlueScore.ToString();

            if (wwToggle.isOn)
            {
                wwMMParent.SetActive(true);
                quarryMMParent.SetActive(false);
            }
            if (quarryToggle.isOn)
            {
                quarryMMParent.SetActive(true);
                wwMMParent.SetActive(false);

            }

            payloadCartLightsHolder.SetActive(false);

            int domButtonCountRed = 0;
            int domButtonCountBlue = 0;

            for (int j = 0; j < 3; j++)
            {
                if (wwMMParent.activeInHierarchy == true)
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
                if (quarryMMParent.activeInHierarchy == true)
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
                    //isRedCountingDown = true;
                    //StartCountDownTimer("red");
                }
                if (domButtonCountBlue == 3)
                {
                    isBlueCountDown = true;
                    //isBlueCountingDown = true;
                    //StartCountDownTimer("blue");
                }
            }
        }
    }

    public void ShutDownDomination()
    {
        quarryMMParent.SetActive(false);
        wwMMParent.SetActive(false);
        isRedCountDown = false;
        isBlueCountDown = false;
        wwToggle.isOn = false;
        quarryToggle.isOn = false;
        shutDownDomToggle.isOn = false;
    }

    //public void CountOne(string _team)
    //{
    //    if (_team == "red")
    //        cdR++;
    //}

    //void StartCountDownTimer(string _team)
    //{
    //    if (isRedCountDown || isBlueCountDown)
    //    {
    //        if (_team == "red")
    //        {
    //            rcd = RedCountDown(_team);
    //            StartCoroutine(rcd);
    //        }
    //        if (_team == "blue")
    //        {
    //            bcd = BlueCountDown(_team);
    //            StartCoroutine(bcd);
    //        }
    //    }
    //}

    //IEnumerator RedCountDown(string _team)
    //{
    //    isRedCountingDown = false;
    //    yield return new WaitForSeconds(1f);
    //    isRedCountingDown = true;
    //    countDownTimerIntR--;
    //    if (countDownTimerTMP[0].transform.parent.gameObject.activeInHierarchy == true)
    //    {
    //        countDownTimerTMP[0].text = countDownTimerIntR.ToString();
    //    }
    //    StartCountDownTimer(_team);
    //}

    //IEnumerator BlueCountDown(string _team)
    //{
    //    isBlueCountingDown = false;
    //    yield return new WaitForSeconds(1f);
    //    isBlueCountingDown = true;
    //    countDownTimerIntB--;
    //    if (countDownTimerTMP[1].transform.parent.gameObject.activeInHierarchy == true)
    //    {
    //        countDownTimerTMP[1].text = countDownTimerIntB.ToString();
    //    }
    //    StartCountDownTimer(_team);
    //}
}
