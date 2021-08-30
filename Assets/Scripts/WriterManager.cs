using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.IO;

public class WriterManager : MonoBehaviour
{
    public Root data;

    [SerializeField]
    public Toggle wdToggle;
    [SerializeField]
    public TextMeshProUGUI pathTMP;

    
    public void WriteData()
    {
        if (wdToggle.isOn)
        {
            StreamWriter sw = new StreamWriter(pathTMP.text);

            for (int i = 0; i < data.Data.Names.Length; i++)
            {
                sw.WriteLine(data.Data.Names[i] + "," + data.Data.Teams[i] + "," + data.Data.Kills[i] + "," + data.Data.Deaths[i]);
            }
           
            sw.Close();
        }
    }
}
