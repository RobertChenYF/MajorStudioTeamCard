using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Card", order = 3)]
public class Card : ScriptableObject
{
    [Header ("General Info")]
    public string cardName;
    public Sprite cardSplashArt;
    [TextAreaAttribute(15,20)]
    public string cardEffectDiscription;

    public enum CardType {Compressed, Executable, Installer};
    //public enum CardClass { testing , Evasion, Vanilla, Basic, Support};
    public enum CardCompany {BasicSoftware, CompanyA,FileKillerCorp,Snorton};
    public enum Keywords {dedicate, delete, burn,  priority, harden,initial};
    [Header("Type and Class")]
    public CardType type;
    //public CardClass Class;
    public CardCompany Company;
    public List<Keywords> containedKeywords;

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
