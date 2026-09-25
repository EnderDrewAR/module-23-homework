using UnityEngine;

public class MineTimer
{
    private readonly float _delay;
    private float _remainingTime;

    public bool IsActivated { get; private set; }
    public bool IsFinished => IsActivated && _remainingTime <= 0f;
    
    public MineTimer(float delay)
    {
        _delay = Mathf.Max(0f, delay);
    }

    public void Activate()
    {
        if (IsActivated)
            return;
        
        IsActivated = true;
        _remainingTime = _delay;
    }

    public void Update(float deltaTime)
    {
        if (!IsActivated || IsFinished)
            return;
        
        _remainingTime = Mathf.Max(0f, _remainingTime - Mathf.Max(0f, deltaTime));
    }
}
