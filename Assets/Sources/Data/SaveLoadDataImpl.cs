using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SaveLoadDataImpl: 
    ISaveLoadUserData, 
    ISaveLoadConnServerData, 
    ISaveLoadConnClientData,
    ISaveLoadConnTypeData,
    ISaveLoadColorTypeData
{


    private static SaveLoadDataImpl _instance;
    private SaveLoadDataImpl() { }

    public static SaveLoadDataImpl Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SaveLoadDataImpl();
            }
            return _instance;
        }
    }


    public void SaveNickname(string nickname)
    {
        PlayerPrefs.SetString(Constants.PLAYER_NAME_KEY, nickname);
        PlayerPrefs.Save();
    }
    public void SaveConnServerData(ConnData connData)
    {
        PlayerPrefs.SetString(Constants.IP_ADDRESS_SERVER_KEY, connData.ipAddress);
        PlayerPrefs.SetInt(Constants.PORT_SERVER_KEY, connData.port);
        PlayerPrefs.Save();
    }
    public void SaveConnClientData(ConnData connData)
    {
        PlayerPrefs.SetString(Constants.IP_ADDRESS_CLIENT_KEY, connData.ipAddress);
        PlayerPrefs.SetInt(Constants.PORT_CLIENT_KEY, connData.port);
        PlayerPrefs.Save();
    }

    public void SaveConnType(int connSelectPos)
    {
        PlayerPrefs.SetInt(Constants.CONN_TYPE_KEY, connSelectPos);
        PlayerPrefs.Save();
    }

    public void SaveColorType(int colorSelectPos)
    {
        PlayerPrefs.SetInt(Constants.PLAYER_COLOR_KEY, colorSelectPos);
        PlayerPrefs.Save();
    }

 
    public string GetNickname()
    {
        return PlayerPrefs.GetString(Constants.PLAYER_NAME_KEY, "");
    }


    public ConnData GetConnClientData()
    {
        return new ConnData(PlayerPrefs.GetString(Constants.IP_ADDRESS_CLIENT_KEY, GetIpAddress.GetLocalIPAddress()));
    }

    public ConnData GetConnDataServer()
    {
        return new ConnData(GetIpAddress.GetLocalIPAddress());
    }

    public int GetConnType()
    {
        return PlayerPrefs.GetInt(Constants.CONN_TYPE_KEY, 0);

    }

    public int GetColorType()
    {
        return PlayerPrefs.GetInt(Constants.PLAYER_COLOR_KEY, 0);
    }
}
