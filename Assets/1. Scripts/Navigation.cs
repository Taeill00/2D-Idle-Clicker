using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Navigation : MonoBehaviour
{
    public Image imgClickUpgrades;
    public Image imgProductionUpgrades;
    public Image imgGeneratorUpgrades;

    public TextMeshProUGUI textClickUpgradeTitle;
    public TextMeshProUGUI textProductionUpgradeSelected;
    public TextMeshProUGUI textGeneratorUpgradeSelected;

    public GameObject uiInGame;
    public GameObject uiSetting;


    private void Start()
    {
        UpgradesManager.Instance.upgradeHandlers[0].upgradeScroll.gameObject.SetActive(true);
        imgClickUpgrades.color = Color.white;
        textClickUpgradeTitle.color = Color.white;

        UpgradesManager.Instance.upgradeHandlers[1].upgradeScroll.gameObject.SetActive(false);
        UpgradesManager.Instance.upgradeHandlers[2].upgradeScroll.gameObject.SetActive(false);
    }

    public void OnBtnSwitchUpgrades(string location)
    {
        UpgradesManager.Instance.upgradeHandlers[0].upgradeScroll.gameObject.SetActive(false);
        UpgradesManager.Instance.upgradeHandlers[1].upgradeScroll.gameObject.SetActive(false);
        UpgradesManager.Instance.upgradeHandlers[2].upgradeScroll.gameObject.SetActive(false);

        imgClickUpgrades.color = Color.gray;
        textClickUpgradeTitle.color = Color.gray;

        imgProductionUpgrades.color = Color.gray;
        textProductionUpgradeSelected.color = Color.gray;

        imgGeneratorUpgrades.color = Color.gray;
        textGeneratorUpgradeSelected.color = Color.gray;

        switch (location)
        {
            case "Click":
                UpgradesManager.Instance.upgradeHandlers[0].upgradeScroll.gameObject.SetActive(true);

                imgClickUpgrades.color = Color.white; 
                textClickUpgradeTitle.color = Color.white;
                break;

            case "Production":
                UpgradesManager.Instance.upgradeHandlers[1].upgradeScroll.gameObject.SetActive(true);

                imgProductionUpgrades.color = Color.white;
                textProductionUpgradeSelected.color = Color.white;
                break;
            case "Generator":
                UpgradesManager.Instance.upgradeHandlers[2].upgradeScroll.gameObject.SetActive(true);

                imgGeneratorUpgrades.color = Color.white;
                textGeneratorUpgradeSelected.color = Color.white;
                break;
        }
    }

    public void Navigate(string location)
    {
        uiInGame.SetActive(false);
        uiSetting.SetActive(false);

        switch (location)
        {
            case "InGame":
                uiInGame.SetActive(true);
                break;
            case "Setting":
                uiSetting.SetActive(true);
                break;
        }
    }
}
