using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour, IVelocitySource, IDirectionalRotatable, IDamageableTarget
{
    public event System.Action Damaged;
    private NavMeshAgent _agent;
    private AgentMover _mover;
    private DirectionalRotator _rotator;
    private Health _health;
    
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField, Min(1f)] private float _maxHealth = 100f;
    
    public float CurrentHealth => _health.Current;
    public float MaxHealth => _health.Max;
    public bool IsDead => _health.IsDead;
    public bool IsInjured => _health.IsInjured;
    
    public Vector3 CurrentVelocity => _mover.CurrentVelocity;
    public Quaternion CurrentRotation => _rotator.CurrentRotation;
    public Vector3 Position => transform.position;

    private void Awake()
    {
        _health = new Health(_maxHealth);
        
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;

        _mover = new AgentMover(_agent, _moveSpeed);
        _rotator = new DirectionalRotator(transform, _rotationSpeed);
    }

    private void Update()
    {
        if (IsDead)
            return;
        
        _rotator.Update(Time.deltaTime);
    }
    
    public void TakeDamage(float damage)
    {
        if (IsDead || damage <= 0f)
            return;

        _health.TakeDamage(damage);

        if (IsDead)
            StopMove();

        Damaged?.Invoke();
    }
    
    public void SetDestination(Vector3 position)
    {
        if (!IsDead)
            _mover.SetDestination(position);
    }

    public bool SetPath(NavMeshPath path) => !IsDead && _mover.SetPath(path);

    public void StopMove() => _mover.Stop();

    public void ResumeMove()
    {
        if (!IsDead)
            _mover.Resume();
    }

    public void SetRotationDirection(Vector3 direction)
    {
        if (!IsDead)
            _rotator.SetInputDirection(direction);
    }

    public bool TryGetPath(Vector3 targetPosition, NavMeshPath pathToTarget)
        => !IsDead && NavMeshUtils.TryGetPath(_agent, targetPosition, pathToTarget);
    
    
}
