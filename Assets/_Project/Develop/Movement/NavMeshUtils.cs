using UnityEngine;
using UnityEngine.AI;

public static class NavMeshUtils
{
    public static float GetPathLength(NavMeshPath path)
    {
        Vector3[] corners = path.corners;
        float pathLength = 0f;

        for (int i = 1; i < corners.Length; i++)
            pathLength += Vector3.Distance(corners[i - 1], corners[i]);

        return pathLength;
    }

    public static bool TryGetPath(Vector3 sourcePosition, Vector3 targetPosition,
        NavMeshQueryFilter queryFilter, NavMeshPath pathToTarget)
    {
        return NavMesh.CalculatePath(sourcePosition, targetPosition, queryFilter, pathToTarget)
            && pathToTarget.status == NavMeshPathStatus.PathComplete;
    }

    public static bool TryGetPath(NavMeshAgent agent, Vector3 targetPosition,
        NavMeshPath pathToTarget)
    {
        if (agent.isActiveAndEnabled == false || agent.isOnNavMesh == false)
            return false;

        var filter = new NavMeshQueryFilter
        {
            agentTypeID = agent.agentTypeID,
            areaMask = agent.areaMask
        };

        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 0.5f, filter) == false)
            return false;

        return agent.CalculatePath(hit.position, pathToTarget)
            && pathToTarget.status == NavMeshPathStatus.PathComplete;
    }
}
