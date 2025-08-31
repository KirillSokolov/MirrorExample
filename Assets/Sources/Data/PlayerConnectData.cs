using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConnectData 
{
    public PlayerConnectData(string nickname, ConnData connData)
    {
        this.nickname = nickname;
        this.connData = connData;
    }

    public string nickname { get; private set; }
    public ConnData connData {get; private set; }

   

}
