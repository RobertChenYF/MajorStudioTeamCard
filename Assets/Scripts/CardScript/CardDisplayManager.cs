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
    private TextMeshPro CompanyNameText;
    private TextMeshPro keywordExplainText;
    private List<string> keywordDescription = new List<string>{ "<b>dedicate</b>: Draw a card if the card company matches this card trigger the effect",
        "<b>delete</b>: remove this card from this combat after the effect triggers",
        "<b>burn</b>: apply burn stack to the target, deal damage equal to the total stack of burn on the target, target loses a stack every time cycle",
        "<b>priority</b>: when you draw a card, draw this card first if this card is in your draw pile",
        "<b>harden</b>: the next time you take damage, take 0 damage and lose a stack of harden" };
    public static string[] companyFullName = {"Basic Software","","File Killer Corp.","","" };

    // Start is called before the first frame update
    void Awake()
    {
        
        
        
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

        card = GetComponent<CardFunction>().card;
        splashArt = transform.Find("splash").GetComponent<SpriteRenderer>();
        nameText = gameObject.transform.Find("CardNameText").GetComponent<TextMeshPro>();
        AttackCostText = transform.Find("AttackCost").GetComponent<TextMeshPro>();
        DrawCostText = transform.Find("DrawCost").GetComponent<TextMeshPro>();
        effectDiscriptionText = transform.Find("CardDescription").GetComponent<TextMeshPro>();
        CompanyNameText = transform.Find("CardCompanyDisplay").GetComponent<TextMeshPro>();
        keywordExplainText = transform.Find("keywordText").GetComponent<TextMeshPro>();
        splashArt.sprite = card.cardSplashArt;
        nameText.text = card.cardName;
        cardType = card.type;
        gameObject.name = card.cardName;
        attackCost = card.attackBarCost;
        drawCost = card.drawBarCost;
        AttackCostText.text = attackCost.ToString();
        DrawCostText.text = drawCost.ToString();
        effectDiscriptionText.text = card.cardEffectDiscription;
        CompanyNameText.text = companyFullName[(int)card.Company];
        string temp = "";
        foreach (Card.Keywords a in card.containedKeywords)
        {
            temp += keywordDescription[(int)a] + "\n";
        }
        keywordExplainText.text = temp;
        GetComponent<CardFunction>().keywordTextBox = keywordExplainText.gameObject;
        keywordExplainText.gameObject.SetActive(false);
    }
}
