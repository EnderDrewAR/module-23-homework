using UnityEngine;

public class ClickMaker : MonoBehaviour
{
    [SerializeField] private GameObject _markerPrefab;
    
    [SerializeField, Min(0f)] private float _hideDistance = 0.2f;
    [SerializeField] private float _heightOffset = 0.05f;

    private GameObject _marker;
    private Character _character;

    public void Initialize(Character character)
    {
        _character = character;

        if (_markerPrefab == null)
        {
            Debug.LogError("Assign Marker Prefab in ClickMaker.", this);
            return;
        }

        if (_marker == null)
            _marker = Instantiate(_markerPrefab, transform);

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

    private void OnDisable() => Hide();

}
