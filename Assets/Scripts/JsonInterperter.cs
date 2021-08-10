using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class JsonInterperter : MonoBehaviour
{
    public TextAsset jsonInput;
    [SerializeField] private List<Root> _output;

    private void Start()
    {
        if (jsonInput != null)
            _output = JsonConvert.DeserializeObject<List<Root>>(jsonInput.text);
    }
}

[Serializable]
public class ButtonInfo
{
    public bool Holding;
    public int Team;
}

[Serializable]
public class FeetPos
{
    public float[] X;
    public float[] Y;
    public float[] Z;
}

[Serializable]
public class HeadPos
{
    public float[] X;
    public float[] Y;
    public float[] Z;
}

[Serializable]
public class HeadDir
{
    public float[] X;
    public float[] Y;
    public float[] Z;
}

[Serializable]
public class LeftHandPos
{
    public float[] X;
    public float[] Y;
    public float[] Z;
}

[Serializable]
public class LeftHandDir
{
    public float[] X;
    public float[] Y;
    public float[] Z;
}

[Serializable]
public class RightHandPos
{
    public float[] X;
    public float[] Y;
    public float[] Z;
}

[Serializable]
public class RightHandDir
{
    public float[] X;
    public float[] Y;
    public float[] Z;
}


[Serializable]
public class Data
{
    public string MatchType;
    public string MapName;
    public string[] Ids;
    public int[] Levels;
    public string[] Names;
    public int[] Teams;
    public int[] Kills;
    public int[] Deaths;
    public int[] Scores;
 
    public FeetPos FeetPos;
    public float[] FeetDir;
    public HeadPos HeadPos;
    public HeadDir HeadDir;
    public LeftHandPos LeftHandPos;
    public LeftHandDir LeftHandDir;
    public RightHandPos RightHandPos;
    public RightHandDir RightHandDir;

    public List<ButtonInfo> ButtonInfo;

    public string Victum;
    public string Killer;
    public bool HeadShot;
    public bool IsAltfire;
    public int WeaponsType;
    public int RedPercent;
    public int BluePercent;
    public int PlayersOnCart;
    public int RedScore;
    public int BlueScore;
    public bool Holding;
    public int TeamHolding;
}

[Serializable]
public class Root
{
    public string Type;
    public float TimeStamp;
    public Data Data;
}