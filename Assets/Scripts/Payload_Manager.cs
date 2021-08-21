using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Payload_Manager : MonoBehaviour
{
    // public Root data;
    public SB_Manager mSB;
    
    [SerializeField]
    private TextMeshProUGUI[] teamTopPanelScore;
    [SerializeField]
    private TextMeshProUGUI[] teamSBMapScore;

    [SerializeField]
    private GameObject payloadCartLightsHolder;

    [SerializeField]
    private GameObject[] payloadCartLights;

    // Start is called before the first frame update
    void Start()
    {
        mSB = mSB.GetComponent<SB_Manager>();
        SocketServer.PayloadEvent += PayloadUpdate;
    }

    // Update is called once per frame
    void PayloadUpdate(Root data)
    {
        payloadCartLightsHolder.SetActive(true);

        teamTopPanelScore[0].text = data.Data.RedPercent.ToString();
        teamSBMapScore[0].text = data.Data.RedPercent.ToString();
        teamTopPanelScore[1].text = data.Data.BluePercent.ToString();
        teamSBMapScore[1].text = data.Data.BluePercent.ToString();

        payloadCartLightsHolder.SetActive(true);

        if (data.Data.PlayersOnCart == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                payloadCartLights[i].GetComponent<Image>().color = Color.black;
            }
        }
        if (data.Data.PlayersOnCart == 1)
        {
            payloadCartLights[0].GetComponent<Image>().color = Color.white;
            payloadCartLights[1].GetComponent<Image>().color = Color.black;
            payloadCartLights[2].GetComponent<Image>().color = Color.black;
        }
        if (data.Data.PlayersOnCart == 2)
        {
            for (int i = 0; i < 2; i++)
            {
                payloadCartLights[i].GetComponent<Image>().color = Color.white;
            }
            payloadCartLights[2].GetComponent<Image>().color = Color.black;
        }
        if (data.Data.PlayersOnCart == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                payloadCartLights[i].GetComponent<Image>().color = Color.white;
            }
        }

    }
}
