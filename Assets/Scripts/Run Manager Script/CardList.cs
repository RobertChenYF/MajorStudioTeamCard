using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class CardList : MonoBehaviour
{
    public Object[] tempLoad;
    //public List<GameObject> FileKillerCorpStartingCard;
    //public List<GameObject> FileKillerCorpClassCard;
    private List<CardClass> allExistingCardClassInGame;
    public CardClass FileKillerCorp;
    public CardClass Snorton;
    public Sprite GPUCostIcon;
    public Sprite CPUCostIcon;
    public GameObject cardWipe;

    public List<Sprite> CardBackGround;
    public List<Sprite> CardDescriptionBackGround;
    public List<Sprite> CardTitleBackGround;

    public Sprite AttackIntentIcon;
    public Sprite ArmorIntentIcon;
    public Sprite SpecialIntentIcon;
    // Start is called before the first frame update
    void Awake()
    {
        FileKillerCorp = new CardClass("File Killer Corp.", "FileKillerCorp");
        Snorton = new CardClass("Snorton", "Snorton");
        allExistingCardClassInGame = new List<CardClass> { FileKillerCorp,Snorton };
        
        
        LoadCard();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadCard()
    {
        foreach (CardClass cardClass in allExistingCardClassInGame)
        {
            
            tempLoad = Resources.LoadAll(cardClass.FileName+"/card prefabs", typeof(GameObject));
            cardClass.AllClassCard = ArrayToList(tempLoad);
            //tempLoad = Resources.LoadAll(cardClass.FileName + "/starting card", typeof(GameObject));
            //cardClass.startingCard = ArrayToList(tempLoad);
            //load starting card
        }
        
        //FileKillerCorpClassCard = ArrayToList(tempLoad);
    }
    public List<GameObject> ArrayToList(Object[] array)
    {
        List<GameObject> list = new List<GameObject>();
        
        list = array.OfType<Object>().Cast<GameObject>().ToList();
        return list;
    }
}


public class CardClass
{
    public List<GameObject> startingCard;
    public List<GameObject> AllClassCard;
    public string Name;
    public string FileName;
    public CardClass(string name,string fileName)
    {
        Name = name;
        
        FileName = fileName;
    }
}
