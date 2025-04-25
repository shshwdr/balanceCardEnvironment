using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopMenu : MenuBase
{
    public Transform itemsParent;
    public void ShowCardSelect()
    {
        Show();
        var allCandidates = CSVLoader.Instance.cardDict.Values.Where(x => x.canDraw).ToList();
        foreach (var cell in itemsParent.GetComponentsInChildren<ShopCell>(true))
        {
         cell.Init(allCandidates.PickItem());
         cell.cardVisualize.isInShop = true;

        }
    }

    public void ShowItemPurchase()
    {
        Show();
        var allCandidates = CSVLoader.Instance.itemDict.Values.Where(x => x.canDraw).ToList();
        foreach (var cell in itemsParent.GetComponentsInChildren<ShopCell>(true))
        {
            cell.InitItem(allCandidates.PickItem());
            cell.cardVisualize.isInShop = true;

            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_ui_click");
        }
    }
}
