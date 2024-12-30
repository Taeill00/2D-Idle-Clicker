using BreakInfinity;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textCurGems;
    [SerializeField] private TextMeshProUGUI textClickPower;

    public UpgradesManager upgradesManager;
    public Data data;

    public BigDouble ClickPower() => 1 + data.clickUpgradeLevel; 

    private void Start()
    {
        data = new Data();
        upgradesManager.StartUpgradeManager();
    }

    private void Update()
    {
        textCurGems.text = $"{data.curGems.ToString("F2")} Gems";
        textClickPower.text = $"+{ClickPower()} Gems";
    }

    public void OnBtnGenerateGems()
    {
        data.curGems += ClickPower();
    }
}
