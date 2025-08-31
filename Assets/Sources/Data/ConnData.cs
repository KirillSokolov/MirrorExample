using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnData 
{
    public string ipAddress { get; private set; }
    public int port { get; private set; }

    public ConnData(string ipAddress, int port)
    {
        this.ipAddress = ipAddress;
        this.port = port;

    }
    public ConnData(string ipAddress)
    {
        this.ipAddress = ipAddress;
        this.port = Constants.DEFOLT_PORT;

    }

}
