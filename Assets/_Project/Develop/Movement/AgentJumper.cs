using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentJumper
{
    private float _speed;
    private NavMeshAgent _agent;
    private AnimationCurve _yOffsetCurve;

    private MonoBehaviour _coroutineRunner;
    
    private Coroutine _jumpProcess;

    public AgentJumper(float speed, AnimationCurve yOffsetCurve, MonoBehaviour coroutineRunner, NavMeshAgent agent)
    {
        _speed = speed;
        _yOffsetCurve = yOffsetCurve;
        _coroutineRunner = coroutineRunner;
        _agent = agent;
    }
    
    public bool InProcess => _jumpProcess != null;
    public bool IsPaused { get; set; }
    
    public void Jump(OffMeshLinkData offMeshLinkData)
    {
        if (InProcess)
            return;

        _jumpProcess = _coroutineRunner.StartCoroutine(JumpProcess(offMeshLinkData));
    }

    private IEnumerator JumpProcess(OffMeshLinkData offMeshLinkData)
    {
        Vector3 startPosition = offMeshLinkData.startPos;
        Vector3 endPosition = offMeshLinkData.endPos;
        
        float duration = Vector3.Distance(startPosition, endPosition) /  _speed;
        
        float progress = 0f;

        while (progress < duration)
        {
            if (IsPaused)
            {
                yield return null;
                continue;
            }

            float yOffset = _yOffsetCurve.Evaluate(progress / duration);
            _agent.transform.position = Vector3.Lerp(startPosition, endPosition, progress / duration) + Vector3.up * yOffset;
            progress += Time.deltaTime ;
            yield return null;
        }
        _agent.CompleteOffMeshLink();
        _jumpProcess = null;
    }

    public void Cancel()
    {
        if (_jumpProcess != null)
            _coroutineRunner.StopCoroutine(_jumpProcess);

        _jumpProcess = null;
        IsPaused = false;
    }
}
