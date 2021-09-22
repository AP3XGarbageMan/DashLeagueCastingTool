using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System;

public class Payload_Manager : MonoBehaviour
{
    private Root data;

    [SerializeField]
    private TextMeshProUGUI[] teamTopPanelScore;
    [SerializeField]
    private TextMeshProUGUI[] teamSBMapScore;

    public GameObject cart;

    public GameObject partyBusTMP;

    public List<Vector3> cartPos = new List<Vector3>();
    public Vector3 cartPosSingle = new Vector3(0, 0, 0);

    public int poc = 0;

    public bool isPayload;
    public bool isCanyon;
    public bool isLP;

    public static event Action<Root> PayloadUpdateOut;
    public static event Action<Root> PayloadUpdateCartGraphOut;

    public List<PayloadPercentTracker> payloadCartTrackerList = new List<PayloadPercentTracker>();

    private void OnEnable()
    {
        SocketServer.PayloadEvent += PayloadUpdate;
    }

    // Update is called once per frame
    void PayloadUpdate(Root _data)
    {   
        isPayload = true;

        if (isPayload == true)
        {            
            this.data = _data;
            cartPos.Add(new Vector3(data.Data.X, data.Data.Y, data.Data.Z));
            cartPosSingle = new Vector3(data.Data.X, data.Data.Y, data.Data.Z);

            poc = data.Data.PlayersOnCart;
            
            payloadCartTrackerList.Add(new PayloadPercentTracker(data.TimeStamp, data.Data.RedPercent, data.Data.BluePercent));

            PayloadUpdateOut?.Invoke(_data);

        }        
    }

    public void ChangeVSColor(GameObject[] _vs, Color _color)
    {
        for (int i = 0; i < 7; i++)
        {
            _vs[i].GetComponent<Image>().color = _color;
        }
    }
}
