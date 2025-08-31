using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToggleGroupType
{
    Connection, Color
}

public class ToggleGroupSelect : MonoBehaviour
{
    private List<IGetEventToggle> eventToggles = new List<IGetEventToggle>();

    [SerializeField]
    private ToggleGroupType toggleGroupType = ToggleGroupType.Connection;


    [SerializeField]
    private Sprite selectSprite, unSelectSprite;

    [SerializeField]
    private Color selectColor = Color.black, unSelectColor = Color.white;


    [SerializeField]
    private List<CustomToggle> toggles;
    void Start()
    {

        switch (toggleGroupType)
        {
            case ToggleGroupType.Connection:
                SelectSavedOnStart(Constants.CONN_TYPE_KEY);
                break;
            case ToggleGroupType.Color:
                SelectSavedOnStart(Constants.PLAYER_COLOR_KEY);
                break;
            default:
                SelectFirstOnStart();
                break;
        }
    }

    private void SelectSavedOnStart(string saveKeyValue)
    {
        int selectNum = PlayerPrefs.GetInt(saveKeyValue, 0);

        eventToggles.AddRange(InterfaceFinderInactive.FindObjectsOfInterfaceIncludingInactive<IGetEventToggle>());
        toggles[selectNum].ChangeToggle(selectSprite, selectColor, true);
        SendEventSelect(toggles[selectNum]);
    }
    private void SelectFirstOnStart()
    {
        eventToggles.AddRange(InterfaceFinderInactive.FindObjectsOfInterfaceIncludingInactive<IGetEventToggle>());
        toggles[0].ChangeToggle(selectSprite, selectColor, true);
        SendEventSelect(toggles[0]);
    }

    // Update is called once per frame
    public void OnClickToggle(CustomToggle selectToggle)
    {
        SendEventSelect(selectToggle);
        toggles.ForEach(toggle =>
        {
            if (toggle.Equals(selectToggle))
            {
                toggle.ChangeToggle(selectSprite, selectColor, true);
            }
            else
            {
                toggle.ChangeToggle(unSelectSprite, unSelectColor, false);
            }
        });
    }

    private void SendEventSelect(CustomToggle selectToggle)
    {
        if (eventToggles != null)
        {
            eventToggles.ForEach(eventToggle =>
            {
                eventToggle.OnToggleSelected(selectToggle.GetNameToggle());
            });
                
        }
    }
}
