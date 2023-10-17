using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpgrade : UpgradeCard
{
    protected override int SkillLevel
    {
        get => PlayerPrefs.GetInt(ConstantVariables.UpgradeTypes.Power + ConstantVariables.LevelStats.SkillLevel, 1);
        set => PlayerPrefs.SetInt(ConstantVariables.UpgradeTypes.Power + ConstantVariables.LevelStats.SkillLevel, PlayerPrefs.GetInt(ConstantVariables.UpgradeTypes.Power
            + ConstantVariables.LevelStats.SkillLevel, 1) + value);
    }

    protected override int CurrentPrice
    {
        get => PlayerPrefs.GetInt(ConstantVariables.UpgradeTypes.Power + ConstantVariables.UpgradePrices.CurrentPrice, startPrice);
        set => PlayerPrefs.SetInt(ConstantVariables.UpgradeTypes.Power + ConstantVariables.UpgradePrices.CurrentPrice, value);
    }

    protected override int IncrementalPrice
    {
        get => PlayerPrefs.GetInt(ConstantVariables.UpgradeTypes.Power + ConstantVariables.UpgradePrices.IncrementalPrice, incrementalBasePrice);
        set => PlayerPrefs.SetInt(ConstantVariables.UpgradeTypes.Power + ConstantVariables.UpgradePrices.IncrementalPrice, PlayerPrefs.GetInt(ConstantVariables.UpgradeTypes.Power
            + ConstantVariables.UpgradePrices.IncrementalPrice, 0) + value);
    }

    protected override float UpgradeCurrentValue
    {
        get => PlayerPrefs.GetFloat(ConstantVariables.UpgradeTypes.Power + ConstantVariables.UpgradeValues.UpgradeCurrentValue, upgradeValue);
        set => PlayerPrefs.SetFloat(ConstantVariables.UpgradeTypes.Power + ConstantVariables.UpgradeValues.UpgradeCurrentValue, value);
    }

    protected override float UpgradeIncrementalValue
    {
        get => PlayerPrefs.GetFloat(ConstantVariables.UpgradeTypes.Power + ConstantVariables.UpgradeValues.UpgradeIncrementalValue, upgradeIncrementalValue);
        set => PlayerPrefs.SetFloat(ConstantVariables.UpgradeTypes.Power + ConstantVariables.UpgradeValues.UpgradeIncrementalValue, value);
    }

    protected override void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteKey(ConstantVariables.UpgradeTypes.Power + ConstantVariables.LevelStats.SkillLevel);
        PlayerPrefs.DeleteKey(ConstantVariables.UpgradeTypes.Power + ConstantVariables.UpgradePrices.CurrentPrice);
        PlayerPrefs.DeleteKey(ConstantVariables.UpgradeTypes.Power + ConstantVariables.UpgradePrices.IncrementalPrice);
        PlayerPrefs.DeleteKey(ConstantVariables.UpgradeTypes.Power + ConstantVariables.UpgradeValues.UpgradeCurrentValue);
        PlayerPrefs.DeleteKey(ConstantVariables.UpgradeTypes.Power + ConstantVariables.UpgradeValues.UpgradeIncrementalValue);
    }
}
