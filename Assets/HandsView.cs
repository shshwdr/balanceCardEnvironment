using System.Collections;
using System.Collections.Generic;
using Pool;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HandsView : Singleton<HandsView>
{
    public Transform parent;

    public CardVisualize cardPrefab;
    public Button drawButton;

    public void InitLevel()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        // foreach (var info in HandManager.Instance.hand)
        // {
        //     var go = Instantiate(cardPrefab.gameObject, parent);
        //     go.GetComponent<CardVisualize>().Init(info);
        // }
        EventPool.OptIn("DrawHand", UpdateHands);
        UpdateHands();
        drawButton.onClick.AddListener(() =>
        {
            DrawCard();
          //  FindObjectOfType<TutorialMenu>(). FinishUseRedraw();
        });
        drawButton.gameObject.SetActive(false);
    }

    public void showRedrawButton()
    {
        drawButton.gameObject.SetActive(true);
    }

    public void DrawCard()
    {
        HandManager.Instance.DrawHand();
        UpdateHands();

        //FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_draw_card");
    }
    
    public void UpdateHands()
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
        foreach (var info in HandManager.Instance.handInBattle)
        {
            var go = Instantiate(cardPrefab.gameObject, parent);
            go.GetComponent<CardVisualize>().Init(info);
        }
    }

}
