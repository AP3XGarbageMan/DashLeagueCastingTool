using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MM_Manager : MonoBehaviour
{
    public Domination_Manager mD;
    public Payload_Manager mP;

    [SerializeField]
    private GameObject mmWW;
    [SerializeField]
    private GameObject mmQuarry;
    [SerializeField]
    private GameObject mmCanyon;
    [SerializeField]
    private GameObject mmLaunchpad;
    [SerializeField]
    private GameObject mmStadium;

    [SerializeField]
    private Toggle mmWWToggle;
    [SerializeField]
    private Toggle mmQuarryToggle;
    [SerializeField]
    private Toggle mmCanyonToggle;
    [SerializeField]
    private Toggle mmLPToggle;
    [SerializeField]
    private Toggle mmStadiumToggle;


    [SerializeField]
    private TextMeshProUGUI mapNameProgressGraph;

    private void Start()
    {
        mD = mD.GetComponent<Domination_Manager>();
        mP = mP.GetComponent<Payload_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mmWWToggle.isOn)
        {
            TurnOffAllMMMaps();
            TurnOnMM("Waterway");
            mD.isWW = true;
            mapNameProgressGraph.text = "Waterway";
        }
        if (mmQuarryToggle.isOn)
        {
            TurnOffAllMMMaps();
            TurnOnMM("Quarry");
            mD.isQuarry = true;
            mapNameProgressGraph.text = "Quarry";
        }
        if (mmCanyonToggle.isOn)
        {
            TurnOffAllMMMaps();
            TurnOnMM("Canyon");
            mP.isCanyon = true;
            mapNameProgressGraph.text = "Canyon";
        }
        if (mmLPToggle.isOn)
        {
            TurnOffAllMMMaps();
            TurnOnMM("Launchpad");
            mP.isLP = true;
            mapNameProgressGraph.text = "Launchpad";
        }
        if (mmStadiumToggle.isOn)
        {
            TurnOffAllMMMaps();
            TurnOnMM("Stadium");
            mapNameProgressGraph.text = "Stadium";
        }
    }

    public void TurnOffAllMMMaps()
    {
        mmCanyon.SetActive(false);
        mmLaunchpad.SetActive(false);
        mmWW.SetActive(false);
        mmQuarry.SetActive(false);
        mmStadium.SetActive(false);
    }

    public void TurnOnMM(string _map)
    {
        switch (_map)
        {
            case "Waterway":
                mmWW.SetActive(true);
                mmWWToggle.isOn = false;
                break;
            case "Quarry":
                mmQuarry.SetActive(true);
                mmQuarryToggle.isOn = false;
                break;
            case "Canyon":
                mmCanyon.SetActive(true);
                mmCanyonToggle.isOn = false;
                break;
            case "Launchpad":
                mmLaunchpad.SetActive(true);
                mmLPToggle.isOn = false;
                break;
            case "Stadium":
                mmStadium.SetActive(true);
                mmStadiumToggle.isOn = false;
                break;
        }

    }
}
