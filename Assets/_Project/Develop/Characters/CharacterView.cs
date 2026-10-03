using UnityEngine;

public class CharacterView : MonoBehaviour
{
    private const float DeadZone = 0.05f;
    
    // Animations
    private readonly int IsRunningKey = Animator.StringToHash("IsRunning");
    private readonly int IsInjuredKey = Animator.StringToHash("IsInjured");
    private readonly int IsDeadKey = Animator.StringToHash("IsDead");
    private readonly int HitKey = Animator.StringToHash("Hit");
    private readonly int InJumpProcessKey = Animator.StringToHash("InJumpProcess");

    [SerializeField] private Animator _animator;
    [SerializeField] private Character _character;
    [SerializeField] private AudioSource _jumpAudioSource;

    private bool _wasJumping;
    private int _lastDamageCount;

    private void PlayJumpSound()
    {
        if (_jumpAudioSource != null)
            _jumpAudioSource.Play();
    }

    private void ShowDamage()
    {
        if (_character.IsDead)
        {
            _animator.ResetTrigger(HitKey);
            _animator.SetBool(IsDeadKey, true);
        }
        else if (_character.IsMovementBlocked == false)
        {
            _animator.SetTrigger(HitKey);
        }
    }

    private void LateUpdate()
    {
        if (_character.DamageCount != _lastDamageCount)
        {
            ShowDamage();
            _lastDamageCount = _character.DamageCount;
        }

        bool isJumping = _character.InJumpProcess;

        if (isJumping && _wasJumping == false)
            PlayJumpSound();

        _wasJumping = isJumping;

        _animator.SetBool(IsDeadKey, _character.IsDead);
        _animator.SetBool(IsInjuredKey, _character.IsInjured);
        _animator.SetBool(InJumpProcessKey, _character.CanMove && _character.InJumpProcess);

        if (_character.CanMove && _character.CurrentVelocity.magnitude > DeadZone)
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
