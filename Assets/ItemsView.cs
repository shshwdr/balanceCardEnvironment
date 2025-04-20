using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pool;
using TMPro;
using UnityEngine;

public class ItemsView : MonoBehaviour
{
    public GameObject prefab;

    public Transform parent;

    public void UpdateView()
    {
        var data = ItemManager.Instance.items;
        if (data.Count > parent.childCount)
        {
            for (int i = parent.childCount; i < data.Count; i++)
            {
                GameObject go = Instantiate(prefab, parent);
            }
        }
        for (int i = 0; i < data.Count; i++)
        {
            parent.GetChild(i).GetComponent<TMP_Text>().text = data[i].desc;
        }

        for (int i = data.Count; i < parent.childCount; i++)
        {
            parent.GetChild(i).GetComponent<TMP_Text>().text = "";
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        EventPool.OptIn("ItemChanged", UpdateView);
    }
}
