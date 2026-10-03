using UnityEngine.AI;

public class AgentJumpController : Controller
{
    private readonly Character _character;

    public AgentJumpController(Character character)
    {
        _character = character;
    }

    protected override void UpdateLogic(float deltaTime)
    {
        if (_character.IsDead || _character.InJumpProcess)
            return;

        if (_character.IsOnNavMeshLink(out OffMeshLinkData offMeshLinkData))
        {
            _character.SetRotationDirection(
                offMeshLinkData.endPos - offMeshLinkData.startPos);
            
            _character.Jump(offMeshLinkData);
        }
    }
}
