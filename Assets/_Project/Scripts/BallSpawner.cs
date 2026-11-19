using FishNet.Object;
using UnityEngine;

public class BallSpawner : NetworkBehaviour
{
    [SerializeField] private NetworkObject _prefabToSpawn;

    public override void OnStartServer()
    {
        base.OnStartServer();

        NetworkObject spawnedObject = Instantiate(_prefabToSpawn, transform.position, transform.rotation);
        ServerManager.Spawn(spawnedObject);
    }
}