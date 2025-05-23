using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pool;
using UnityEngine;

public class GameRoundManager : Singleton<GameRoundManager>
{
    public void Init()
    {
        currentState = StateType.start;
        Next();
    }
    
    enum StateType
    {
        start,
        battle,
        reward,
        pickCard,
        buyItem,
    };

    public bool isInBattle
    {
         get
         {
             return currentState == StateType.battle;
         }
    }
    private StateType currentState = StateType.start;
    public void Next()
    {
        switch (currentState)
        {
            case StateType.start:
                currentState = StateType.battle;
                startBattle();
                break;
            case StateType.battle:
                currentState = StateType.reward;

                //if (CSVLoader.Instance.turnRequirementDict.ContainsKey(GameManager.Instance.Day))
                {
                    
                    StartCoroutine(showReward());
                }
                break;
            case StateType.reward:
                currentState = StateType.buyItem;
                StartCoroutine(showPickItem());
                break;
            // case StateType.pickCard:
            //     currentState = StateType.buyItem;
            //     StartCoroutine(showPickItem());
            //     break;
            case StateType.buyItem:
                currentState = StateType.battle;
                startBattle();
                break;
            default:
                break;
        }
    }

    IEnumerator showReward()
    {
        var industryReward = Hud.Instance.industryMeter.GetComponentInParent<MeterView>().currentResult;
        var natureReward = Hud.Instance.natureMeter.GetComponentInParent<MeterView>().currentResult;
        
        var currentTurnReq = CSVLoader.Instance.turnRequirementDict[GameManager.Instance.Day-1];
        var industryValue = $"{GameManager.Instance.Industry}/{currentTurnReq.industryReq.LastItem()}";
        var natureValue = $"{GameManager.Instance.Nature}/{currentTurnReq.natureReq.LastItem()}";
        

        if (industryReward == "DIE")
        {
            GameOver("Better get more industry points next time");
        }else if (natureReward == "DIE")
        {
            GameOver("Better get more nature points next time");
        }
        else
        {
            
            if(!CSVLoader.Instance.turnRequirementDict.ContainsKey(GameManager.Instance.Day))
            {
                GameWin();
                yield break;
            }
            FindObjectOfType<PopupMenuResult>().ShowText($"{industryValue}",$"{natureValue}",$"Earned: {industryReward}",$"Next Disaster:{natureReward}");
            yield return new WaitUntil(() => FindObjectOfType<PopupMenuResult>().IsActive == false);
            
            var goldCount  = int.Parse(industryReward);
            if (goldCount > 0)
            {
                GameManager.Instance.Gold += goldCount;
            }
            
            var disasterCount = int.Parse(natureReward);
            
            DisasterManager.Instance.ClearDisaster();
            if (disasterCount > 0)
            {
                var disasters = CSVLoader.Instance.disasterDict.Values.Where(x => x.canDraw).ToList();
                for(int i = 0;i<disasterCount;i++)
                {
                    var disaster = disasters.PickItem();
                    FindObjectOfType<PopupMenuDisaster>().ShowText($"{disaster.desc}");
                    FindObjectOfType<PopupMenuDisaster>().ShowTitle($"{disaster.title}");
                    DisasterManager.Instance.AddDisaster(disaster);
                    yield return new WaitUntil(() => FindObjectOfType<PopupMenuDisaster>().IsActive == false);
                    
                }
            }
            
            
            Next();
            //yield return new WaitForSeconds(1);
        }
        
    }

    // IEnumerator showPickCard()
    // {
    //     FindObjectOfType<ShopMenu>().ShowCardSelect();
    //     yield return new WaitUntil(() => FindObjectOfType<ShopMenu>().IsActive == false);
    //     Next();
    //     
    // }
    
    IEnumerator showPickItem()
    {
        FindObjectOfType<ShopMenu>().ShowItemPurchase();
        yield return new WaitUntil(() => FindObjectOfType<ShopMenu>().IsActive == false);
        Next();
        
    }

    void GameOver(string t)
    {
        
        FindObjectOfType<GameOver>().ShowText(t,false);
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_game_over");
    }

    void startBattle()
    {
        
        EventPool.Trigger("meterUpdate",-1,true);
        
        EventPool.Trigger("meterUpdate",-1,false);
        GameManager.Instance.StartNewDay();
        
    }
    
    public void GameWin()
    {
        FindObjectOfType<GameOver>().ShowText("You saved the world!",true);
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_level_win");
    }
}
