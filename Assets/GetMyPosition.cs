using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMyPosition : MonoBehaviour
{
    public HDPlayer hdp;
    public int iPos;

    // Start is called before the first frame update
    void Start()
    {
        hdp = hdp.GetComponent<HDPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = hdp.newFeetPos[iPos];
    }
}
