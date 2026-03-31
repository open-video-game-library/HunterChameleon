public class StateMachine
{
    public IState currentState;

    public void ChangeState(IState newState)
    {
        if (newState == currentState) { return; }

        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Tick()
    {
        currentState?.Tick();
    }
}