using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CP_Manager : MonoBehaviour
{
    public Root data;
    public SB_Manager mSB;
    public SocketServer ss;
    //public Payload_Manager mP;
    //public Domination_Manager mD;
    
    [SerializeField]
    private GameObject teamHoldingPanel;

    public int redScore = 0;
    public int blueScore = 0;

    public bool isHolding = false;
    public int teamHolding = -1;

    public bool isCP = false;

    // Start is called before the first frame update
    void Start()
    {
        SocketServer.ControllEvent += ControllUpdate;
        ss = ss.GetComponent<SocketServer>();
        mSB = mSB.GetComponent<SB_Manager>();
        //mD = mD.GetComponent<Domination_Manager>();
        //mP = mP.GetComponent<Payload_Manager>();
    }

    // Update is called once per frame
    void ControllUpdate(Root _data)
    {
        this.data = _data;

        isCP = true;

        if (isCP)
        {         
            blueScore = data.Data.BlueScore;
            redScore = data.Data.RedScore;
        }

        mSB.UpdateFromCP(redScore, blueScore);

    }

    public void GoToMainMenu()
    {
        ss.stopListening();
        SceneManager.LoadScene(1);

    }
}

