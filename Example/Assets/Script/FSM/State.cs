public abstract class State<T> where T : class
{
    //해당 상태에 진입했을 때 1회 호출
    public abstract void Enter(T entity);

    //해당 상태에서 빠져나갈 때 1회 호출
    public abstract void Exit(T entity);

    //해당 상태를 업데이트 할 때 매 프레임마다 호출
    public abstract void Execute(T entity);
}
