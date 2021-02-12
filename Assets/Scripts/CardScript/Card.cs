using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Card", order = 3)]
public class Card : ScriptableObject
{
    [Header ("General Info")]
    public string cardName;
    public Sprite cardSplashArt;
    public string cardEffectDiscription;

    public enum cardType {Attack, Instant};
    public enum cardClass {A, B};
    [Header("Type and Class")]
    public cardType type;
    public cardClass Class;
    
    

    [Header ("Card Cost")]
    public float attackBarCost;
    public float drawBarCost;

    //[Header("Additional Script")]
    //public CardEffect script;

}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AttackCard", order = 1)]
public class AttackCard : Card
{
    public AttackCard()
    {
        type = cardType.Attack;
    }

}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/InstantCard", order = 2)]
public class InstantCard : Card
{
    public InstantCard()
    {
        type = cardType.Instant;
    }

}
