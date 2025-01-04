using System.Collections.Generic;
using System.Linq;
using BreakInfinity;
using TMPro;
using UnityEngine;
using static Setting;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    [SerializeField] private TextMeshProUGUI textCurGems;
    [SerializeField] private TextMeshProUGUI textGemsPerSecond;
    [SerializeField] private TextMeshProUGUI textClickPower;

    public Data data;
    public Setting setting;

    private const string dataFileName = "PlayerData_Tutorial";
    public float saveTime;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        data = SaveSystem.SaveExists(dataFileName) ? SaveSystem.LoadData<Data>(dataFileName) : new Data();
        UpgradesManager.Instance.StartUpgradeManager();
        setting.StartSettings();
    }

    private void Update()
    {
        textCurGems.text = $"{data.curGems.ToString("F2")} Gems";
        textGemsPerSecond.text = $"{GemsPerSecond():F2}/s";
        textClickPower.text = $"+{ClickPower()} Gems";

        data.curGems += GemsPerSecond() * Time.deltaTime;

        for(int i = 0; i < data.productionUpgradeLevel.Count; i++)
        {
            data.productionUpgradeGenerated[i] += UpgradesPerSecond(i) * Time.deltaTime;
        }

        saveTime += Time.deltaTime * (1 / Time.timeScale);
        if(saveTime >= 15)
        {
            Save();
            saveTime = 0;
        }
    }

    public BigDouble ClickPower()
    {
        BigDouble total = 1;

        for (int i = 0; i < data.clickUpgradeLevel.Count; i++)
        {
            total += UpgradesManager.Instance.upgradeHandlers[0].upgradeBasePower[i] * data.clickUpgradeLevel[i];
        }

        return total;
    }


    public BigDouble GemsPerSecond()
    {
        BigDouble total = 0;

        for (int i = 0; i < data.productionUpgradeLevel.Count; i++)
        {
            total += UpgradesManager.Instance.upgradeHandlers[1].upgradeBasePower[i]
                    * (data.productionUpgradeLevel[i] + data.productionUpgradeGenerated[i]);
        }

        return total;
    }

    public BigDouble UpgradesPerSecond(int index)
    {
        return UpgradesManager.Instance.upgradeHandlers[2].upgradeBasePower[index] * data.generatorUpgradeLevel[index]; 
    }

    public void OnBtnGenerateGems()
    {
        data.curGems += ClickPower();
    }

    public void Save()
    {
        SaveSystem.SaveData(data, dataFileName);
    }
}
 