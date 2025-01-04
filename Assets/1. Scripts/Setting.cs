using TMPro;
using UnityEngine;

public class Setting : MonoBehaviour
{
    public static Setting Instance;
    private void Awake() => Instance = this;

    public string[] notationNames;

    public TextMeshProUGUI[] settingText;

    public GameObject[] settingPanels;

    public void StartSettings()
    {
        notationNames = new string[] { "Standard", "Scientific" };
        Methods.notation = GameController.instance.data.notation;
        SyncSetting();
    }

    public void ChangeSetting(string settingName)
    {
        var data = GameController.instance.data;

        switch(settingName)
        {
            case "Notation":
                data.notation++;
                if (data.notation > notationNames.Length - 1)
                    data.notation = 0;

                Methods.notation = data.notation;
                break;
        }

        SyncSetting(settingName);
    }

    public void SyncSetting(string settingName = "")
    {
        if(settingName == string.Empty)
        {
            settingText[0].text= $"Notation:\n{notationNames[Methods.notation]}";
            return;
        }

        switch (settingName)
        {
            case "Notation":
                settingText[0].text = "Notation:\n" + notationNames[Methods.notation];
                break;
        }
    }

    public void NavigateSettings(string location)
    {
        foreach(var panel in settingPanels)
        {
            panel.SetActive(false);
        }

        switch (location)
        {
            case "Save":
                settingPanels[0].SetActive(true);
                break;
            case "Main":
                settingPanels[1].SetActive(true);
                break;
        }
    }
}
