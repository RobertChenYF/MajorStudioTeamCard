using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Card", order = 3)]
public class Card : ScriptableObject
{
    [Header ("General Info")]
    public string cardName;
    public Sprite cardSplashArt;
    public string cardEffectDiscription;

    public enum CardType {Attack, Instant};
    public enum CardClass { testing };
    [Header("Type and Class")]
    public CardType type;
    public CardClass Class;
    
    

    [Header ("Card Cost")]
    public float attackBarCost;
    public float drawBarCost;


}

/*[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AttackCard", order = 1)]
public class AttackCard : Card
{
    public AttackCard()
    {
        type = CardType.Attack;
    }

}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/InstantCard", order = 2)]
public class InstantCard : Card
{
    public InstantCard()
    {
        type = CardType.Instant;
    }

}

    */