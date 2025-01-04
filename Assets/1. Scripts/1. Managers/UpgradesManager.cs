using BreakInfinity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesManager : MonoBehaviour
{
    public static UpgradesManager Instance;

    public UpgradeHandler[] upgradeHandlers;

    private void Awake()
    {
        Instance = this;
    }

    public void StartUpgradeManager()
    {
        Methods.UpgradeCheck(GameController.instance.data.clickUpgradeLevel, 4);
        Methods.UpgradeCheck(GameController.instance.data.productionUpgradeLevel, 4);
        Methods.UpgradeCheck(GameController.instance.data.productionUpgradeGenerated, 4);
        Methods.UpgradeCheck(GameController.instance.data.generatorUpgradeLevel, 4);

        #region ClickUpgrade Init

        CreateUpgrades(GameController.instance.data.clickUpgradeLevel, 0);

        upgradeHandlers[0].upgradeNames = new[] { "Click\n Power +1", "Click\n Power +5", "Click\n Power +10", "Click\n Power +25" };
        upgradeHandlers[0].upgradeBaseCost = new BigDouble[] {10, 50, 100, 1000};
        upgradeHandlers[0].upgradeCostMult = new BigDouble[] {1.25, 1.35, 1.55, 2};
        upgradeHandlers[0].upgradeBasePower = new BigDouble[] {1, 5, 10, 25};
        upgradeHandlers[0].upgradesUnlock = new BigDouble[] {0, 25, 50, 500};

        UpdateUpgradeUI("click");

        #endregion

        #region ProductionUpgrade Init

        CreateUpgrades(GameController.instance.data.productionUpgradeLevel, 1);

        upgradeHandlers[1].upgradeNames = new[] { "+1 Gem/s", "+2 Gems/s", "+10 Gems/s", "+100 Gems/s" };
        upgradeHandlers[1].upgradeBaseCost = new BigDouble[] {25, 100, 1000, 10000 };
        upgradeHandlers[1].upgradeCostMult = new BigDouble[] {1.5, 1.75, 2, 3 };
        upgradeHandlers[1].upgradeBasePower = new BigDouble[] {1, 2, 10, 100 };
        upgradeHandlers[1].upgradesUnlock = new BigDouble[] {10, 50, 500, 5000 };

        UpdateUpgradeUI("production");

        #endregion

        #region GeneratorUpgrade Init

        CreateUpgrades(GameController.instance.data.generatorUpgradeLevel, 2);

        upgradeHandlers[2].upgradeNames = new[] 
        {
            $"Produces +0.1 \n +1 Upgrades/s",
            $"Produces +0.05 \n +2 Upgrades/s",
            $"Produces +0.02 \n +10 Upgrades/s",
            $"Produces +0.01 \n +100 Upgrades/s",
        };
        upgradeHandlers[2].upgradeBaseCost = new BigDouble[] {5000, 1e4, 1e5, 1e6 };
        upgradeHandlers[2].upgradeCostMult = new BigDouble[] {1.5, 1.75, 2, 2.5 };
        upgradeHandlers[2].upgradeBasePower = new BigDouble[] {0.1, 0.05, 0.02, 0.01};
        upgradeHandlers[2].upgradesUnlock = new BigDouble[] {2500, 5e3, 5e4, 5e5 };

        UpdateUpgradeUI("generator");

        #endregion

        void CreateUpgrades<T>(List<T> level, int index)
        {
            //upgradeHandlers[index].upgrades.Clear();

            for(int i = 0; i < level.Count; i++)
            {
                Upgrades upgrade = Instantiate(upgradeHandlers[index].upgradePrefab, upgradeHandlers[index].upgradesPanel);
                upgrade.upgradeID = i;
                upgrade.gameObject.SetActive(false);
                upgradeHandlers[index].upgrades.Add(upgrade);
            }

            upgradeHandlers[index].upgradeScroll.normalizedPosition = new Vector2(0, 0);
        }

    }

    private void Update()
    {
        UpgradeUnlockSystem(GameController.instance.data.curGems, upgradeHandlers[0].upgradesUnlock, 0);
        UpgradeUnlockSystem(GameController.instance.data.curGems, upgradeHandlers[1].upgradesUnlock, 1);
        UpgradeUnlockSystem(GameController.instance.data.curGems, upgradeHandlers[2].upgradesUnlock, 2);

        void UpgradeUnlockSystem(BigDouble currency, BigDouble[] unlock, int index)
        {
            for (int i = 0; i < upgradeHandlers[index].upgrades.Count; i++)
            {
                if (!upgradeHandlers[index].upgrades[i].gameObject.activeSelf)
                    upgradeHandlers[index].upgrades[i].gameObject.SetActive(currency >= unlock[i]);
            }
        }

        if (upgradeHandlers[1].upgradeScroll.gameObject.activeSelf)
        {
            UpdateUpgradeUI("production");
        }
    }

    public void UpdateUpgradeUI(string type, int upgradeID = -1)
    {
        var data = GameController.instance.data;

        switch (type)
        {
            case "click":
                UpdateAllUI(upgradeHandlers[0].upgrades, data.clickUpgradeLevel, upgradeHandlers[0].upgradeNames, 0, upgradeID, type);
                break;

            case "production":
                UpdateAllUI(upgradeHandlers[1].upgrades, data.productionUpgradeLevel, upgradeHandlers[1].upgradeNames, 1, upgradeID, type, 
                            data.productionUpgradeGenerated);
                break;

            case "generator":
                UpdateAllUI(upgradeHandlers[2].upgrades, data.generatorUpgradeLevel, upgradeHandlers[2].upgradeNames, 2, upgradeID, type);
                break;
        }
    }

    private void UpdateAllUI(List<Upgrades> upgrades, List<int> upgradeLevels, string[] upgradeNames, int index, int upgradeID, string type)
    {
        if (upgradeID == -1)
        {
            for (int i = 0; i < upgradeHandlers[index].upgrades.Count; i++)
            {
                UpdateUI(i);
            }
        }
        else
        {
            UpdateUI(upgradeID);
        }

        void UpdateUI(int ID)
        {
            upgrades[ID].textLevel.text = upgradeLevels[ID].ToString("F0");
            upgrades[ID].textCost.text = $"Cost: {UpgradeCost(type, ID):F2}";
            upgrades[ID].textName.text = upgradeNames[ID];
        }
    }

    private void UpdateAllUI(List<Upgrades> upgrades,List<BigDouble> upgradeLevels, 
                             string[] upgradeNames, int index, int upgradeID, string type, List<BigDouble> upgradeGenerated = null)
    {
        if (upgradeID == -1)
        {
            for (int i = 0; i < upgradeHandlers[index].upgrades.Count; i++)
            {
                UpdateUI(i);
            }
        }
        else
        {
            UpdateUI(upgradeID);
        }

        void UpdateUI(int ID)
        {
            BigDouble generated = upgradeGenerated == null ? 0 : upgradeGenerated[ID];

            upgrades[ID].textLevel.text = (upgradeLevels[ID] + generated).ToString("F2");
            upgrades[ID].textCost.text = $"Cost: {UpgradeCost(type, ID):F2}";
            upgrades[ID].textName.text = upgradeNames[ID];
        }
    }

    public BigDouble UpgradeCost(string type, int upgradeID)
    {
        var data = GameController.instance.data;

        switch (type)
        {
            case "click":
                return UpgradeCost_Int(0, data.clickUpgradeLevel, upgradeID);
            case "production":
                return UpgradeCost_BigDouble(1, data.productionUpgradeLevel, upgradeID);
            case "generator":
                return UpgradeCost_Int(2, data.generatorUpgradeLevel, upgradeID);
        }
         
        return 0;
    }

    private BigDouble UpgradeCost_BigDouble(int index, List<BigDouble> levels, int upgradeID)
    {
        return upgradeHandlers[index].upgradeBaseCost[upgradeID] * 
               BigDouble.Pow(upgradeHandlers[index].upgradeCostMult[upgradeID], levels[upgradeID]);
    }

    private BigDouble UpgradeCost_Int(int index, List<int> levels, int upgradeID)
    {
        return upgradeHandlers[index].upgradeBaseCost[upgradeID] *
               BigDouble.Pow(upgradeHandlers[index].upgradeCostMult[upgradeID], (BigDouble)levels[upgradeID]);
    }

    #region Buy
    public void BuyUpgrade(string type, int upgradeID)
    { 
        var data = GameController.instance.data;

        switch (type)
        {
            case "click": 
                Buy(data.clickUpgradeLevel, type, upgradeID);
                break;
            case "production": 
                Buy(data.productionUpgradeLevel, type, upgradeID);
                break;
            case "generator":
                Buy(data.generatorUpgradeLevel, type, upgradeID);
                break;
        }
    }

    private void Buy(List<int> upgradeLevels, string type, int upgradeID)
    {
        var data = GameController.instance.data;

        if (data.curGems >= UpgradeCost(type, upgradeID))
        {
            data.curGems -= UpgradeCost(type, upgradeID);
            upgradeLevels[upgradeID] += 1;

            UpdateUpgradeUI(type, upgradeID);
        }
    }

    private void Buy(List<BigDouble> upgradeLevels, string type, int upgradeID)
    {
        var data = GameController.instance.data;

        if (data.curGems >= UpgradeCost(type, upgradeID))
        {
            data.curGems -= UpgradeCost(type, upgradeID);
            upgradeLevels[upgradeID] += 1;

            UpdateUpgradeUI(type, upgradeID);
        }
    }
    #endregion
}
