using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public SB_Manager mSB;
    public int playerPos;
    public Transform healthBarImage;

    private float currentAmount = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        mSB = mSB.GetComponent<SB_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        currentAmount = mSB.currentHealth[playerPos];
        healthBarImage.GetComponent<Image>().fillAmount = (currentAmount / 100);
    }
}
