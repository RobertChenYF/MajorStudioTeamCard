using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardObjectScript : MonoBehaviour
{
    [SerializeField] private Card card;
    [SerializeField] private SpriteRenderer splashArt;
    [SerializeField] private TextMeshPro nameText;
    [SerializeField] private TextMeshPro AttackCostText;
    [SerializeField] private TextMeshPro DrawCostText;
    [SerializeField] private TextMeshPro effectDiscriptionText;

    // Start is called before the first frame update
    void Start()
    {
        MakeCard();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeCard()
    {
        splashArt.sprite = card.cardSplashArt;
        nameText.text = card.cardName;
        gameObject.name = card.cardName;
        AttackCostText.text = card.attackBarCost.ToString();
        DrawCostText.text = card.drawBarCost.ToString();
        effectDiscriptionText.text = card.cardEffectDiscription;
    }
}
