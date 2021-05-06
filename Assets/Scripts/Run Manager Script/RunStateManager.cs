using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunStateManager : MonoBehaviour
{
    [HideInInspector] public CardClass currentActiveClass1;
    [HideInInspector] public CardClass currentActiveClass2;
    public RunState currentRunState;
    public List<CardFunction> playerRunDeck;
    public List<GameObject> playerCardGameobject;
    public List<GameObject> AllEnemyList;

    [SerializeField] private GameObject CombatUICanvas;
    [SerializeField] public GameObject RewardWindow;
    [SerializeField] public GameObject CombatPreviewWindow;
    [SerializeField] public GameObject AllCardsInGame;
    

    [SerializeField] private Transform cardPos1;
    [SerializeField] private Transform cardPos2;
    [SerializeField] private Transform cardPos3;

    public Button skipButton;
    public Button chooseButton;
    public GameObject selectRing;
    public CardFunction currentSelectCard = null;

    public int draftLeft = 2;
    //public CardClass currentActiveClass2;

    // Start is called before the first frame update
    void Start()
    {
        currentActiveClass1 = Services.cardList.FileKillerCorp;
        currentActiveClass2 = Services.cardList.Snorton;
        if (draftLeft > 0)
        {
            ChangeState(new Reward(this));
        }
        else
        {
            ChangeState(new BeforeCombat(this));
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentRunState.StateBehavior();
    }

    public void ChangeState(RunState newState)
    {
        if (currentRunState != null) currentRunState.Leave();
        currentRunState = newState;
        currentRunState.Enter();
    }

    public void SpawnNewMainEnemy()
    {
        GameObject newEnemy = Instantiate(AllEnemyList[0], Services.actionManager.enemyDefaultPos.position,Quaternion.identity);

    }

    public void CombatUICanvasSet(bool a)
    {
        CombatUICanvas.SetActive(a);
    }

    public void EmptyAllList()
    {
        Services.actionManager.DrawDeck.Clear();
        Services.actionManager.DeletePile.Clear();
        Services.actionManager.DiscardPile.Clear();
        Services.actionManager.AttackField.Clear();
        Services.actionManager.PriorityDeck.Clear();
        Services.actionManager.PlayerHand.Clear();
    }
    public class CombatStart : AGPEvent
    {

        public CombatStart()
        {

        }
    }
    public void CombatSetup()
    {
        
        CombatUICanvas.SetActive(true);
        //set player armor to 0
        Services.statsManager.LoseAllArmor();
        Services.playerBuffManager.RemoveAllBuff();
        Services.actionManager.TempStart();
        Services.eventManager.Fire(new CombatStart());
        //set cost for 2 basic action
        //set player resource
        foreach (CardFunction card in playerRunDeck)
        {
            Services.actionManager.AddToDrawPile(card);


        }
        Services.actionManager.TempStart();
        Services.actionManager.DrawMutipleCard(5);
    }

    public void AddToRunReck(CardFunction card)
    {
        playerRunDeck.Add(card);
        card.gameObject.transform.SetParent(AllCardsInGame.transform);
    }

    public List<GameObject> GenerateReward(CardClass cardClass)
    {
        List<GameObject> rewardCard = new List<GameObject>();

        List<int> cardCode = new List<int>();

        while (cardCode.Count < 3)
        {
            int a = Random.Range(0, cardClass.AllClassCard.Count);
            if (!cardCode.Contains(a))
            {
                cardCode.Add(a);
            }
        }

        foreach (int a in cardCode)
        {
            GameObject card = Instantiate(cardClass.AllClassCard[a]);
            card.transform.SetParent(RewardWindow.transform);
            rewardCard.Add(card);
        }
        return rewardCard;
    }

    public List<GameObject> GenerateRewardMixPool(CardClass cardClassA,CardClass cardClassB)
    {
        List<GameObject> rewardCard = new List<GameObject>();
        List<GameObject> cardPool = new List<GameObject>();
        foreach (GameObject card in cardClassA.AllClassCard)
        {
            cardPool.Add(card);
        }
        foreach (GameObject card in cardClassB.AllClassCard)
        {
            cardPool.Add(card);
        }
        List<int> cardCode = new List<int>();

        while (cardCode.Count < 3)
        {
            int a = Random.Range(0, cardPool.Count);
            if (!cardCode.Contains(a))
            {
                cardCode.Add(a);
            }
        }

        foreach (int a in cardCode)
        {
            GameObject card = Instantiate(cardPool[a]);
            card.transform.SetParent(RewardWindow.transform);
            rewardCard.Add(card);
        }
        return rewardCard;
    }

    public void DisplayReward(List<GameObject> cards)
    {
        cards[0].transform.position = cardPos1.position;
        cards[1].transform.position = cardPos2.position;
        cards[2].transform.position = cardPos3.position;
    }

    public void PressSkipButton()
    {
        draftLeft --;
        if (draftLeft == 0)
        {
            ChangeState(new BeforeCombat(this));
        }
        else if (draftLeft > 0)
        {
            ChangeState(new Reward(this));
        }
    }

    public void PressChooseButton()
    {
        AddToRunReck(currentSelectCard);
        currentSelectCard = null;
        draftLeft--;
        if (draftLeft == 0)
        {
            ChangeState(new BeforeCombat(this));
        }
        else if (draftLeft > 0)
        {
            ChangeState(new Reward(this));
        }
        
    }

    public void PressEnterCombat()
    {
        ChangeState(new Combat(this));
    }
}
