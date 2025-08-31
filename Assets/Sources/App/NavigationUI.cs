using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationUI : MonoBehaviour
{

    [SerializeField]
    private GameObject menuUIObject;
    [SerializeField]
    private GameObject gameUIObject;

    public void OpenPlayScreen()
    {
        menuUIObject.SetActive(false);
        gameUIObject.SetActive(true);
    }
}
