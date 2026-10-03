using UnityEngine;

public class Health 
{
    private const float InjuredHealthRatio = 0.3f;

    public float Max { get; }
    public float Current { get; private set; }
    
    public bool IsDead => Current <= 0f;
    public bool IsInjured => IsDead == false && Current < Max * InjuredHealthRatio;
    
    public Health(float maxHealth)
    {
        Max = Mathf.Max(1f, maxHealth);
        Current = Max;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead || damage <= 0f)
            return;
        
        Current = Mathf.Max(0f, Current - damage);
    }

    public void Heal(float value)
    {
        if (IsDead || value <= 0f)
            return;
        
        Current = Mathf.Min(Max, Current + value);
    }

}
