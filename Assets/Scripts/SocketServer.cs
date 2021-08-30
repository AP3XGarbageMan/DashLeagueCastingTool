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
    private TcpListener _tcpServer;
    private TcpClient tcpClient;
    private NetworkStream stream;
    private byte[] reciveBuffer;
    private int bufferSize = 4096;

    [SerializeField] private Root dataInspector;
    private Dictionary<string, Vector3> _playerposAsLastSeen = new Dictionary<string, Vector3>();
    public static Dictionary<string, Vector3> playerPositions = new Dictionary<string, Vector3>();
    public static string[] ppArray;   
    private Dictionary<int, string> _indexToPlayername = new Dictionary<int, string>();

    public SB_Manager mSB;
    public Payload_Manager mPL;
    public Domination_Manager mD;
    public CP_Manager mCP;
    public KF_Manager mKF;
    public PP_Manager mPP;
    //public WriterManager mWD;

    // These events can be sub to. so the socketserver doesn't need to know what to call 
    //public static event Action<Root> KillFeedEvent;
    //public static event Action<Root> StartEvent;
    //public static event Action<Root> ScoreBoardEvent;
    //public static event Action<Root> PlayerPosEvent;
    //public static event Action<Root> PayloadEvent;
    //public static event Action<Root> DominationEvent;
    //public static event Action<Root> ControllEvent;

    private void Start()
    {
        _tcpServer = new TcpListener(IPAddress.Parse("127.0.0.1"), 3333);
        _tcpServer.Start();
        _tcpServer.BeginAcceptTcpClient(TcpConnectionCallback, null);

        //KFm = GetComponent<KF_Manager>();
        mSB = mSB.GetComponent<SB_Manager>();
        mPL = mPL.GetComponent<Payload_Manager>();
        mD = mD.GetComponent<Domination_Manager>();
        mCP = mCP.GetComponent<CP_Manager>();
        mKF = mKF.GetComponent<KF_Manager>();
        mPP = mPP.GetComponent<PP_Manager>();

        //mWD = mWD.GetComponent<WriterManager>();
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

            dataREADER(dataInspector);
        }
    }


    void dataREADER(Root data)
    {
        // Debug.Log("reading data");

        //switch (data.Type)
        //{
        //    case "Start": StartEvent?.Invoke(data); break;
        //    case "Dead": KillFeedEvent?.Invoke(data); break;
        //    case "PP": PlayerPosEvent?.Invoke(data); break;
        //    case "ScoreBoard": ScoreBoardEvent?.Invoke(data); break;
        //    case "Payload": PayloadEvent?.Invoke(data); break;
        //    case "Domination": DominationEvent?.Invoke(data); break;
        //    case "Controll": ControllEvent?.Invoke(data); break;

        //    default:
        //        Debug.Log($"NOT used data type \"{data.Type}\"");
        //        break;
        //}

        switch (data.Type)
        {
            case "Dead":
                mKF.data = data;
                mKF.killHappened = true;
                break;
            case "PP":
                mPP.data = data;
                mPP.SetupPP();
                break;
            case "ScoreBoard":
                mSB.data = data;
                mSB.SetScoreBoard();
                //mWD.data = data;
                //mWD.WriteData();
                break;
            case "Payload":
                mPL.data = data;
                mD.isDomination = false;
                mPL.isPayload = true;
                mCP.isCP = false;
                break;
            case "Domination":
                mD.data = data;
                mD.isDomination = true;
                mPL.isPayload = false;
                mCP.isCP = false;
                break;
            case "Controll":
                mCP.data = data;
                mD.isDomination = false;
                mPL.isPayload = false;
                mCP.isCP = true;
                break;
        }
    }
}
