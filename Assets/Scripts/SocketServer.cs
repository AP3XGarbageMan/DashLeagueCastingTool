using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using TMPro;

public class SocketServer : MonoBehaviour
{
    public int[] buttonInfo;
    private TcpListener _tcpServer;
    private TcpClient tcpClient;
    private NetworkStream stream;
    private byte[] reciveBuffer;
    private int bufferSize = 4096;

    public static bool staticIsHeadshot = false;
    public static bool staticKillHappened = false;
    public static bool staticReadingData = false;
    public static bool staticIsPayload = false;
    public static bool staticIsDomination = false;
    public static bool staticIsCP = false;
    
    public static bool[] staticHolding = { false, false, false };

    public static string[] staticPlayerNamesList = { "name", "name", "name", "name", "name", "name", "name", "name", "name", "name" };
    public static string[] staticVictumKiller = { "name", "name" };
    public static string staticMapName = "map";

    public static List<string> jsonOut = new List<string>();

    public static int[] staticPlayerKillList = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public static int[] staticPlayerDeathList = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public static int[] staticHeadShotCounter = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public static int[] staticTeamList = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public static int[] totalTeamKills = { 0, 0 };
    public static int[] totalTeamDeaths = { 0, 0 };
    public static int[] totalTeamsHS = { 0, 0 };
    public static int[] staticTeamButton = { -1, -1, -1 };
    public static int[] buttonInfoTeams = { 0, 0, 0 };

    public static int staticGunKillInt = 0;
    public static int staticRedPercent = 0;
    public static int staticBluePercent = 0;
    public static int staticPlayerOnCart = 0;
    public static int staticRedPointDom = 0;
    public static int staticBluePointDom = 0;
    public static int staticRedPointCp = 0;
    public static int staticBluePointCp = 0; 

    public static float[] totalTeamKD = { 0, 0 };

    [SerializeField] private Root dataInspector;
    private Dictionary<string, Vector3> _playerposAsLastSeen = new Dictionary<string, Vector3>();
    public static Dictionary<string, Vector3> playerPositions = new Dictionary<string, Vector3>();
    public static string[] ppArray;   
    private Dictionary<int, string> _indexToPlayername = new Dictionary<int, string>();



    //public static float timeStamp;
    //public string filePath = "";

    //public static List<string> jsonOut = new List<string>();


    public void ResetData()
    {
        staticIsHeadshot = false;
        staticKillHappened = false;
        staticReadingData = false;
        staticGunKillInt = 0;
        staticRedPercent = 0;
        staticBluePercent = 0;
        staticPlayerOnCart = 0;

        for (int i = 0; i < 3; i++)
        {
            staticHolding[i] = false;
            staticTeamButton[i] = -1;
        }

        for (int i = 0; i < 10; i++)
        {
            staticPlayerKillList[i] = 0;
            staticPlayerDeathList[i] = 0;
            staticHeadShotCounter[i] = 0;
            staticTeamList[i] = 0;
            staticPlayerNamesList[i] = "name";
        }

        for (int i = 0; i < 2; i++)
        {
            staticVictumKiller[i] = "name";
            totalTeamKills[i] = 0;
            totalTeamDeaths[i] = 0;
            totalTeamsHS[i] = 0;
            totalTeamKD[i] = 0;
        }

        jsonOut.Clear();
    }
    private void Start()
    {
        _tcpServer = new TcpListener(IPAddress.Parse("127.0.0.1"), 3333);
        _tcpServer.Start();
        _tcpServer.BeginAcceptTcpClient(TcpConnectionCallback, null);


        //var thing = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // thing.transform.position = Vector3.zero;

    }

    // I HATE C# CALLBACKS WHYYYYYYYYYYYY this is so dumb
    void TcpConnectionCallback(IAsyncResult result)
    {
        tcpClient = _tcpServer.EndAcceptTcpClient(result);
        _tcpServer.BeginAcceptTcpClient(TcpConnectionCallback, null);

        tcpClient.ReceiveBufferSize = bufferSize;
        tcpClient.SendBufferSize = bufferSize;

        stream = tcpClient.GetStream();
        reciveBuffer = new byte[bufferSize];
        stream.BeginRead(reciveBuffer, 0, bufferSize, ReceiveCallback, reciveBuffer);
    }

    void ReceiveCallback(IAsyncResult _result)
    {
        int length = stream.EndRead(_result);

        if (length <= 0)
        {
            tcpClient.Close();
        }
        else
        {
            stream.BeginRead(reciveBuffer, 0, bufferSize, ReceiveCallback, null);
            byte[] array = new byte[length];
            Array.Copy(reciveBuffer, array, length);

            string stringdata = Encoding.ASCII.GetString(array);
            string convertedData = String.Empty;

            int barsopen = 0;

            foreach (char c in stringdata)
            {
                convertedData += c;

                // WHAT ???? dammit c#
                if (c == "{"[0])
                {
                    barsopen++;
                }
                else if (c == "}"[0])
                {
                    barsopen--;
                    if (barsopen == 0)
                    {
                        break;
                    }
                }
            }

            dataInspector = JsonConvert.DeserializeObject<Root>(convertedData);
            jsonOut.Add(convertedData);
            dataREADER(dataInspector);


        }
    }


    void dataREADER(Root data)
    {
        Debug.Log("reading data");
        staticReadingData = true;
        switch (data.Type)
        {
            case "Dead":
                //Debug.Log(data.Type);
                // victum, killer
                staticVictumKiller[0] = data.Data.Victum;
                //Debug.Log("Killer = " + staticVictumKiller[1]);
                staticVictumKiller[1] = data.Data.Killer;
                //Debug.Log("Victum = " + staticVictumKiller[0]);
                staticIsHeadshot = data.Data.HeadShot;
                staticGunKillInt = data.Data.WeaponsType;
                //Debug.Log(data.Data.Killer + " Killed " + data.Data.Victum + " with a " + data.Data.WeaponsType.ToString());
                // Have name, need to add a point. 
                for (int i = 0; i < staticPlayerNamesList.Length; i++)
                {
                    if (staticPlayerNamesList[i] == data.Data.Killer)
                    {
                        if (data.Data.HeadShot)
                        {
                            staticHeadShotCounter[i]++;
                            if (staticTeamList[i] == 0)
                            {
                                // add one to red
                                totalTeamsHS[0]++;
                            }
                            else
                            {
                                // add one to blue
                                totalTeamsHS[1]++;
                            }
                        }

                        if (staticTeamList[i] == 0)
                        {
                            totalTeamKills[0]++;
                        }
                        if (staticTeamList[i] == 1)
                        {
                            totalTeamKills[1]++;
                        }
                    }
                    if (staticPlayerNamesList[i] == data.Data.Victum)
                    {
                        if (staticTeamList[i] == 0)
                        {
                            totalTeamDeaths[0]++;
                        }
                        if (staticTeamList[i] == 1)
                        {
                            totalTeamDeaths[1]++;
                        }
                    }
                }



                staticKillHappened = true;

                break;
            case "PP":
                //Debug.Log(data.Type);
                //if (_indexToPlayername.Count == 0) return;

                for (var j = 0; j < data.Data.FeetPos.X.Length; j++)
                {
                    playerPositions.Add(SocketServer.staticPlayerNamesList[j], new Vector3(data.Data.FeetPos.X[j], data.Data.FeetPos.Y[j], data.Data.FeetPos.Z[j]));
                }

                //_playerposAsLastSeen[_indexToPlayername[i]] = new Vector3(data.Data.FeetPos.X[i], data.Data.FeetPos.Y[i], data.Data.FeetPos.Z[i]);

                break;
            case "ScoreBoard":
                //Debug.Log(data.Type);

                for (int j = 0; j < data.Data.Names.Length; j++)
                {
                    staticPlayerNamesList[j] = data.Data.Names[j];
                    staticPlayerKillList[j] = data.Data.Kills[j];
                    staticPlayerDeathList[j] = data.Data.Deaths[j];
                    staticTeamList[j] = data.Data.Teams[j];
 
                }
                break;
            case "Payload":
                staticIsPayload = true;
                staticBluePercent = data.Data.BluePercent;
                staticRedPercent = data.Data.RedPercent;
                staticPlayerOnCart = data.Data.PlayersOnCart;
                //Debug.Log("Blue percent = " + staticBluePercent.ToString() );
                break;
            case "Domination":
                // Debug.Log("type = " + data.Type);
                staticIsDomination = true;
                int bcount = data.Data.ButtonInfo.Count;
                // Debug.Log("buttonInfo count = " + bcount.ToString());

                for (int i = 0; i < 3; i++)
                {
                    buttonInfoTeams[i] = data.Data.ButtonInfo[i].Team;
                }

                staticBluePointDom = data.Data.BlueScore;
                staticRedPointDom = data.Data.RedScore;
                staticMapName = data.Data.MapName;
                Debug.Log("Map name = " + data.Data.MapName);

                //for (int i = 0; i < data.Data.ButtonInfo.Team.Length; i++)
                //{
                //    staticTeamButton[i] = data.Data.ButtonInfo.Team[i];
                //    staticHolding[i] = data.Data.ButtonInfo.Holding[i];
                //    Debug.Log("Team button info = " + staticTeamButton[i].ToString());
                //    Debug.Log("Team holding button = " + staticHolding[i].ToString());
                //}
                break;
            case "Controll":
                Debug.Log("type = " + data.Type);
                staticIsCP = true;

                staticRedPointCp = data.Data.RedScore;
                staticBluePointCp = data.Data.BlueScore;

                //for (int i = 0; i < data.Data.ButtonInfo.Team.Length; i++)
                //{
                //    staticTeamButton[i] = data.Data.ButtonInfo.Team[i];
                //    staticHolding[i] = data.Data.ButtonInfo.Holding[i];
                //    Debug.Log("Team button info = " + staticTeamButton[i].ToString());
                //    Debug.Log("Team holding button = " + staticHolding[i].ToString());
                //}
                break;
        }
    }
}
