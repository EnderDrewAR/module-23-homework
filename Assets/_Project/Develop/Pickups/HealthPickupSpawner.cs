using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HealthPickupSpawner 
{
    private readonly MonoBehaviour _coroutineRunner;
    private readonly Character _character;
    private readonly Collider _pickupCollector;
    private readonly HealthPickup _prefab;
    private readonly float _interval;
    private readonly float _radius;
    
    // Spawn position 
    private const int SpawnAttempts = 10;
    private const float NavMeshSearchDistance = 1f;
    private const float HeightOffset = 1f;
    
    private Coroutine _spawnCoroutine;

    public HealthPickupSpawner(MonoBehaviour coroutineRunner, 
        Character character, 
        Collider pickupCollector, 
        HealthPickup prefab, 
        float interval,
        float radius)
    {
        _coroutineRunner = coroutineRunner;
        _character = character;
        _pickupCollector = pickupCollector;
        _prefab = prefab;
        _interval = interval;
        _radius = radius;
    }

    public void Toggle()
    {
        if (_spawnCoroutine == null)
            StartSpawning();
        else
            StopSpawning();
    }
    
    public void StartSpawning()
    {
        if (_spawnCoroutine != null)
            return;

        _spawnCoroutine = _coroutineRunner.StartCoroutine(SpawnProcess());
    }
    
    public void StopSpawning()
    {
        if (_spawnCoroutine == null)
            return;

        _coroutineRunner.StopCoroutine(_spawnCoroutine);
        _spawnCoroutine = null;
    }

    private IEnumerator SpawnProcess()
    {
        while (true)
        {
            yield return new WaitForSeconds(_interval);
            
            if (_character == null || _character.IsDead)
                continue;

            Spawn();
        }
    }

    private void Spawn()
    {
        for (int i = 0; i < SpawnAttempts; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);

            Vector3 direction = new Vector3(
                Mathf.Cos(angle),
                0f,
                Mathf.Sin(angle)
            );

            Vector3 candidate =
                _character.transform.position + direction * _radius;

            bool found = NavMesh.SamplePosition(
                candidate,
                out NavMeshHit hit,
                NavMeshSearchDistance,
                NavMesh.AllAreas
            );

            if (found == false)
                continue;

            Vector3 position = hit.position + Vector3.up * HeightOffset;

            HealthPickup pickup = Object.Instantiate(
                _prefab,
                position,
                Quaternion.identity
            );

            pickup.Initialize(_character, _pickupCollector);
            return;
        }
    }
}
