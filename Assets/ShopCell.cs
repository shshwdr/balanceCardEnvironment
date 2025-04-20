using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCell : MonoBehaviour
{
    public CardVisualize cardVisualize;
    public Button buyButton;
    
    public void Init(CardInfo info)
    {
        cardVisualize.Init(info);

        buyButton.GetComponentInChildren<TMP_Text>().text = "Select";
        buyButton.onClick.AddListener(() =>
        {
            HandManager.Instance.AddCard(info);
            FindObjectOfType<ShopMenu>().Hide();
            //GameManager.Instance.Next();
        });
    }

    public void InitItem(ItemInfo info)
    {
        cardVisualize.InitItem(info);
        
        buyButton.GetComponentInChildren<TMP_Text>().text = $"Buy ${info.cost}";
        buyButton.interactable = GameManager.Instance.Gold >= info.cost;
        buyButton.onClick.AddListener(() =>
        {
            ItemManager.Instance.AddItem(info);
            FindObjectOfType<ShopMenu>().Hide();
            GameManager.Instance.Gold -= info.cost;
            //GameManager.Instance.Next();
        });
    }
}
