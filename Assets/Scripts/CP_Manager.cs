using TMPro;
using UnityEngine;

public class CP_Manager : MonoBehaviour
{
    public Root data;
    public SB_Manager mSB;
    public bool isCP = false;

    [SerializeField]
    private TextMeshProUGUI[] teamTopPanelScore;
    [SerializeField]
    private TextMeshProUGUI[] teamSBMapScore;

    // Start is called before the first frame update
    void Start()
    {
        mSB = mSB.GetComponent<SB_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCP)
        {
            teamTopPanelScore[0].text = data.Data.RedScore.ToString();
            teamSBMapScore[0].text = data.Data.RedScore.ToString();
            teamTopPanelScore[1].text = data.Data.BlueScore.ToString();
            teamSBMapScore[1].text = data.Data.BlueScore.ToString();
        }

    }
}

