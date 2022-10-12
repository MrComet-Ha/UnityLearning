using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum playerState { Rest = 0, Travel, GoArena, WangOf,TakeADrink }
public class Player : BaseObjectState
{
    int power;
    int health;
    int fatigue;
    int exp;
    Locations curLocation;

    //Player 의 모든 상태와 현재 상태를 저장해줄 변수
    private State<Player>[] status;
    //private State<Player> curStatus;
    private StateMachine<Player> stateMachine;

    public int Power{
        set => power = Mathf.Max(0,value);
        get => power;
    }
    public int Health{
        set => health = Mathf.Max(0,value);
        get => health;
    }
    public int Fatigue{
        set => fatigue = Mathf.Max(0,value);
        get => fatigue;
    }
    public int Exp{
        set => exp = Mathf.Max(0,value);
        get => exp;
    }
    public Locations CurLocation{
        set => curLocation = value;
        get => curLocation;
    }

    public override void Setup(string name)
    {
        //BaseObjectState의 Setup 호출(ID,이름,색상 설정)
        base.Setup(name);
        gameObject.name = $"{ID:D2}_player_{name}";

        //status의 길이 선언
        status = new State<Player>[5];
        //status에 Rest 추가
        status[(int)playerState.Rest] = new PlayerOwnedStates.Rest();
        status[(int)playerState.Travel] = new PlayerOwnedStates.Travel();
        status[(int)playerState.GoArena] = new PlayerOwnedStates.GoArena();
        status[(int)playerState.WangOf] = new PlayerOwnedStates.WangOf();
        status[(int)playerState.TakeADrink] = new PlayerOwnedStates.TakeADrink();

        //현재 상태를 Rest로 변경
        //curStatus = status[(int)playerState.Rest];


        stateMachine = new StateMachine<Player>();
        stateMachine.SetUp(this,status[(int)playerState.Rest]);
        power = 0;
        health = 0;
        fatigue = 0;
        exp = 0;
        curLocation = Locations.Home;

        PrintText("자, 빵구쟁이들 안녕안녕.");
    }

    public override void Updated(){
        //PrintText("오늘은 뭘 할까...");
        /*if(curStatus != null){
            curStatus.Execute(this);
        }
        */
        stateMachine.Execute();
    }

    public void ChangeState(playerState newState){
        /*if(status[(int)newState] == null)
            return;
        if(curStatus != null){
            curStatus.Exit(this);
        }
        
        curStatus = status[(int)newState];
        curStatus.Enter(this);
        */
        stateMachine.ChangeState(status[(int)newState]);
    }
}
