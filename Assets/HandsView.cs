using System.Collections;
using System.Collections.Generic;
using Pool;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HandsView : Singleton<HandsView>
{
    public Transform parent;

    public CardVisualize cardPrefab;
    [FormerlySerializedAs("drawButton")] public Button endDayButton;

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
        endDayButton.onClick.AddListener(() =>
        {
            DrawCard();
          //  FindObjectOfType<TutorialMenu>(). FinishUseRedraw();
        });
        //endDayButton.gameObject.SetActive(false);
    }

    // public void showRedrawButton()
    // {
    //     endDayButton.gameObject.SetActive(true);
    // }

    public void DrawCard()
    {
        HandManager.Instance.DrawHand();
        UpdateHands();

        //FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_draw_card");
    }

    public void DrawCards(int count)
    {
        HandManager.Instance.DrawCard(count);
        UpdateHands();
    }

    public void DiscardCards(int count)
    {
        HandManager.Instance.DiscardCards(count);
        UpdateHands();
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
