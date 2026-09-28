using UnityEngine;
using UnityEngine.AI;

public class PlayerClickMovableController : Controller
{
    private readonly Character _character;
    private readonly Camera _camera;
    private readonly LayerMask _groundMask;
    private readonly NavMeshPath _pathToTarget = new NavMeshPath();

    public PlayerClickMovableController(Character character, Camera camera, LayerMask groundMask)
    {
        _character = character;
        _camera = camera;
        _groundMask = groundMask;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        if (Input.GetMouseButtonDown(0) == false)
            return;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity,
                _groundMask, QueryTriggerInteraction.Ignore) == false)
            return;

        if (_character.TryGetPath(hit.point, _pathToTarget) == false)
            return;

        // Apply the validated path without calculating it again.
        if (_character.SetPath(_pathToTarget))
            _character.ResumeMove();
    }

    public override void Disable()
    {
        base.Disable();
        _character.StopMove();
    }
}
