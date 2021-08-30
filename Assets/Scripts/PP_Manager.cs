using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Manager : MonoBehaviour
{
    public Root data;
    public HDPlayer hdPlayer;

    public Vector3[] feetPos = new Vector3[10];

    private IEnumerator spawn_Orbs;
    public int playerCount = 0;

    private void Start()
    {
        hdPlayer = hdPlayer.GetComponent<HDPlayer>();
        for (int i = 0; i < 10; i++)
        {
            feetPos[i] = new Vector3(0, 0, 0);
        }
    }

    public void SetupPP()
    {
        Debug.Log("length is " + data.Data.FeetPos.X.Length.ToString());
        playerCount = data.Data.FeetPos.X.Length;

        for (int i = 0; i < playerCount; i++)
        {
            feetPos[i] = new Vector3(data.Data.FeetPos.X[i], data.Data.FeetPos.Y[i], data.Data.FeetPos.Z[i]);
        }
        Debug.Log("length is again..." + playerCount.ToString());
        
        hdPlayer.SetupPP();
    }
    
}
