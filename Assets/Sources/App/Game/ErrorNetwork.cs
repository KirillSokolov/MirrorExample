using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface INetworkConnListener 
{
    void OnSuccess(string message);
    void OnFailure(string errorMessage);
}
