using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Validation
{
    public static string Nickname(string nickname)
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
