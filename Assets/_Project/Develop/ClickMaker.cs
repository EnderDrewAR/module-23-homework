using UnityEngine;

public class ClickMaker : MonoBehaviour
{
    [SerializeField] private GameObject _marker;
    [SerializeField] private Character _character;
    [SerializeField, Min(0f)] private float _hideDistance = 0.2f;
    [SerializeField] private float _heightOffset = 0.05f;

    private Vector3 _destination;
    private UnityEngine.AI.NavMeshAgent _agent;

    private void Awake()
    {
        Hide();
    }

    public void Initialize(Character character)
    {
        _character = character;
        _agent = character.GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (_marker == null)
            CreateMarker();

        Hide();
    }

    private void Update()
    {
        if (_marker == null || !_marker.activeSelf || _character == null)
            return;

        if (_character.IsDead)
        {
            Hide();
            return;
        }

        Vector3 distance = _destination - _character.Position;
        distance.y = 0f;
        float hideDistance = _agent == null
            ? _hideDistance
            : Mathf.Max(_hideDistance, _agent.stoppingDistance + 0.05f);

        if (distance.sqrMagnitude <= hideDistance * hideDistance)
            Hide();
    }

    public void Show(Vector3 destination)
    {
        _destination = destination;
        _marker.transform.position = destination + Vector3.up * _heightOffset;
        _marker.SetActive(true);
    }

    public void Hide()
    {
        if (_marker != null)
            _marker.SetActive(false);
    }

    private void CreateMarker()
    {
        _marker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        _marker.name = "DestinationMarker";
        _marker.SetActive(false);
        _marker.transform.SetParent(transform, false);
        _marker.transform.localScale = new Vector3(0.5f, 0.02f, 0.5f);

        Collider markerCollider = _marker.GetComponent<Collider>();
        markerCollider.enabled = false;
        Destroy(markerCollider);

        Renderer markerRenderer = _marker.GetComponent<Renderer>();
        markerRenderer.sharedMaterial = Resources.Load<Material>("DestinationMarker");
        markerRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        markerRenderer.receiveShadows = false;
    }

    private void OnDisable() => Hide();

}
