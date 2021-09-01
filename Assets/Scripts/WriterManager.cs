using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.IO;

public class WriterManager : MonoBehaviour
{
    public Root data;
    public SB_Manager mSB;

    [SerializeField]
    public Toggle wdToggle;
    [SerializeField]
    public Toggle wdwkwToggle;
    [SerializeField]
    public Toggle wdwkbwToggle;
    [SerializeField]
    public TextMeshProUGUI pathTMP;


    private void Start()
    {
        mSB = mSB.GetComponent<SB_Manager>();
    }

    //public void WriteData()
    //{
    //    if (wdToggle.isOn)
    //    {
    //        StreamWriter sw = new StreamWriter(pathTMP.text);

    //        for (int i = 0; i < data.Data.Names.Length; i++)
    //        {
    //            sw.WriteLine(data.Data.Names[i] + "," + data.Data.Teams[i] + "," + data.Data.Kills[i] + "," + data.Data.Deaths[i]);
    //        }
           
    //        sw.Close();
    //    }
    //}

    public void WriteIt()
    {
        if (wdwkwToggle.isOn)
        {
            WriteWhoKilledWho();
        }

        if (wdwkbwToggle.isOn)
        {
            WriteWhoWasKilledByWho();
        }
    }

    public void WriteWhoKilledWho()
    {
        if (wdwkwToggle.isOn)
        {
            StreamWriter sw = new StreamWriter(pathTMP.text);

            for (int i = 0; i < mSB.pIG.Count; i++)
            {
                sw.WriteLine(mSB.pIG[i].Name);

                string lineTwo = "";

                foreach (string _killed in mSB.pIG[i].hasKilled)
                {
                    lineTwo += _killed + ", ";
                }
                sw.WriteLine(lineTwo);
            }
            sw.Close();
            wdwkwToggle.isOn = false;
        }
    }

    public void WriteWhoWasKilledByWho()
    {
        if (wdwkbwToggle.isOn)
        {
            StreamWriter sw = new StreamWriter(pathTMP.text);

            for (int i = 0; i < mSB.pIG.Count; i++)
            {
                sw.WriteLine(mSB.pIG[i].Name);

                string lineTwo = "";

                foreach (string _kb in mSB.pIG[i].killedBy)
                {
                    lineTwo += _kb + ", ";
                }
                sw.WriteLine(lineTwo);
            }
            sw.Close();
            wdwkbwToggle.isOn = false;
        }
    }
}
