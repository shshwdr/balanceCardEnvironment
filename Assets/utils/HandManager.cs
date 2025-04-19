using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pool;
using UnityEngine;

public class HandManager : Singleton<HandManager>
{
    public List<CardInfo> ownedCards = new List<CardInfo>();
    
    public List<CardInfo> deck = new List<CardInfo>();
    public List<CardInfo> handInBattle = new List<CardInfo>();
    public List<CardInfo> discardedInBattle = new List<CardInfo>();
    
    private int handMax = 4;
    public void InitDeck()
    {
        deck = ownedCards.ToList();
    }

    public void useCard(CardInfo info)
    {
        handInBattle.Remove(info);
        if (info.exhaust)
        {
            
        }
        else
        {
            discardedInBattle.Add(info);
        }
        EventPool.Trigger("DrawHand");
        
        DoCardAction(info);
    }

    void DoCardAction(CardInfo info)
    {
        int test = 0;
        for (int i = 0; i < info.actions.Count;i++)
        {
            test++;
            if (test > 100)
            {
                Debug.LogError("DoCardAction infinite loop");
                break;
            }
            string action = info.actions[i];
            switch (info.actions[i])
            {
                case "industry":
                {
                    
                    i++;
                    int value = int.Parse(info.actions[i]);
                     
                         GameManager.Instance.Industry += value;
                    break;
                }
                case "nature":
                {
                    i++;
                    int value = int.Parse(info.actions[i]);
                    
                    GameManager.Instance.Nature += value;
                    break;
                }
                case "industryMan":
                    case "natureMan":
                {
                    i++;
                    int value = int.Parse(info.actions[i]);
                      GameManager.Instance.AddCharacter(action, value);
                    SceneRenderer.Instance.characterSpawner.SpawnPrefab(action, value);
                    break;
                }
                case "draw":
                {
                    i++;
                    int value = int.Parse(info.actions[i]);
                    HandsView.Instance.DrawCards(value);
                    break;
                }
                case "discard":
                {
                    i++;
                    int value = int.Parse(info.actions[i]);
                    HandsView.Instance.DiscardCards(value);
                    break;
                }
                case "boostIndustry":
                case "boostNature":
                {
                    i++;
                    int value = int.Parse(info.actions[i]);
                    GameManager.Instance.AddState(action, value);
                    break;
                }
            }
        }

        if (info.types.Contains("industry"))
        {
            GameManager.Instance.Industry += GameManager.Instance.industryManCount * (1+ GameManager.Instance.industryBoost);
        }
        if (info.types.Contains("nature"))
        {
            GameManager.Instance.Nature += GameManager.Instance.natureManCount * (1+ GameManager.Instance.natureBoost);
        }
    }
    
    

    public void DrawSpecificCard(string key,bool fromdeck = true)
    {
        var infect = CSVLoader.Instance.cardDict[key];
        if (fromdeck)
        {
            if(!deck.Contains(infect))
            {
                deck .AddRange( discardedInBattle);
            }
            
            if (deck.Contains(infect))
            {
                deck.Remove(infect);
                handInBattle.Add(infect);
            }
        }
        else
        {
            handInBattle.Add(infect);
        }
        
        EventPool.Trigger("DrawHand");
    }

    public void DiscardCards(int count)
    {
        for (int i = 0; i < count; i++)
        {

            if (handInBattle.Count == 0)
            {
                break;
            }
            var info = handInBattle.PickItem();
            handInBattle.Remove(info);
            if (info.exhaust)
            {
            
            }
            else
            {
                discardedInBattle.Add(info);
            }
        }
      
        EventPool.Trigger("DrawHand");  
    }
    public void DrawCard(int count)
    {
        //discardedInBattle.AddRange(handInBattle);
        //handInBattle.Clear();
        for (int i = 0; i < count; i++)
        {
            if (deck.Count == 0)
            {
                deck = discardedInBattle;
            }

            if (deck.Count == 0)
            {
                break;
            }

           
            {
                
                handInBattle.Add(deck.PickItem());
            }
        }
        EventPool.Trigger("DrawHand");
    }
    
    public void DrawHand()
    {
        discardedInBattle.AddRange(handInBattle);
        handInBattle.Clear();
        for (int i = 0; i < handMax; i++)
        {
            if (deck.Count == 0)
            {
                deck = discardedInBattle;
            }

            if (deck.Count == 0)
            {
                break;
            }

           
            {
                
                handInBattle.Add(deck.PickItem());
            }
        }
        EventPool.Trigger("DrawHand");
    }

    public void ClearBattleHand()
    {
        handInBattle.Clear();
        discardedInBattle.Clear();
        EventPool.Trigger("DrawHand");
    }
    public void AddCard(CardInfo info)
    {
        ownedCards.Add(info);
    }
    public void Init()
    {
        ownedCards.Clear();
        handInBattle.Clear();
        discardedInBattle.Clear();
        foreach (var info in CSVLoader.Instance.cardDict.Values)
        {
            for (int i = 0; i < info.start; i++)
            {
                ownedCards.Add(info);
            }
            
        }
    }
}
