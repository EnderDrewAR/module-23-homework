using UnityEngine;

public class MineView : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosionEffect;

    private Mine _mine;
    private MeshRenderer[] _renderers;
    private MaterialPropertyBlock _properties;

    private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

    private void Awake()
    {
        _mine = GetComponentInParent<Mine>();
        _renderers = GetComponentsInChildren<MeshRenderer>();
        _properties = new MaterialPropertyBlock();
    }

    private void Update()
    {
        if (_mine.HasExploded)
        {
            ShowExplosion();
            return;
        }

        if (_mine.IsActivated)
            ShowActivation();
    }

    private void ShowActivation()
    {
        Color color = Color.Lerp(Color.red, Color.yellow,
            Mathf.PingPong(Time.time * 6f, 1f));

        foreach (MeshRenderer renderer in _renderers)
        {
            renderer.GetPropertyBlock(_properties);
            _properties.SetColor(BaseColor, color);
            renderer.SetPropertyBlock(_properties);
        }
    }

    private void ShowExplosion()
    {
        foreach (MeshRenderer renderer in _renderers)
            renderer.enabled = false;

        if (_explosionEffect != null)
            _explosionEffect.Play();

        enabled = false;
    }
}
