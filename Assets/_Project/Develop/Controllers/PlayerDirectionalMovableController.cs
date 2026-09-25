using UnityEngine;

public class PlayerDirectionalMovableController : Controller
{
    private IDirectionalMovable _movable;
    
    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";

    public PlayerDirectionalMovableController(IDirectionalMovable movable)
    {
        _movable = movable;
    }
    
    protected override void UpdateLogic(float deltaTime)
    {
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw(HorizontalAxis), 0f, Input.GetAxisRaw(VerticalAxis));
        
        _movable.SetMoveDirection(inputDirection);
    }
}