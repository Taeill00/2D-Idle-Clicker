using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BreakInfinity;
using Unity.VisualScripting;

[Serializable]  
public class Data 
{
    public BigDouble curGems;

    public List<int> clickUpgradeLevel;
    public List<BigDouble> productionUpgradeLevel;
    public List<BigDouble> productionUpgradeGenerated;
    public List<int> generatorUpgradeLevel;

    public int notation;

    public Data() 
    {
        curGems = 0;

        clickUpgradeLevel = new int[4].ToList();
        productionUpgradeLevel = new BigDouble[4].ToList();
        productionUpgradeGenerated = new BigDouble[4].ToList();
        generatorUpgradeLevel = new int[4].ToList();

        notation = 0;
    }
}
