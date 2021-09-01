using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Payload_Manager : MonoBehaviour
{
    public Root data;
    public SB_Manager mSB;
    public bool isPayload = false;

    [SerializeField]
    private TextMeshProUGUI[] teamTopPanelScore;
    [SerializeField]
    private TextMeshProUGUI[] teamSBMapScore;

    [SerializeField]
    private GameObject payloadCartLightsHolder;
    [SerializeField]
    private GameObject lpMapParent;
    [SerializeField]
    private GameObject canyonMMMapParent;
    [SerializeField]
    private GameObject partyBus;
    [SerializeField]
    private GameObject[] payloadCartLights;

    [SerializeField]
    private Toggle lpMMToggle;
    [SerializeField]
    private Toggle canyonMMToggle;

    public List<Vector3> payloadPos = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        mSB = mSB.GetComponent<SB_Manager>();
        payloadCartLightsHolder.SetActive(false);
    }

    public void TogglePartyBus(bool _switch)
    {
        partyBus.gameObject.SetActive(_switch);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPayload)
        {
            if (lpMMToggle.isOn)
            {
                lpMapParent.SetActive(true);
            }
            else
            {
                lpMapParent.SetActive(false);  
            }
            if (canyonMMToggle.isOn)
            {
                canyonMMMapParent.SetActive(true);
            }
            else
            {
                canyonMMMapParent.SetActive(false);
            }

            payloadCartLightsHolder.SetActive(true);


            teamTopPanelScore[0].text = data.Data.RedPercent.ToString();
            teamSBMapScore[0].text = data.Data.RedPercent.ToString();
            teamTopPanelScore[1].text = data.Data.BluePercent.ToString();
            teamSBMapScore[1].text = data.Data.BluePercent.ToString();

            payloadCartLightsHolder.SetActive(true);

            if (data.Data.PlayersOnCart == 0)
            {
                partyBus.SetActive(false);
                for (int i = 0; i < 3; i++)
                {
                    payloadCartLights[i].GetComponent<Image>().color = Color.black;
                }
            }
            if (data.Data.PlayersOnCart == 1)
            {
                partyBus.SetActive(false);
                payloadCartLights[0].GetComponent<Image>().color = Color.white;
                payloadCartLights[1].GetComponent<Image>().color = Color.black;
                payloadCartLights[2].GetComponent<Image>().color = Color.black;
            }
            if (data.Data.PlayersOnCart == 2)
            {
                partyBus.SetActive(false);
                for (int i = 0; i < 2; i++)
                {
                    payloadCartLights[i].GetComponent<Image>().color = Color.white;
                }
                payloadCartLights[2].GetComponent<Image>().color = Color.black;
            }
            if (data.Data.PlayersOnCart == 3)
            {
                partyBus.SetActive(false);
                for (int i = 0; i < 3; i++)
                {
                    payloadCartLights[i].GetComponent<Image>().color = Color.white;
                }
            }
            if (data.Data.PlayersOnCart > 3)
            {
                // party bus!!!!
                for (int i = 0; i < 3; i++)
                {
                    partyBus.SetActive(true);
                }
            }
        }

    }
}
