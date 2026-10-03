using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _activationRadius = 2f;
    [SerializeField, Min(0f)] private float _explosionRadius = 3f;
    [SerializeField, Min(0f)] private float _explosionDelay = 1.5f;
    [SerializeField, Min(0f)] private float _damage = 25f;
    [SerializeField] private LayerMask _damageableMask = 0;

    private MineTimer _timer;
    private readonly HashSet<IDamageable> _damagedTargets = new HashSet<IDamageable>();

    public bool IsActivated => _timer != null && _timer.IsActivated;
    public bool HasExploded { get; private set; }

    private void Awake()
    {
        _timer = new MineTimer(_explosionDelay, this);
    }

    private void Update()
    {
        if (HasExploded)
            return;

        if (IsActivated == false && HasDamageableInRadius(_activationRadius))
            _timer.Activate();

        if (_timer.IsFinished)
        {
            HasExploded = true;
            DamageTargetsInRadius();
        }
            
    }

    private bool HasDamageableInRadius(float radius)
    {
        foreach (Collider targetCollider in GetOverlappingColliders(radius))
        {
            IDamageable target = FindDamageable(targetCollider);

            if (target != null && IsAlive(target))
                return true;
        }

        return false;
    }

    private void DamageTargetsInRadius()
    {
        _damagedTargets.Clear();

        foreach (Collider targetCollider in GetOverlappingColliders(_explosionRadius))
        {
            IDamageable target = FindDamageable(targetCollider);

            if (target != null && IsAlive(target) && _damagedTargets.Add(target))
                target.TakeDamage(_damage);
        }
    }

    private Collider[] GetOverlappingColliders(float radius) => Physics.OverlapSphere(
        transform.position,
        radius,
        _damageableMask,
        QueryTriggerInteraction.Collide);

    private static IDamageable FindDamageable(Collider targetCollider)
        => targetCollider.GetComponentInParent(typeof(IDamageable)) as IDamageable;

    private static bool IsAlive(IDamageable target)
    {
        if (target is IKillable killable)
            return killable.IsDead == false;

        return true;
    }

    private void OnDrawGizmos()
    {
        if (HasExploded == false)
            DrawRadius(_explosionRadius, Color.red);
    }

    private void OnDrawGizmosSelected()
    {
        if (HasExploded == false)
            DrawRadius(_activationRadius, Color.yellow);
    }

    private void DrawRadius(float radius, Color color)
    {
        Gizmos.color = color;
        Vector3 center = transform.position + Vector3.up * 0.05f;
        Vector3 previous = center + Vector3.right * radius;
        for (int i = 1; i <= 64; i++)
        {
            float angle = i * Mathf.PI * 2f / 64;
            Vector3 next = center + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
            Gizmos.DrawLine(previous, next);
            previous = next;
        }
    }

}
