using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DataGraph : MonoBehaviour
{
    public Domination_Manager mD;
    public Payload_Manager mP;
    public PopulateMainScoreboard pMSB;
    
    [SerializeField]
    private Sprite dotSpriteBlue;
    [SerializeField]
    private Sprite dotSpriteRed;

    [SerializeField]
    private GameObject graphContainer;

    [SerializeField]
    private GameObject redTeamHolder;
    [SerializeField]
    private GameObject blueTeamHolder;

    private List<Vector3> dataPointsBT = new List<Vector3>();
    public List<Vector3> cartPercentBlue = new List<Vector3>();
    public List<Vector3> cartPercentRed = new List<Vector3>();

    public float yMaxVal = 100.0f;
    public float xMaxVal = 0;
    public float zMaxVal = 100.0f;
    public float gameStartTime = 0.0f;

    public int whereCartChanged = 0;

    private void Start()
    {
        mD = mD.GetComponent<Domination_Manager>();
        mP = mP.GetComponent<Payload_Manager>();
        pMSB = pMSB.GetComponent<PopulateMainScoreboard>();
    }

    private void Update()
    {
        if (mP.isPayload)
        {
            PlotPayloadPercentTracker();
        }
    }

    public void PlotPayloadPercentTracker()
    {
        // Plot only blue (at start) team data
        if (!pMSB.payloadSwapped)
        {
            cartPercentBlue.Clear();

            foreach (Transform child in blueTeamHolder.transform)
            {
                if (child.name == "dataPoint")
                {
                    Destroy(child.gameObject);
                }
            }
            for (int i = 0; i < mP.payloadCartTrackerList.Count; i++)
            {
                cartPercentBlue.Add(new Vector3(mP.payloadCartTrackerList[i].Time, mP.payloadCartTrackerList[i].redPayloadPercent, mP.payloadCartTrackerList[i].bluePayloadPercent));
            }
            whereCartChanged = mP.payloadCartTrackerList.Count;

            // (vector3 - time, red, blue), sprite, time start, parent
            ShowGraphSingle(cartPercentBlue, dotSpriteBlue, cartPercentBlue[0].x, blueTeamHolder);
        }

        // Plot only red (at start) team data
        if (pMSB.payloadSwapped)
        {
            cartPercentRed.Clear();

            foreach (Transform child in redTeamHolder.transform)
            {
                if (child.name == "dataPoint")
                {
                    Destroy(child.gameObject);
                }
            }
            for (int i = (whereCartChanged -1); i < mP.payloadCartTrackerList.Count; i++)
            {
                cartPercentRed.Add(new Vector3(mP.payloadCartTrackerList[i].Time, mP.payloadCartTrackerList[i].redPayloadPercent, mP.payloadCartTrackerList[i].bluePayloadPercent));
            }
            ShowGraphSingle(cartPercentRed, dotSpriteRed, cartPercentRed[0].x, redTeamHolder);
        }
    }

    public void PlotDominationButtonTaps()
    {
        dataPointsBT.Clear();

        foreach (Transform child in graphContainer.transform)
        {
            if (child.name == "dataPoint")
            {
                Destroy(child.gameObject);
            }
        }

        for (int i = 0; i < mD.domBTStats.Count; i++)
        {
            dataPointsBT.Add(new Vector3(mD.domBTStats[i].Time, mD.domBTStats[i].ButtonTapsRed, mD.domBTStats[i].ButtonTapsBlue));
        }
        ShowGraph(dataPointsBT, dotSpriteRed, dotSpriteBlue);
    }

    private void DrawDot(Vector2 _anchoredPosition, Sprite _dot, GameObject _parent)
    {
        GameObject dot = new GameObject("data", typeof(Image));
        dot.name = "dataPoint";
        dot.transform.SetParent(_parent.transform, false);
        dot.GetComponent<Image>().sprite = _dot;
        RectTransform rectTransform = dot.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = _anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
    }

    public void ShowGraph(List<Vector3> _dataPoints, Sprite _dotColorR, Sprite _dotColorB)
    {
        //buttonTapListRed = mD.domBTStatsRed;
        FindMaxY(_dataPoints);
        //FindMaxX(_dataPoints);
        FindMaxZ(_dataPoints);

        float graphHeight = graphContainer.GetComponent<RectTransform>().sizeDelta.y;
        float graphWidth = graphContainer.GetComponent<RectTransform>().sizeDelta.x;

        for (int i = 0; i < _dataPoints.Count; i++) 
        {
            float xPos = ((_dataPoints[i].x - _dataPoints[0].x) / 1500) * graphWidth;
            float yPos = (_dataPoints[i].y / yMaxVal) * graphHeight;
            float zPos = (_dataPoints[i].z / zMaxVal) * graphHeight;

            DrawDot(new Vector2(xPos, yPos), _dotColorR, graphContainer);
            DrawDot(new Vector2(xPos, zPos), _dotColorB, graphContainer);
        }
    }

    public void ShowGraphSingle(List<Vector3> _dataPoints, Sprite _dotColor, float _swapTime, GameObject _parent)
    {     
        //FindMaxX(_dataPoints);
        //FindMaxZ(_dataPoints);

        float graphHeight = graphContainer.GetComponent<RectTransform>().sizeDelta.y;
        float graphWidth = graphContainer.GetComponent<RectTransform>().sizeDelta.x;

        for (int i = 0; i < _dataPoints.Count; i++)
        {
            float xPos = ((_dataPoints[i].x - _swapTime) / 800.0f) * graphWidth;
            float zPos = (_dataPoints[i].z / 110.0f) * graphHeight;

            DrawDot(new Vector2(xPos, zPos), _dotColor, _parent);
        }
    }

    public void FindMaxY(List<Vector3> _DBT)
    {
        float maxVal = 0;

        for (int i = 0; i < _DBT.Count; i++)
        {
            if (_DBT[i].y > maxVal)
            {
                maxVal = _DBT[i].y;
            }
        }
        yMaxVal = maxVal;
    }
    public void FindMaxX(List<Vector3> _DBT)
    {
        float maxVal = 0;
        for (int i = 0; i < _DBT.Count; i++)
        {
            if (_DBT[i].x > maxVal)
            {
                maxVal = _DBT[i].x;
            }
        }
        xMaxVal = maxVal;
    }
    public void FindMaxZ(List<Vector3> _DBT)
    {
        float maxVal = 0;
        for (int i = 0; i < _DBT.Count; i++)
        {
            if (_DBT[i].z > maxVal)
            {
                maxVal = _DBT[i].z;
            }
        }
        zMaxVal = maxVal;
    }

    public void DestroyGraph()
    {
        Destroy(this.gameObject);
    }
}
