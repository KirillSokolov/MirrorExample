using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ISaveLoadUserData
{
    public void SaveNickname(string nickname);
    public string GetNickname();

}
public interface ISaveLoadConnServerData
{
    public void SaveConnServerData(ConnData connData);
    public ConnData GetConnDataServer();

}

public interface ISaveLoadConnClientData
{
    public void SaveConnClientData(ConnData connData);
    public ConnData GetConnClientData();

}

public interface ISaveLoadConnTypeData
{
    public void SaveConnType(int connSelectPos);
    public int GetConnType();

}

public interface ISaveLoadColorTypeData
{
    public void SaveColorType(int connSelectPos);
    public int GetColorType();

}
