using System;
using UnityEngine;

public class ChangeMMColors : MonoBehaviour
{
    public Domination_Manager mD;

    public int buttonArrayInt = 0;
    public int valueAtButton = -1;

    public int currentIntAtButton = -1;

    public Color colorBlue = new Color32(8, 135, 255, 255);
    public Color colorRed = new Color32(240, 14, 52, 255);
    public Color colorWhite = new Color32(255, 255, 255, 255);

    private void Start()
    {
        Domination_Manager.DominationUpdateMM += DominationUpdateMM;
        mD = mD.GetComponent<Domination_Manager>();
    }

    public void DominationUpdateMM(Root _data)
    {
        Debug.Log("I am base " + this.gameObject.name + "and the button value is: " + mD.domButtonValue[buttonArrayInt].ToString() );
        switch (mD.domButtonValue[buttonArrayInt])
        {
            case -1:
                this.GetComponent<MeshRenderer>().material.color = Color.white;
                currentIntAtButton = -1;
                break;
            case 0:
                this.GetComponent<MeshRenderer>().material.color = colorRed;
                currentIntAtButton = 0;
                break;
            case 1:
                this.GetComponent<MeshRenderer>().material.color = colorBlue;
                currentIntAtButton = 1;
                break;
        }
    }
}
