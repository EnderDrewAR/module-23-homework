using UnityEngine;

public class InputSystem : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _groundMask = ~0;
    [SerializeField] private ClickMaker _clickMaker;
    
    private Controller _characterController;

    private void Awake()
    {
        if (_clickMaker == null)
        {
            GameObject markerSystem = new GameObject("ClickMarkerSystem");
            markerSystem.transform.SetParent(transform, false);
            _clickMaker = markerSystem.AddComponent<ClickMaker>();
        }

        _clickMaker.Initialize(_character);

        _characterController = new CompositeController(
            new PlayerClickMovableController(_character, _camera, _groundMask),
            new AlongMovableVelocitylRotatableController(_character, _character)
            );
        
        _characterController.Enable();
    }

    private void Update()
    {
        _characterController.Update(Time.deltaTime);
    }
}

