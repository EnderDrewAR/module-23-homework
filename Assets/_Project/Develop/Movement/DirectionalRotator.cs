using UnityEngine;

public class DirectionalRotator 
{
    private Transform _transform;

    private float _rotationSpeed;
    
    private Vector3 _currentDirection;
    
    private const float DeadZone = 0.05f;
    
    public DirectionalRotator(Transform transform, float rotationSpeed)
    {
        _transform = transform;
        _rotationSpeed = rotationSpeed;
    }
    
    public Quaternion CurrentRotation => _transform.rotation;
    
    public void SetInputDirection(Vector3 direction) => _currentDirection = direction;

    public void Update(float deltaTime)
    {
        if (_currentDirection.magnitude < DeadZone)
            return;
        
        Quaternion lookRotation = Quaternion.LookRotation(_currentDirection);
        
        float step = _rotationSpeed * deltaTime;
        
        _transform.rotation = Quaternion.RotateTowards(_transform.rotation, lookRotation, step);
    }
}
