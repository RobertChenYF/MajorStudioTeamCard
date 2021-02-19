using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class CardDisplayManager : MonoBehaviour
{
    
    private Card card;
    private float attackCost;
    private float drawCost;
    private Card.CardType cardType;
    private SpriteRenderer splashArt;
    private TextMeshPro nameText;
    private TextMeshPro AttackCostText;
    private TextMeshPro DrawCostText;
    private TextMeshPro effectDiscriptionText;

    // Start is called before the first frame update
    void Awake()
    {
        
        
        card = GetComponent<CardFunction>().card;
        splashArt = transform.Find("splash").GetComponent<SpriteRenderer>();
        nameText = gameObject.transform.Find("CardNameText").GetComponent<TextMeshPro>();
        AttackCostText = transform.Find("AttackCost").GetComponent<TextMeshPro>();
        DrawCostText = transform.Find("DrawCost").GetComponent<TextMeshPro>();
        effectDiscriptionText = transform.Find("CardDescription").GetComponent<TextMeshPro>();
    }

    void Start()
    {
        enabled = false;
    }
    void Update()
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {

        
        splashArt.sprite = card.cardSplashArt;
        nameText.text = card.cardName;
        cardType = card.type;
        gameObject.name = card.cardName;
        attackCost = card.attackBarCost;
        drawCost = card.drawBarCost;
        AttackCostText.text = attackCost.ToString();
        DrawCostText.text = drawCost.ToString();
        effectDiscriptionText.text = card.cardEffectDiscription;
    }
}
