using UnityEngine;

public interface IDirectionalRotatable : ITransformPosition
{
    public Quaternion CurrentRotation { get; }
    
    public void SetRotationDirection(Vector3 inputDirection);
}
