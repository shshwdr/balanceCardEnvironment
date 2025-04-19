using System.Collections;
using System.Collections.Generic;
using Sinbad;
using UnityEngine;

public class CardInfo
{
    public string identifier;
    public List<string> actions;
    public List<string> types;
    public int selectType;
    public int start;
    public string title;
    public string desc;
    public int energy;
    public int cost;
    public bool canDraw;
    public bool exhaust;
    public int unlockAt;
}

public class TurnRequirementInfo
{
    public int turn;
    public List<int> industryReq;
    public List<int> industryReward;
    public List<int> natureReq;
    public List<int> natureDisaster;

}
public class CSVLoader : Singleton<CSVLoader>
{
    public Dictionary<string, CardInfo> cardDict = new Dictionary<string, CardInfo>();
    public Dictionary<int, TurnRequirementInfo> turnRequirementDict = new Dictionary<int, TurnRequirementInfo>();
    // Start is called before the first frame update
    public void Init()
    {
        var heroInfos =
            CsvUtil.LoadObjects<CardInfo>(GetFileNameWithABTest("card"));
        foreach (var info in heroInfos)
        {
            cardDict[info.identifier] = info;
        }
        var turnRequirements =
            CsvUtil.LoadObjects<TurnRequirementInfo>(GetFileNameWithABTest("turnRequirement"));
        foreach (var info in turnRequirements)
        {
            turnRequirementDict[info.turn] = info;
        }
    }
    
    string GetFileNameWithABTest(string name)
    {
        // if (ABTestManager.Instance.testVersion != 0)
        // {
        //     var newName = $"{name}_{ABTestManager.Instance.testVersion}";
        //     //check if file in resource exist
        //      
        //     var file = Resources.Load<TextAsset>("csv/" + newName);
        //     if (file)
        //     {
        //         return newName;
        //     }
        // }
        return name;
    }
}


