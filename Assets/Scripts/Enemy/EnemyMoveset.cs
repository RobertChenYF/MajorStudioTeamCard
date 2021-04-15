using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyMoveset : MonoBehaviour
{
    public enum moveType {dealDamage,GainArmor,Special};
    [HideInInspector]public Enemy enemy;
    [SerializeField] public int CycleBeforeMove;
    [SerializeField] protected int damageAmount;
    [SerializeField] protected int armorAmount;
    [SerializeField] protected string specialMoveExplain;
    [SerializeField] protected List<moveType> moves;
    [SerializeField] protected UnityEvent SpecialMoveset;

    public virtual void MoveTrigger()
    {
        foreach (moveType a in moves)
        {
            if (a == moveType.dealDamage)
            {
                Services.statsManager.TakeDamage(damageAmount);
            }
            else if (a == moveType.GainArmor)
            {
                enemy.GainArmor(armorAmount);
            }
            else if (a == moveType.Special)
            {
                SpecialMoveset.Invoke();
            }
        }
    }

    public virtual string MoveDisplay()
    {
        string moveString = "";
        if (moves.Contains(moveType.dealDamage))
        {
            moveString += "deal " + damageAmount + " damage" + "\n";
        }
        else if (moves.Contains(moveType.GainArmor))
        {
            moveString += "gain " + armorAmount + " armor\n";
        }
        else if (moves.Contains(moveType.Special))
        {
            moveString += specialMoveExplain;
        }
        return moveString;
    }

    
}
