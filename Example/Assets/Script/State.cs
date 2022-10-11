public abstract class State
{
    //해당 상태에 진입했을 때 1회 호출
    public abstract void Enter(Player entity);

    //해당 상태에서 빠져나갈 때 1회 호출
    public abstract void Exit(Player entity);

    //해당 상태를 업데이트 할 때 매 프레임마다 호출
    public abstract void Execute(Player entity);
}
