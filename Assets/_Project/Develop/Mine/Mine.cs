using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _activationRadius = 2f;
    [SerializeField, Min(0f)] private float _explosionRadius = 3f;
    [SerializeField, Min(0f)] private float _explosionDelay = 1.5f;
    [SerializeField, Min(0f)] private float _damage = 25f;
 
    private IDamageableTarget _target;
    private MineTimer _timer;
    
    public bool IsActivated => _timer != null && _timer.IsActivated;
    public bool HasExploded { get; private set; }

    private void Awake() => _timer = new MineTimer(_explosionDelay);

    public void Initialize(IDamageableTarget target) => _target = target;

    private void Update() => Tick(Time.deltaTime);

    public void Tick(float deltaTime)
    {
        if (HasExploded)
            return;

        if (!IsActivated && IsTargetAlive() && IsTargetInRadius(_activationRadius))
            _timer.Activate();

        _timer.Update(deltaTime);

        if (_timer.IsFinished)
        {
            HasExploded = true;
            if (IsTargetAlive() && IsTargetInRadius(_explosionRadius))
                _target.TakeDamage(_damage);
        }
    }

    private bool IsTargetAlive()
    {
        if (_target == null)
            return false;
        if (_target is Object unityObject && unityObject == null)
            return false;
        return !_target.IsDead;
    }

    private bool IsTargetInRadius(float radius)
    {
        Vector3 offset = _target.Position - transform.position;
        offset.y = 0f;
        return offset.sqrMagnitude <= radius * radius;
    }

    private void OnDrawGizmos()
    {
        if (!HasExploded)
            DrawRadius(_explosionRadius, Color.red);
    }

    private void OnDrawGizmosSelected()
    {
        if (!HasExploded)
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
