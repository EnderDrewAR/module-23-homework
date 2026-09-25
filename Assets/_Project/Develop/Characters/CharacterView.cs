using UnityEngine;

public class CharacterView : MonoBehaviour
{
    private const float DeadZone = 0.05f;
    private readonly int IsRunningKey = Animator.StringToHash("IsRunning");
    private readonly int IsInjuredKey = Animator.StringToHash("IsInjured");
    private readonly int IsDeadKey = Animator.StringToHash("IsDead");
    private readonly int HitKey = Animator.StringToHash("Hit");

    [SerializeField] private Animator _animator;
    [SerializeField] private Character _character;

    private void OnEnable() => _character.Damaged += OnDamaged;

    private void OnDisable() => _character.Damaged -= OnDamaged;

    private void OnDamaged()
    {
        if (_character.IsDead)
        {
            _animator.ResetTrigger(HitKey);
            _animator.SetBool(IsDeadKey, true);
        }
        else
        {
            _animator.SetTrigger(HitKey);
        }
    }

    private void LateUpdate()
    {
        _animator.SetBool(IsDeadKey, _character.IsDead);
        _animator.SetBool(IsInjuredKey, _character.IsInjured);

        if (!_character.IsDead && _character.CurrentVelocity.magnitude > DeadZone)
            StartRunning();
        else
            StopRunning();
    }

    private void StartRunning()
    {
        _animator.SetBool(IsRunningKey, true);
    }
    
    private void StopRunning()
    {
        _animator.SetBool(IsRunningKey, false);
    }

}
