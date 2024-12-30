using BreakInfinity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class UpgradesManager : MonoBehaviour
{
    [Header("Scripts Reference")]
    public GameController gameController;
    public Upgrades clickUpgrade;

    public Data data;
 
    [HideInInspector]public string clickUpgradeName;
    [HideInInspector]public BigDouble clickUpgradeBaseCost;
    [HideInInspector]public BigDouble clickUpgradeCostMult;

    public void StartUpgradeManager()
    {
        data = gameController.data;

        clickUpgradeName = "Gems\n per Click";
        clickUpgradeBaseCost = 10;
        clickUpgradeCostMult = 1.5;

        UpdateClickUpgradeUI();
    }

    public void UpdateClickUpgradeUI()
    {
        clickUpgrade.textLevel.text = gameController.data.clickUpgradeLevel.ToString();
        clickUpgrade.textCost.text = $"Cost: {Cost().ToString("F2")} Gems";
        clickUpgrade.textName.text = "+1 " + clickUpgradeName;
    }

    public BigDouble Cost()
    {
        return clickUpgradeBaseCost * BigDouble.Pow(clickUpgradeCostMult, data.clickUpgradeLevel);
    }

    public void BuyUpgrade()
    {
        if(gameController.data.curGems >= Cost())
        {
            gameController.data.curGems -= Cost();
            gameController.data.clickUpgradeLevel += 1;
        }

        UpdateClickUpgradeUI();
    }
}
