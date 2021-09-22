using UnityEngine;

public class ButtonSenseHit : MonoBehaviour
{
    [SerializeField]
    private Transform[] handsHolder;

    public Domination_Manager mD;

    public int buttonIAm;

    private void Start()
    {
        // :)
        mD = mD.GetComponent<Domination_Manager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (mD.isDom)
        {
            if (mD.isWW)
            {
                // get the name of the gameobject that hit the collider
                string name = other.name;
                // for the player we need to find
                int playerI = 0;

                // go through left hand and right hand parents and find 
                for (int i = 0; i < handsHolder[0].childCount; i++)
                {
                    if (handsHolder[0].GetChild(i).name == name)
                    {
                        playerI = i;
                    }
                    if (handsHolder[1].GetChild(i).name == name)
                    {
                        playerI = i;
                    }

                }

                mD.lastIntToHit = playerI;
            }
        }
    }
}
