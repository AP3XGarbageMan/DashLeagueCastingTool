using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnKF : MonoBehaviour
{
    private IEnumerator crSpawnKF;

    public void StartTheCO()
    {
        Debug.Log("should be starting a coroutine");
        crSpawnKF = RunMe();
        Debug.Log("should be starting a coroutine like for real");
        StartCoroutine(crSpawnKF);
    }

    IEnumerator RunMe ()
    {
        Debug.Log("made it into run me");
        yield return new WaitForSeconds(2f);
    }
}
