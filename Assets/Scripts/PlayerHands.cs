using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    public PP_Manager mPP;
    public int iPos;

    public bool isRight;

    // Start is called before the first frame update
    void Start()
    {
        // get off my back. this is comfy. 
        mPP = mPP.GetComponent<PP_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        // check if we manually set our bool on the GameObject to true, if not it is a left hand
        if (isRight)
        {
            this.gameObject.transform.position = mPP.rightHands[iPos];
        }
        else 
        {
            this.gameObject.transform.position = mPP.leftHands[iPos];
        }

    }
}
