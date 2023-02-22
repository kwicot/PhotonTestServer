using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ExitGames.Client.Photon;
using IngameDebugConsole;
using Photon.Pun;
using TMPro;
using UnityEditor;
using UnityEngine;

public class CustomCommands : MonoBehaviour
{
    [SerializeField] private ServerSettings _serverSettings;

    private static CustomCommands Instance;

    private void Awake()
    {
        Instance = this;
    }

    [ConsoleMethod("server.ip", "Set the server ip address"), UnityEngine.Scripting.Preserve]
    public static void SetServerIpAddress(string value)
    {
        Instance._serverSettings.AppSettings.Server = value;
    }
    [ConsoleMethod("server.ip", "Get the server ip address"), UnityEngine.Scripting.Preserve]
    public static string GetServerIpAddress()
    {
        return Instance._serverSettings.AppSettings.Server;
    }
    
    
    [ConsoleMethod("server.port", "Set the server port address"), UnityEngine.Scripting.Preserve]
    public static void SetServerPortAddress(int value)
    {
        Instance._serverSettings.AppSettings.Port = value;
    }
    [ConsoleMethod("server.port", "Get the server port address"), UnityEngine.Scripting.Preserve]
    public static string GetServerPortAddress()
    {
        return Instance._serverSettings.AppSettings.Port.ToString();
    }
    
    
    [ConsoleMethod("server.address", "Set the server IP and Port address"), UnityEngine.Scripting.Preserve]
    public static void SetServerAddress(string ip, int port)
    {
        Instance._serverSettings.AppSettings.Server = ip;
        Instance._serverSettings.AppSettings.Port = port;
    }
    [ConsoleMethod("server.address", "Get the server IP and Port address"), UnityEngine.Scripting.Preserve]
    public static string GetServerAddress()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(Instance._serverSettings.AppSettings.Server + ":");
        stringBuilder.Append(Instance._serverSettings.AppSettings.Port);
        return stringBuilder.ToString();
    }
    

    [ConsoleMethod("server.netLog", "Network logging. 0- OFF, 1- ERROR, 2- WARNING, 3- INFO, 5- ALL"), UnityEngine.Scripting.Preserve]
    public static void SetServerNetworkLogging(int type)
    {
        Instance._serverSettings.AppSettings.NetworkLogging = (DebugLevel)type;
    }
    [ConsoleMethod("server.punLog", "PUN logging. 0- ErrorsOnly, 1- Information, 2- Full"), UnityEngine.Scripting.Preserve]
    public static void SetServerPunLogging(int type)
    {
        Instance._serverSettings.PunLogging = (PunLogLevel)type;
    }
    
    
}
