using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopulateBiggerScoreBoard : MonoBehaviour
{
    public SB_Manager mSB;
    public Domination_Manager mD;

    [SerializeField]
    private TextMeshProUGUI[] playerBT;
    [SerializeField]
    private TextMeshProUGUI[] playerCD;

    [SerializeField]
    private TextMeshProUGUI[] teamButtonTapsTMP;
    [SerializeField]
    private TextMeshProUGUI[] teamCountDownsTMP;

    // Start is called before the first frame update
    void Start()
    {
        mSB = mSB.GetComponent<SB_Manager>();
        mD = mD.GetComponent<Domination_Manager>();
    }

    private void Update()
    {
        if (mD.isDom)
        {
            for (int i = 0; i < mSB.playerNamesWithSpaces.Length; i++)
            {
                playerBT[i].text = mSB.pIG[i].totalButtonTaps.ToString();
                playerCD[i].text = mSB.pIG[i].countDownsStarted.ToString();

            }
            for (int i = 0; i < 2; i++)
            {
                teamButtonTapsTMP[i].text = mD.buttontaps[i].ToString();
            }
            teamCountDownsTMP[0].text = mD.redCountDowns.ToString();
            teamCountDownsTMP[1].text = mD.blueCountDowns.ToString();
        }
    }
}
