using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthController : SingletonMonoBehaviour<GrowthController>
{
    public List<TextAsset> growthPhaseZero = new List<TextAsset>();
    public List<TextAsset> growthPhaseOne = new List<TextAsset>();
    public List<TextAsset> growthPhaseTwo = new List<TextAsset>();
    public List<TextAsset> growthPhaseThree = new List<TextAsset>();

    public string GetRandomMap(int growthPhase)
    {
        List<TextAsset> assetList = null;
        switch (growthPhase)
        {
            default:
            case 0:
                assetList = growthPhaseZero;
                break;
            case 1:
                assetList = growthPhaseOne;
                break;
            case 2:
                assetList = growthPhaseTwo;
                break;
            case 3:
                assetList = growthPhaseThree;
                break;
        }

        if(assetList != null)
        {
            TextAsset asset = assetList[Random.Range(0, assetList.Count - 1)];
            if(asset != null)
            {
                return asset.text;
            }
        }
        return "";
    }

    public int GetDoorCount(int growthPhase)
    {
        return 5;
    }

    public List<int> GetMonsters(int growthPhase)
    {
        return new List<int>() { 1, 2, 3 };
    }
}
