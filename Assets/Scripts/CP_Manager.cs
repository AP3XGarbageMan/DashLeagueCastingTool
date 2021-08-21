using TMPro;
using UnityEngine;

public class CP_Manager : MonoBehaviour
{
    public SB_Manager mSB;
    
    [SerializeField]
    private TextMeshProUGUI[] teamTopPanelScore;
    [SerializeField]
    private TextMeshProUGUI[] teamSBMapScore;

    // Start is called before the first frame update
    void Start()
    {
        mSB = mSB.GetComponent<SB_Manager>();
        SocketServer.ControllEvent += ControllUpdate;
    }

    // Update is called once per frame
    void ControllUpdate(Root data)
    {
        teamTopPanelScore[0].text = data.Data.RedScore.ToString();
        teamSBMapScore[0].text = data.Data.RedScore.ToString();
        teamTopPanelScore[1].text = data.Data.BlueScore.ToString();
        teamSBMapScore[1].text = data.Data.BlueScore.ToString();
    }
}

