using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomToggle : MonoBehaviour
{
    [SerializeField]
    private string nameToggle;
    [SerializeField]
    private GameObject backgroundToggle;
    [SerializeField]
    private Image selectToggle;
    [SerializeField]
    private TextMeshProUGUI textToggle;

    public void ChangeToggle(Sprite toggleIcon, Color textColor, bool isSelect = false)
    {
        backgroundToggle.SetActive(isSelect);
        textToggle.color = textColor;
        selectToggle.sprite = toggleIcon;
    }

    public string GetNameToggle()
    {
        return nameToggle;
    }
 
}
