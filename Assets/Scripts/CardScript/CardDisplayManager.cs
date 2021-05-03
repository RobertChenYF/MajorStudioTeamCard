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
    private SpriteRenderer CostIndicator;
    private SpriteRenderer Background;
    private SpriteRenderer descriptionBG;
    private SpriteRenderer TopNameBG;
    private SpriteRenderer CompanyNameBG;
    private TextMeshPro nameText;
    private TextMeshPro CostText;
    //private TextMeshPro DrawCostText;
    private TextMeshPro effectDiscriptionText;
    private TextMeshPro CompanyNameText;
    private TextMeshPro keywordExplainText;
    [SerializeField]private Sprite GPUCost;
    [SerializeField]private Sprite CPUCost;
    private List<string> keywordDescription = new List<string>{ "<b>Dedicate</b>: Draw a card. If the card comes from the same company, trigger this effect.",
        "<b>Delete</b>: This card will not return to your draw deck after play. (If you picked this card, it will remain in your run deck.)",
        "<b>Burn</b>: Applies a debuff to the target. When applying new burns to the target, deal damage equal to the previous burn amount. Burn reduces by one per cycle.",
        "<b>Priority</b>: This card goes at the top of your draw pile.",
        "<b>Harden</b>: Become immune to damage. Harden is lost everytime you would have taken damage.",
        "<b>Wipe</b>: A cheap, one use 5 damage card.",
        "<b>Sequence</b>: Triggers an effect when you have 4 cards waiting for decompression.",
        "<b>Initial</b>: Has an additional effect when the first card to be decompressed.",
        "<b>Usage</b>: Has a limited amount of plays before being permanantly removed from your deck.",
        "<b>Generated</b>: A card created by another card."
        };
    public static string[] companyFullName = {"Basic Software","HackerzAren'tUs Inc.","File Killer Corp.","Snorton","" };

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
        Background = transform.Find("cardBackground").GetComponent<SpriteRenderer>();
        descriptionBG = transform.Find("carddesc").GetComponent<SpriteRenderer>();
        CompanyNameBG = transform.Find("bottom background").GetComponent<SpriteRenderer>();
        TopNameBG = transform.Find("title background").GetComponent<SpriteRenderer>();
        CostIndicator = transform.Find("CostIcon").GetComponent<SpriteRenderer>();
        nameText = gameObject.transform.Find("CardNameText").GetComponent<TextMeshPro>();
        CostText = transform.Find("Cost").GetComponent<TextMeshPro>();
        //DrawCostText = transform.Find("DrawCost").GetComponent<TextMeshPro>();
        effectDiscriptionText = transform.Find("CardDescription").GetComponent<TextMeshPro>();
        CompanyNameText = transform.Find("CardCompanyDisplay").GetComponent<TextMeshPro>();
        keywordExplainText = transform.Find("keywordText").GetComponent<TextMeshPro>();
        splashArt.sprite = card.cardSplashArt;
        nameText.text = card.cardName;
        cardType = card.type;
        gameObject.name = card.cardName;
        attackCost = card.attackBarCost;
        drawCost = card.drawBarCost;

        if (attackCost > 0)
        {
            CostText.text = attackCost.ToString();
            CostIndicator.sprite = Services.cardList.CPUCostIcon;
        }
        else if(drawCost > 0)
        {
            CostText.text = drawCost.ToString();
            CostIndicator.sprite = Services.cardList.GPUCostIcon;
        }
        else
        {
            CostText.text = drawCost.ToString();
        }

        //DrawCostText.text = drawCost.ToString();
        int index = 0;
        if (card.Company == Card.CardCompany.BasicSoftware)
        {
            index = 0;
        }
        else if (card.Company == Card.CardCompany.FileKillerCorp)
        {
            index = 1;
        }
        else if (card.Company == Card.CardCompany.Snorton)
        {
            index = 2;
        }
        Background.sprite = Services.cardList.CardBackGround[index];
        TopNameBG.sprite = Services.cardList.CardTitleBackGround[index];
        CompanyNameBG.sprite = Services.cardList.CardTitleBackGround[index];
        descriptionBG.sprite = Services.cardList.CardDescriptionBackGround[index];
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
