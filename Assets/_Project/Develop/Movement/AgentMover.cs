using UnityEngine;
using UnityEngine.AI;

public class AgentMover
{
    private readonly NavMeshAgent _agent;

    public Vector3 CurrentVelocity => _agent.velocity;

    public AgentMover(NavMeshAgent agent, float movementSpeed)
    {
        _agent = agent;
        _agent.speed = movementSpeed;
        _agent.acceleration = 999f;
    }

    public void SetDestination(Vector3 position)
    {
        if (_agent.isActiveAndEnabled && _agent.isOnNavMesh)
            _agent.SetDestination(position);
    }

    public bool SetPath(NavMeshPath path)
    {
        return _agent.isActiveAndEnabled && _agent.isOnNavMesh
            && _agent.SetPath(path);
    }

    public void Stop()
    {
        if (_agent.isActiveAndEnabled && _agent.isOnNavMesh)
            _agent.isStopped = true;
    }

    public void Resume()
    {
        if (_agent.isActiveAndEnabled && _agent.isOnNavMesh)
            _agent.isStopped = false;
    }
}
