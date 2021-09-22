using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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

    // These events can be sub to. so the socketserver doesn't need to know what to call 
    public static event Action<Root> KillFeedEvent;
    public static event Action<Root> KFEvent;
    public static event Action<Root> StartEvent; 
    public static event Action<Root> ScoreBoardEvent;
    public static event Action<Root> PlayerPosEvent; 
    public static event Action<Root> PayloadEvent;
    public static event Action<Root> DominationEvent; 
    public static event Action<Root> ControllEvent;

    public List<string> jsonOut = new List<string>();

    private void Start()
    {
        _tcpServer = new TcpListener(IPAddress.Parse("127.0.0.1"), 3333);
        _tcpServer.Start();
        _tcpServer.BeginAcceptTcpClient(TcpConnectionCallback, null);
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

            // There has to be a better way to do this
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

            jsonOut.Add(convertedData);
            dataInspector = JsonConvert.DeserializeObject<Root>(convertedData);

            dataREADER(dataInspector);
        }
    }

    //private void Update()
    //{
    //    // Fake Data Creator
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        var fakeData = new Root();
    //        fakeData.Data = new Data();
    //        fakeData.Type = "Dead";
    //        fakeData.Data.Killer = "My mom";
    //        fakeData.Data.Victum = "Yourself";
    //        fakeData.Data.HeadShot = true;
    //        fakeData.Data.IsAltfire = false;
    //        fakeData.Data.WeaponsType = 0;

    //        dataREADER(fakeData);
    //    }
    //}


    void dataREADER(Root data)
    {
        switch (data.Type)
        {
            case "Start": StartEvent?.Invoke(data); break;
            case "Dead": KFEvent?.Invoke(data); break;
            case "PP": PlayerPosEvent?.Invoke(data); break;
            case "ScoreBoard": ScoreBoardEvent?.Invoke(data); break;
            case "Payload": PayloadEvent?.Invoke(data); break;
            case "Domination": DominationEvent?.Invoke(data); break;
            case "Controll": ControllEvent?.Invoke(data); break;
            
            default:
                Debug.Log($"NOT used data type \"{data.Type}\"");
                break;
        }
    }

    public void stopListening()
    {
        _tcpServer.Stop();
    }
}
