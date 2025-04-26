using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopMenu : MenuBase
{
    public Transform cardsParent;
    public Transform itemsParent;

    public void ShowCardSelect()
    {
        Show();
    }

    public void ShowItemPurchase()
    {
        Show();
        {
            var allCandidates = CSVLoader.Instance.cardDict.Values.Where(x => x.canDraw).ToList();
            foreach (var cell in cardsParent.GetComponentsInChildren<ShopCell>(true))
            {
                cell.Init(allCandidates.PickItem());
                cell.cardVisualize.SetInShop();
            }
        }


        {
            var allCandidates = CSVLoader.Instance.itemDict.Values.Where(x => x.canDraw && !ItemManager.Instance.items.Contains(x)).ToList();

            foreach (var cell in itemsParent.GetComponentsInChildren<ShopCell>(true))
            {
                cell.InitItem(allCandidates.PickItem());
               // cell.cardVisualize.isInShop = true;
                FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_ui_click");
            }
        }
    }

    public void UpdateMenu()
    {
        foreach (var cell in itemsParent.GetComponentsInChildren<ShopCell>(true))
        {
            cell.UpdateCell();
        }
        foreach (var cell in cardsParent.GetComponentsInChildren<ShopCell>(true))
        {
            cell.UpdateCell();
        }
    }
}