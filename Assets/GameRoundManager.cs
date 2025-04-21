using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

                StartCoroutine(showReward());
                break;
            case StateType.reward:
                currentState = StateType.pickCard;
                StartCoroutine(showPickCard());
                break;
            case StateType.pickCard:
                currentState = StateType.buyItem;
                StartCoroutine(showPickItem());
                break;
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

        if (industryReward == "DIE")
        {
            GameOver("Better get more industry points next time");
        }else if (natureReward == "DIE")
        {
            GameOver("Better get more nature points next time");
        }
        else
        {
            FindObjectOfType<PopupMenu>().ShowText($"Day Finished!\nYou get {industryReward} in industry\nYou get {natureReward} disaster in nature");
            yield return new WaitUntil(() => FindObjectOfType<PopupMenu>().IsActive == false);
            
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
                    FindObjectOfType<PopupMenu>().ShowText($"You Get Disaster {disaster.desc}");
                    DisasterManager.Instance.AddDisaster(disaster);
                    yield return new WaitUntil(() => FindObjectOfType<PopupMenu>().IsActive == false);
                    
                }
            }
            
            
            Next();
            //yield return new WaitForSeconds(1);
        }
        
    }

    IEnumerator showPickCard()
    {
        FindObjectOfType<ShopMenu>().ShowCardSelect();
        yield return new WaitUntil(() => FindObjectOfType<ShopMenu>().IsActive == false);
        Next();
        
    }
    
    IEnumerator showPickItem()
    {
        FindObjectOfType<ShopMenu>().ShowItemPurchase();
        yield return new WaitUntil(() => FindObjectOfType<ShopMenu>().IsActive == false);
        Next();
        
    }

    void GameOver(string t)
    {
        
        FindObjectOfType<GameOver>().ShowText(t);
    }

    void startBattle()
    {
        GameManager.Instance.StartNewDay();
    }
    
    public void GameWin()
    {

    }
}
