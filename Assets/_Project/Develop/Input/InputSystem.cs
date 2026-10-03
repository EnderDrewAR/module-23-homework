using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(ClickMaker))]
public class InputSystem : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _groundMask = ~0;
        
    // Pickup Health
    [SerializeField] private HealthPickup _healthPickupPrefab;
    [SerializeField] private Collider _pickupCollector;
    [SerializeField, Min(0.1f)] private float _spawnInterval = 3f;
    [SerializeField, Min(0f)] private float _spawnRadius = 5f;
    
    // Audio
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioSettingsUI _audioSettingsUI;

    private ClickMaker _clickMaker;
    private HealthPickupSpawner _healthPickupSpawner;
    private Controller _characterController;
    private AudioHandler _audioHandler;

    private void Awake()
    {
        ClickMaker clickMaker = GetComponent<ClickMaker>();
        clickMaker.Initialize(_character);
        
        _characterController = new CompositeController(
            new PlayerClickMovableController(_character, _camera, _groundMask),
            new AlongMovableVelocitylRotatableController(_character, _character),
            new AgentJumpController(_character)
            );
        
        _characterController.Enable();

        _healthPickupSpawner = new HealthPickupSpawner(
            this, 
            _character, 
            _pickupCollector,
            _healthPickupPrefab,
            _spawnInterval,
            _spawnRadius
        );
        
        // Audio
        _audioHandler = new AudioHandler(_audioMixer);
        _audioHandler = new AudioHandler(_audioMixer);
        _audioSettingsUI.Initialize(_audioHandler);
    }

    private void Update()
    {
        _characterController.Update(Time.deltaTime);
        
        if (Input.GetKeyDown(KeyCode.F))
            _healthPickupSpawner.Toggle();
    }

    private void OnDisable()
    {
        if (_healthPickupSpawner != null)
            _healthPickupSpawner.StopSpawning();
    }
        
}

