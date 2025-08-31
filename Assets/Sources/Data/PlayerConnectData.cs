using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConnectData 
{
    public PlayerConnectData(string nickname, ConnData connData)
    {
        this.nickname = ValidationNickname(nickname);
        this.connData = connData;
    }

    public string nickname { get; private set; }
    public ConnData connData {get; private set; }


    public static string ValidationNickname(string nickname)
    {
        if (nickname.Equals(""))
        {
            return GenerateNickname();
        }
        else
        {
            return nickname;
        }
    }

    private static string GenerateNickname()
    {
        string name = "Player";
        int index = Random.Range(0, 100);
        return $"{name}_{index}";
    }


}
