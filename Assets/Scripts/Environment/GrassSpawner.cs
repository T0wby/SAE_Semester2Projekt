using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GrassSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _grassPrefab;
    [SerializeField] private Vector2 _spawnArea = Vector2.zero;
    [SerializeField] private int _maxSpawns = 5;
    [SerializeField] private float _spawnTimer = 5f;
    private List<GameObject> _spawnedGrass;

    public List<GameObject> SpawnedGrass { get => _spawnedGrass; }

    private void Awake()
    {
        _spawnedGrass = new List<GameObject>();
        StartCoroutine(SpawnGrass());
    }

    private IEnumerator SpawnGrass()
    {
        while (true)
        {
            if (_spawnedGrass.Count < _maxSpawns)
            {
                float xPos = transform.position.x + Random.Range(-_spawnArea.x * 0.5f, _spawnArea.x * 0.5f);
                float zPos = transform.position.z + Random.Range(-_spawnArea.y * 0.5f, _spawnArea.y * 0.5f);
                _spawnedGrass.Add(Instantiate(_grassPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity,this.transform));
            }

            yield return new WaitForSeconds(_spawnTimer);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawCube(transform.position, new Vector3(_spawnArea.x, 1f, _spawnArea.y));
    }
}
