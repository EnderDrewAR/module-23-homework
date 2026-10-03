using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _healAmount  = 25f;

    private Character _character;
    private bool _collected;
    private Collider _pickupCollector;

    public void Initialize(Character character, Collider pickupCollector)
    {
        _character = character;
        _pickupCollector = pickupCollector;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (_collected)
            return;
        
        if (other != _pickupCollector)
            return;
        
        if (_character.IsDead || _character.CurrentHealth >= _character.MaxHealth)
            return;
        
        _collected = true;
        _character.Heal(_healAmount);
        Destroy(gameObject);
    }
}
