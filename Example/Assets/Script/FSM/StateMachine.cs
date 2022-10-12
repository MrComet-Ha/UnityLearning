public class StateMachine<T> where T : class
{
    private T ownerEntity;
    private State<T> curState;
    private State<T> preState; // 이전 상태/State Blip(상태 블립) ? 에이전트의 상태가 변경되었을 때, 바로 직전 상태로 복귀한다는 조건하에 다른 상태로 변경하는 것. 예)평타 치다가 스킬을 쓰거나, 달리다가 다시 걷거나.
    private State<T> globalState;   //전역 상태 ? 에이전트가 상태를 수행할 시, 어떤 상태에서든 호출해야하는 조건 논리가 있을 때 이 논리를 소유하는 상태. 예)HP가 0이 되어 죽었거나, 피격 후 무적이 되었거나.
    public void SetUp(T owner, State<T> entryState){
        ownerEntity = owner;
        curState = null;
        preState = null;
        globalState = null;

        ChangeState(entryState);
    }

    public void Execute(){
        if(globalState != null){
            globalState.Execute(ownerEntity);
        }
        if(curState != null){
            curState.Execute(ownerEntity);
        }
    }

    public void ChangeState(State<T> newState){
        if(newState == null)
            return;
        if(curState != null){
            preState = curState;
            
            curState.Exit(ownerEntity);
        }

        curState = newState;
        curState.Enter(ownerEntity);
    }

    public void SetGlobalState(State<T> newState){
        globalState = newState;
    }

    public void RevertToPreviousState(){
        ChangeState(preState);
    }
}
