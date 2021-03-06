using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferentRunStates : MonoBehaviour
{



}

public class BeforeCombat : RunState
{
    GameObject tempEnemy;
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
        //Debug.Log("start");

        //manager.SpawnNewMainEnemy();
        tempEnemy = manager.InstantiateEnemyPreview();
        Services.combatManager.PauseTimeCycle();
        manager.CombatPreviewWindow.SetActive(true);
        //load enemy preview window


    }

    public override void Leave()
    {
        base.Leave();
        // manager.DestroyThis(tempEnemy);
        // Services.combatManager.AllMainEnemy.Clear();
        manager.moveTransform(tempEnemy);
        manager.CombatPreviewWindow.SetActive(false);
        //close enemy preview window
    }
}

public class Combat : RunState
{
    private float countDown;
    public Combat(RunStateManager runStateManager) : base(runStateManager)
    {

    }

    public override void StateBehavior()
    {
        //if all main enemy die
        if (Services.combatManager.AllMainEnemy.Count == 0)
        {
            if (countDown > 1.5f)
            {
                manager.currentStage++;
                if (manager.currentStage >= Services.runStateManager.AllEnemyList.Count)
                {
                    manager.ChangeState(new GameWin(manager));
                }
                else
                {
                    manager.ChangeState(new Reward(manager));
                }
            }

            countDown += Time.deltaTime;
        }

    }

    public override void Enter()
    {
        base.Enter();
        
        Services.actionManager.ResetBasicActionCost();
        Services.resourceManager.resetResource();
        Services.combatManager.resetCycleTimer();
        manager.AllCardsInGame.SetActive(true);
        manager.CombatUICanvasSet(true);
        //Debug.Log("combat");
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
        manager.draftLeft = 2;

        Services.eventManager.Fire(new CombatEndEvent());
        //clear all deck list
        //Services.actionManager.StopAttack();
        Services.combatManager.PauseTimeCycle();
        //Services.combatManager.PauseTimeCycle();
        manager.AllCardsInGame.SetActive(false);
        manager.CombatUICanvasSet(false);
    }

    public class CombatEndEvent: AGPEvent
    {
        public CombatEndEvent(){

        }
    }
}

public class Gameover : RunState
{
    public Gameover(RunStateManager runStateManager) : base(runStateManager)
    {

    }

    public override void Enter()
    {
        base.Enter();
        //set gameover screen on
        manager.GameLoseScreen.SetActive(true);
    }

    public override void Leave()
    {
        base.Leave();
    }

    public override void StateBehavior()
    {
        
    }
}

public class GameWin : RunState
{
    public GameWin(RunStateManager runStateManager) : base(runStateManager)
    {

    }

    public override void Enter()
    {
        base.Enter();
        manager.GameWinScreen.SetActive(true);
        //set gamewin screen on
    }

    public override void Leave()
    {
        base.Leave();
    }

    public override void StateBehavior()
    {

    }
}

public class Reward : RunState
{
    List<GameObject> rewardCards = new List<GameObject>();
    public Reward(RunStateManager runStateManager) : base(runStateManager)
    {

    }

    public override void StateBehavior()
    {
        Services.combatManager.PauseTimeCycle();
        
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

        manager.RewardWindow.SetActive(true);
        manager.selectRing.SetActive(false);
        Services.combatManager.PauseTimeCycle();
        manager.skipButton.gameObject.SetActive(true);
        manager.chooseButton.gameObject.SetActive(true);
        manager.chooseButton.interactable = false;
        // List<GameObject> rewardCards = manager.GenerateReward(manager.currentActiveClass1);

        rewardCards = manager.GenerateRewardMixPool(manager.currentActiveClass1,manager.currentActiveClass2);
        manager.DisplayReward(rewardCards);
        //Debug.Log("reward");
        //system choose reward
        //display reward
    }

    public override void Leave()
    {
        base.Leave();
        foreach (GameObject a in rewardCards)
        {
            a.SetActive(false);
        }
        manager.RewardWindow.SetActive(false);
        manager.skipButton.gameObject.SetActive(false);
        manager.chooseButton.gameObject.SetActive(false);
        //close display window
    }
}


