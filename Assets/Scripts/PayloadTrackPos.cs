using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayloadTrackPos : MonoBehaviour
{
    public static int trackPosCounter = 0;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I've been hit");

        trackPosCounter++;
    }

}
