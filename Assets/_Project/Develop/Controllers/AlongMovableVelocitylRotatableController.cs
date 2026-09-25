public class AlongMovableVelocitylRotatableController : Controller
{
    private IVelocitySource _movable;
    private IDirectionalRotatable _rotatable;

    public AlongMovableVelocitylRotatableController(IVelocitySource movable, IDirectionalRotatable rotatable)
    {
        _movable = movable;
        _rotatable = rotatable;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        _rotatable.SetRotationDirection(_movable.CurrentVelocity);
    }
}
