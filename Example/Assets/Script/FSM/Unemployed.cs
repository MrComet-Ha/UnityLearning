using System;
using UnityEngine;

public enum UnemployedStates{ Rest = 0, WangOf, TakeADrink, VisitBathroom, Global}
public class Unemployed : BaseObjectState
{
    int bored;
    int stress;
    int fatigue;
    Locations curLocation;

    public State<Unemployed>[] status;
    public StateMachine<Unemployed> stateMachine;

    public int Bored{
        set => bored = (int)MathF.Max(0,value);
        get => bored;
    }
    public int Stress{
        set => stress = (int)MathF.Max(0,value);
        get => stress;
    }
    public int Fatigue{
        set => fatigue = (int)MathF.Max(0,value);
        get => fatigue;
    }
    public Locations CurLocation{
        set => curLocation = value;
        get => curLocation;
    }

    public UnemployedStates CurState { private set; get;}  //현재 상태

    public override void Setup(string name){
        base.Setup(name);

        gameObject.name = $"{ID:D2}_Unemployed_{name}";

        status = new State<Unemployed>[5];
        status[(int)UnemployedStates.Rest] = new UnemployedOwnStates.Rest();
        status[(int)UnemployedStates.WangOf] = new UnemployedOwnStates.WangOf();
        status[(int)UnemployedStates.TakeADrink] = new UnemployedOwnStates.TakeADrink();
        status[(int)UnemployedStates.VisitBathroom] = new UnemployedOwnStates.VisitBathroom();
        status[(int)UnemployedStates.Global] = new UnemployedOwnStates.StateGlobal();

        stateMachine = new StateMachine<Unemployed>();
        stateMachine.SetUp(this, status[(int)UnemployedStates.Rest]);

        bored = 0;
        stress = 0;
        fatigue = 0;
        curLocation = Locations.Home;
    }

    public override void Updated(){
        stateMachine.Execute();
    }

    public void ChangeState(UnemployedStates newState){
        CurState = newState;

        stateMachine.ChangeState(status[(int)newState]);
    }

    public void RevertToPreviousState(){
        stateMachine.RevertToPreviousState();
    }
}
