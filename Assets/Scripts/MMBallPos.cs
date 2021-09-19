using UnityEngine;
using UnityEngine.UI;

public class MMBallPos : MonoBehaviour
{
    public PP_Manager mPP;
    public int iPos;

    // Start is called before the first frame update
    void Start()
    {
        mPP = mPP.GetComponent<PP_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = mPP.feetPos[iPos];
    }
}
