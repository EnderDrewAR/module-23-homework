using UnityEngine;

public class MineView : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explosionEffectPrefab;
    [SerializeField] private AudioSource _audioSource;

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
        _audioSource.Play();
        foreach (MeshRenderer renderer in _renderers)
            renderer.enabled = false;

        if (_explosionEffectPrefab != null)
        {
            ParticleSystem explosionEffect = Instantiate(
                _explosionEffectPrefab,
                transform.position,
                Quaternion.identity);
            ParticleSystem.MainModule main = explosionEffect.main;
            main.loop = false;
            main.stopAction = ParticleSystemStopAction.Destroy;
            explosionEffect.Play();
        }

        enabled = false;
    }
}
