using UnityEngine;

public class ActionManager : MonoBehaviour
{
    // 현재 진행중인 액션이 무엇인지 알기 위해
    private IAction _current = null;

    public void StartAction(IAction action)
    {
        // 요구한 액션이 같은 액션이면 리턴
        if (_current == action) return;

        // 기존 액션 끝내고 액션 변경
        if (_current != null)
            _current.End();
        _current = action;
    }
}