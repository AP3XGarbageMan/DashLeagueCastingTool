using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadDotMover : MonoBehaviour
{
    public Payload_Manager mP;

    // Start is called before the first frame update
    void Start()
    {
        mP = mP.GetComponent<Payload_Manager>();    
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position = mP.cartPosSingle;
    }
}
