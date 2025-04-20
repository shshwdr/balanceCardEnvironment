using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pool;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private void Awake()
    {
        CSVLoader.Instance.Init();
        HandManager.Instance.Init();
        HandManager.Instance.InitDeck();

    }

    public Dictionary<string, int> states = new Dictionary<string, int>();

    private int startEnergy = 4;
    private int energy;
    public int Energy {
        get => energy;
        set
        {
            energy = value;
            EventPool.Trigger("EnergyChanged");
        }
    }

    public bool hasEnoughEnergy(int e)
    {
        return energy >= e;
    }

    public void ConsumeEnergy(int e)
    {
        Energy -= e;
    }

    public void ResetEnergy()
    {
        Energy = startEnergy;
    }

    public void DoubleBoost()
    {
        foreach (var key in states.Keys.ToList())
        {
             states[key] *= 2;
        }
        EventPool.Trigger("StateChanged");
    }
    public void AddState(string key, int value)
    {
        if (states.ContainsKey(key))
        {
            states[key] += value;
        }
        else
        {
            states[key] = value;
        }
        EventPool.Trigger("StateChanged");
    }

    public int GetState(string key)
    {
        if (states.ContainsKey(key))
        {
            return states[key];
        }
        return 0;
    }
    public int Turn
    {
        get => turn;
        set
        {
            turn = value;
            if (turnInDay == turn)
            {
                Day++;
                turn = 1;
                GameRoundManager.Instance.Next();
            }

            ResetEnergy();
            EventPool.Trigger("TurnChanged");
        }
    }

    public int turnInDay = 3;
    private int turn = 1;

    private int day = 1;
    public int Day
    {
        get => day;
        set
        {
            day = value;
            EventPool.Trigger("DayChanged");
        }
    }

    private int industry;
    private int nature;
    private int gold;

    public int Industry
    {
        get => industry;
        set
        {
            if(value!=industry)
            DamageNumbersManager.Instance.ShowResourceCollection(Hud.Instance.industryMeter, value-industry, DamageNumberType.industry);
            var diff = value - industry;
            if (diff < 0)
            {
                if (DisasterManager.Instance.buffManager.hasBuff("doubleLose"))
                {
                    diff *= 2;
                }
                if (ItemManager.Instance.buffManager.GetBuffValue("industryLoseAddIndustryMan") > 0)
                {
                    GameManager.Instance.AddCharacter("industryMan",  ItemManager.Instance.buffManager.GetBuffValue("industryLoseAddIndustryMan"));
                }
            }else if (diff > 0)
            {
                if (DisasterManager.Instance.buffManager.hasBuff("IndustryLoseNature"))
                {
                    Nature -= 20;
                }
            }
            industry += diff;
            EventPool.Trigger("IndustryChanged");
        }
    }
    public int Nature
    {
        get => nature;
        set
        {
            if(value!=nature)
            DamageNumbersManager.Instance.ShowResourceCollection( Hud.Instance.natureMeter, value - nature, DamageNumberType.nature);
            var diff = value - nature;
            if (diff < 0)
            {
                if (DisasterManager.Instance.buffManager.hasBuff("doubleLose"))
                {
                    diff *= 2;
                }
                if (ItemManager.Instance.buffManager.GetBuffValue("natureLoseAddNatureMan") > 0)
                {
                    GameManager.Instance.AddCharacter("natureMan",  ItemManager.Instance.buffManager.GetBuffValue("natureLoseAddNatureMan"));
                }
            }else if (diff > 0)
            {
                if (DisasterManager.Instance.buffManager.hasBuff("natureLoseIndustry"))
                {
                    Industry -= 20;
                }
            }
            nature += diff;
            EventPool.Trigger("NatureChanged");
        }
    }
    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
            EventPool.Trigger("GoldChanged");
        }
    }

    private Dictionary<string, int> charactersDict = new Dictionary<string, int>();

    public void AddCharacter(string key, int value)
    {
        if (charactersDict.ContainsKey(key))
        {
            charactersDict[key] += value;
        }
        else
        {
            charactersDict[key] = value;
        }
        EventPool.Trigger("CharacterChanged");
    }

    public int industryManCount => GetCharacter("industryMan");
    public int industryBoost => GetState("boostIndustry");
    public int natureManCount => GetCharacter("natureMan");
    public int natureBoost => GetState("boostNature");
    public int GetCharacter(string key)
    {
        if (charactersDict.ContainsKey(key))
        {
            return charactersDict[key];
        }
        return 0;
    }

    public void StartNewDay()
    {
        Industry = 0;
        Nature = 0;
        
        clearBoost();
    }
    public void clearBoost()
    {
        states.Clear();
        EventPool.Trigger("StateChanged");
    }
    public void InitNewTurn()
    {
        
        Turn = 1;
        Day = 1;
        HandsView.Instance.DrawCard();
        foreach (var meterView in FindObjectsOfType<MeterView>())
        {
            meterView.UpdateViewForStartOfTurn();
        }

        GameRoundManager.Instance.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitNewTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                
                Industry -= 10;
            }
            else
            {
                Industry += 10;
                
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                Nature -= 10;
            }
            else
            {
                Nature += 10;
                
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Gold += 10;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            
            ItemManager.Instance.AddAll();
            DisasterManager.Instance.AddAll();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
