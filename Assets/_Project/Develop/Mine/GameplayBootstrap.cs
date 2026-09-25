using UnityEngine;

public class GameplayBootstrap : MonoBehaviour
{
    [SerializeField] private Character _character;

    private void Start()
    {
        foreach (GameObject root in gameObject.scene.GetRootGameObjects())
            foreach (Mine mine in root.GetComponentsInChildren<Mine>(true))
                mine.Initialize(_character);
    }
}
