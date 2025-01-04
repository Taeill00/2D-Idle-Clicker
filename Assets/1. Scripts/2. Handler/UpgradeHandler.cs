using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;
using System.Collections.Generic;

public class UpgradeHandler : MonoBehaviour
{
    public List<Upgrades> upgrades;

    public Upgrades upgradePrefab;
    public ScrollRect upgradeScroll;
    public Transform upgradesPanel; // Parent

    public string[] upgradeNames;
    [HideInInspector] public BigDouble[] upgradeBaseCost;
    [HideInInspector] public BigDouble[] upgradeCostMult;
    [HideInInspector] public BigDouble[] upgradeBasePower;
    [HideInInspector] public BigDouble[] upgradesUnlock;
}
