using UnityEngine;

public interface IState 
{
    public void EnterState();
    public void ExcuteState();
    public void ExitState();
}
