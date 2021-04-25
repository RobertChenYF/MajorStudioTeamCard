using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferentRunStates : MonoBehaviour
{



}

public class BeforeCombat : RunState
{

    public BeforeCombat(RunStateManager runStateManager) : base(runStateManager)
    {

    }

    public override void StateBehavior()
    {
        //when player choose to enter combat
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("start");
        manager.SpawnNewMainEnemy();
        Services.combatManager.PauseTimeCycle();
        manager.CombatPreviewWindow.SetActive(true);
        //load enemy preview window


    }

    public override void Leave()
    {
        base.Leave();
        manager.CombatPreviewWindow.SetActive(false);
        //close enemy preview window
    }
}

public class Combat : RunState
{

    public Combat(RunStateManager runStateManager) : base(runStateManager)
    {

    }

    public override void StateBehavior()
    {
        //if all main enemy die
        if (Services.combatManager.AllMainEnemy.Count == 0)
        {
            manager.ChangeState(new Reward(manager));
        }
    }

    public override void Enter()
    {
        base.Enter();
        manager.AllCardsInGame.SetActive(true);
        manager.CombatUICanvasSet(true);
        Debug.Log("combat");
        manager.EmptyAllList();

        manager.CombatSetup();

        //load enemy
        
        //start time cycle

        Services.combatManager.ContinueTimeCycle();
        //load all cards

        //draw player 5 card
    }

    public override void Leave()
    {
        base.Leave();
        //clear all deck list
        Services.combatManager.PauseTimeCycle();
    }
}

public class Reward : RunState
{

    public Reward(RunStateManager runStateManager) : base(runStateManager)
    {

    }

    public override void StateBehavior()
    {
        //if player picked all reward go to combat overview state
        if (manager.currentSelectCard != null)
        {
            manager.selectRing.SetActive(true);
            manager.selectRing.transform.position = manager.currentSelectCard.transform.position;
            manager.chooseButton.interactable = true;

        }
    }

    public override void Enter()
    {
        base.Enter();
        manager.AllCardsInGame.SetActive(false);
        manager.CombatUICanvasSet(false);
        manager.RewardWindow.SetActive(true);
        manager.selectRing.SetActive(false);
        manager.skipButton.gameObject.SetActive(true);
        manager.chooseButton.gameObject.SetActive(true);
        manager.chooseButton.interactable = false;
        List<GameObject> rewardCards = manager.GenerateReward(manager.currentActiveClass1);
        manager.DisplayReward(rewardCards);
        Debug.Log("reward");
        //system choose reward
        //display reward
    }

    public override void Leave()
    {
        base.Leave();
        manager.RewardWindow.SetActive(false);
        manager.skipButton.gameObject.SetActive(false);
        manager.chooseButton.gameObject.SetActive(false);
        //close display window
    }
}


