using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Manager : MonoBehaviour
{
    public Root data;

    public Vector3[] feetPos = new Vector3[10];
    public Vector3[] leftHands = new Vector3[10];
    public Vector3[] rightHands = new Vector3[10];

    private IEnumerator spawn_Orbs;
    public int playerCount = 0;

    [SerializeField]
    private GameObject dotSpriteBlue;
    [SerializeField]
    private GameObject dotSpriteRed;

    [SerializeField]
    private GameObject dotParent;

    private void Start()
    {
        SocketServer.PlayerPosEvent += SetupPP;

        for (int i = 0; i < 10; i++)
        {
            feetPos[i] = new Vector3(0, 0, 0);
            leftHands[i] = new Vector3(0, 0, 0);
            rightHands[i] = new Vector3(0, 0, 0);
        }
    }

    public void SetupPP(Root _data)
    {
        this.data = _data;
        //Debug.Log("length is " + data.Data.FeetPos.X.Length.ToString());
        playerCount = data.Data.FeetPos.X.Length;

        for (int i = 0; i < playerCount; i++)
        {
            feetPos[i] = new Vector3(data.Data.FeetPos.X[i], data.Data.FeetPos.Y[i] + 20.0f, data.Data.FeetPos.Z[i]);
            leftHands[i] = new Vector3(data.Data.LeftHandPos.X[i], data.Data.LeftHandPos.Y[i], data.Data.LeftHandPos.Z[i]);
            rightHands[i] = new Vector3(data.Data.RightHandPos.X[i], data.Data.RightHandPos.Y[i], data.Data.RightHandPos.Z[i]);
        }
        //Debug.Log("length is again..." + playerCount.ToString());
    }

    public void DrawHeatMap()
    {

        for (int i = 0; i < feetPos.Length; i++)
        {
            if (i < 5)
            {
                GameObject newDot = Instantiate(dotSpriteRed, dotParent.transform);
                newDot.transform.position = feetPos[i];
            }
            if (i < 5)
            {
                GameObject newDot = Instantiate(dotSpriteBlue, dotParent.transform);
                newDot.transform.position = feetPos[i];
            }
        }
    }
}
