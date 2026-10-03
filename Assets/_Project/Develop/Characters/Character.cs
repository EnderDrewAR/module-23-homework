using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour, IVelocitySource, IDirectionalRotatable, IDamageable, IKillable
{
    private NavMeshAgent _agent;
    
    private AgentMover _mover;
    private DirectionalRotator _rotator;
    private AgentJumper _jumper;
    private bool _resumeAfterHit;
    
    private Health _health;
    
    // Mover and Rotator
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    
    // Jumper
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private AnimationCurve _jumpCurve;
    
    // Health
    [SerializeField, Min(1f)] private float _maxHealth = 100f;
    [SerializeField] private AudioSource _healAudioSource;
    
    // Health --- property
    public float CurrentHealth => _health.Current;
    public float MaxHealth => _health.Max;
    public bool IsDead => _health.IsDead;
    public bool IsInjured => _health.IsInjured;
    public int DamageCount { get; private set; }

    public void Heal(float value)
    {
        _health.Heal(value);
        
        if (_healAudioSource != null)
            _healAudioSource.Play();
    } 
    
    // Mover and Rotator --- property
    public Vector3 CurrentVelocity => _mover.CurrentVelocity;
    public Quaternion CurrentRotation => _rotator.CurrentRotation;
    public Vector3 Position => transform.position;
    public bool HasDestination => _mover.HasDestination;
    public Vector3 Destination => _mover.Destination;
    public float StoppingDistance => _mover.StoppingDistance;
    public bool IsMovementBlocked { get; private set; }
    public bool CanMove => IsDead == false && IsMovementBlocked == false;

    // Jumper --- property
    public bool InJumpProcess => _jumper.InProcess;
    
    private void Awake()
    {
        _health = new Health(_maxHealth);
        
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;

        _mover = new AgentMover(_agent, _moveSpeed);
        _rotator = new DirectionalRotator(transform, _rotationSpeed);
        _jumper = new AgentJumper(_jumpSpeed, _jumpCurve, this, _agent);
    }

    private void Update()
    {
        if (CanMove == false)
            return;
        
        _rotator.Update(Time.deltaTime);
    }
    
    public void TakeDamage(float damage)
    {
        if (IsDead || damage <= 0f)
            return;

        _health.TakeDamage(damage);

        if (IsDead)
        {
            StopMove();
            _jumper.Cancel();
        }

        DamageCount++;
    }
    
    public void SetDestination(Vector3 position)
    {
        if (CanMove)
            _mover.SetDestination(position);
    }

    public bool SetPath(NavMeshPath path) => CanMove && _mover.SetPath(path);

    public void StopMove() => _mover.Stop();

    public void ResumeMove()
    {
        if (CanMove)
            _mover.Resume();
    }

    public void SetRotationDirection(Vector3 direction)
    {
        if (CanMove)
            _rotator.SetInputDirection(direction);
    }

    public bool TryGetPath(Vector3 targetPosition, NavMeshPath pathToTarget)
        => CanMove && NavMeshUtils.TryGetPath(_agent, targetPosition, pathToTarget);

    public void Jump(OffMeshLinkData offMeshLinkData)
    {
        if (CanMove == false || _jumper.InProcess)
            return;

        _jumper.Jump(offMeshLinkData);
    }
    
    public bool IsOnNavMeshLink(out OffMeshLinkData offMeshLinkData)
    {
        if (_agent.isOnOffMeshLink)
        {
            offMeshLinkData = _agent.currentOffMeshLinkData;
            return true;
        }

        offMeshLinkData = default(OffMeshLinkData);
        return false;
    }
    
    public void BeginHitReaction()
    {
        if (IsDead || IsMovementBlocked)
            return;

        _resumeAfterHit = HasDestination || InJumpProcess;
        IsMovementBlocked = true;

        StopMove();
        _jumper.IsPaused = true;
    }

    public void EndHitReaction()
    {
        IsMovementBlocked = false;
        _jumper.IsPaused = false;

        if (_resumeAfterHit && IsDead == false)
            ResumeMove();

        _resumeAfterHit = false;
    }
}
