using UnityEngine;

public interface IDirectionalMovable : IVelocitySource 
{

    
    void SetMoveDirection(Vector3 inputDirection);
}

