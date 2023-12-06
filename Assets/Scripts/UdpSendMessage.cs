using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class UdpSendMessage : MonoBehaviour
{
    const int PORT_NUM = 1998;
    //[SerializeField]
   // public int controllerAction;

    private IPAddress serverAddr;
    private IPEndPoint endPoint;
    private Socket sock;
    private byte[] send_buffer;

    public static UdpSendMessage Instance { get; private set; }

    private const string SCALE = "Scale";
    private const string ROTATE = "Rotate";
    private const string ROTATION = "Rotation";
    private const string STOPROT = "StopRotation";

    private bool startRot = false;
    private GameObject rotationButton;
  
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("==== In Start ===");
        sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        sock.EnableBroadcast = true;
        serverAddr = IPAddress.Parse("255.255.255.255");
        endPoint = new IPEndPoint(serverAddr, PORT_NUM);
        Debug.Log("---- ServerAddr:" + endPoint.Address.ToString());

        rotationButton = GameObject.FindGameObjectWithTag("rotationButton");

        Debug.Log("==== Out Start ===");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRotateButtonClick()
    {
        // 1 - represents ROTATE button click
        Debug.Log("==== OnRotateButtonClick ----");
        SendMessageToClient(1);
    }

    public void OnScaleButtonClick()
    {
        // 2 - represents SCALE button click
        Debug.Log("==== OnScaleButtonClick ---");
        SendMessageToClient(2);
    }

    public void OnRotationClick()
    {
        startRot = true;
        SendMessageToClient(3);
       // rotationButton.tex
    }

    

    public void SendMessageToClient(int index)
    {
        Debug.Log("==== SendMessageToClient ---");
        string msg=null;
        
        if(index == 1) {
            Debug.Log("===== value :1");
            msg = ROTATE;
        }
        else if(index == 2)
        {
            Debug.Log("===== Value : 2");
            msg = SCALE;
        }
        else if(index == 3)
        {
            Debug.Log("===== Value : 3");
            if (startRot == true)
            {
                Debug.Log("--- Rotation ---");
                msg = ROTATION;
            }
            else
            {
                msg = STOPROT;
            }
        }
        Debug.Log("Sending Text Message - " + msg);
        SendPacket(msg);
       

    }

    //Method to send Packet across the socket via UDP
    public void SendPacket(string message)
    {
        Debug.Log("==== In SendPacket ----");
        try
        {
            send_buffer = Encoding.ASCII.GetBytes(message);
            sock.SendTo(send_buffer, endPoint);
            Debug.Log("---- ServerAddr:" + sock.LocalEndPoint.ToString());
            Debug.Log(message);
        }
        catch (SocketException s)
        {
            Debug.Log(s);
        }
        Debug.Log("==== Out SendPacket ----");
    }

    //Application quit method
    private void OnApplicationQuit()
    {
        if (sock != null)
        {
            sock.Dispose();
            sock.Close();
        }
    }
}
