using UnityEngine;
using UnityEngine.AI;

public class PlayerClickMovableController : Controller
{
    private readonly Character _character;
    private readonly Camera _camera;
    private readonly LayerMask _groundMask;
    private readonly ClickMaker _clickMaker;
    private readonly NavMeshPath _pathToTarget = new NavMeshPath();

    public PlayerClickMovableController(Character character, Camera camera, LayerMask groundMask,
        ClickMaker clickMaker)
    {
        _character = character;
        _camera = camera;
        _groundMask = groundMask;
        _clickMaker = clickMaker;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity,
                _groundMask, QueryTriggerInteraction.Ignore))
            return;

        if (!_character.TryGetPath(hit.point, _pathToTarget))
            return;

        // Apply the validated path without calculating it again.
        if (_character.SetPath(_pathToTarget))
        {
            _character.ResumeMove();
            Vector3[] corners = _pathToTarget.corners;
            _clickMaker.Show(corners[corners.Length - 1]);
        }
    }

    public override void Disable()
    {
        base.Disable();
        _character.StopMove();
        _clickMaker.Hide();
    }
}
