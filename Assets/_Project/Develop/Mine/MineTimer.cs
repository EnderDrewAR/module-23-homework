using System.Collections;
using UnityEngine;

public class MineTimer
{
    private readonly float _delay;
    private readonly MonoBehaviour _coroutineRunner;
    private float _remainingTime;

    public bool IsActivated { get; private set; }
    public bool IsFinished { get; private set; }
    
    public MineTimer(float delay, MonoBehaviour coroutineRunner)
    {
        _coroutineRunner = coroutineRunner;
        _delay = Mathf.Max(0f, delay);
        
        IsActivated = false;
        IsFinished = false;
    }

    public void Activate()
    {
        if (IsActivated)
            return;
        
        IsActivated = true;
        _coroutineRunner.StartCoroutine(TimerProcess());
    }

    private IEnumerator TimerProcess()
    {
        yield return new WaitForSeconds(_delay);

        IsFinished = true;
    }
}
