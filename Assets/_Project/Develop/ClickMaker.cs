using UnityEngine;

public class ClickMaker : MonoBehaviour
{
    [SerializeField] private GameObject _marker;
    [SerializeField] private Character _character;
    [SerializeField, Min(0f)] private float _hideDistance = 0.2f;
    [SerializeField] private float _heightOffset = 0.05f;

    private void Awake()
    {
        Hide();
    }

    public void Initialize(Character character)
    {
        _character = character;

        if (_marker == null)
            CreateMarker();

        Hide();
    }

    private void Update()
    {
        if (_marker == null || _character == null)
            return;

        if (_character.IsDead || _character.HasDestination == false)
        {
            Hide();
            return;
        }

        Vector3 destination = _character.Destination;
        Vector3 distance = destination - _character.Position;
        distance.y = 0f;
        float hideDistance = Mathf.Max(_hideDistance, _character.StoppingDistance + 0.05f);

        if (distance.sqrMagnitude <= hideDistance * hideDistance)
        {
            Hide();
            return;
        }

        _marker.transform.position = destination + Vector3.up * _heightOffset;
        if (_marker.activeSelf == false)
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
