using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrades : MonoBehaviour
{
    public int upgradeID;
    public Image btnUpgrade;
    public TextMeshProUGUI textLevel;
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textCost;

    public Image fill;

    public void OnBtnBuyClickUpgrade()
    {
        UpgradesManager.Instance.BuyUpgrade("click", upgradeID);
    }

    public void OnBtnBuyProductionUpgrade()
    {
        UpgradesManager.Instance.BuyUpgrade("production", upgradeID);
    }

    public void OnBtnBuyGeneratorUpgrade()
    {
        UpgradesManager.Instance.BuyUpgrade("generator", upgradeID);
    }
}
