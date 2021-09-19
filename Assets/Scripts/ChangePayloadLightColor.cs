using UnityEngine.UI;
using UnityEngine;

public class ChangePayloadLightColor : MonoBehaviour
{
    public Payload_Manager mP;
    public int lightNum;
    public int playersOnCart;

    public Sprite dead;
    public Sprite whiteBar;

    // Start is called before the first frame update
    void Start()
    {
        mP = mP.GetComponent<Payload_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mP.isPayload)
        {
            playersOnCart = mP.poc;

            if (playersOnCart > lightNum)
            {
                this.gameObject.GetComponent<Image>().sprite = whiteBar;
            }
            else
            {
                this.gameObject.GetComponent<Image>().sprite = dead;
            }
        }
    }
}
