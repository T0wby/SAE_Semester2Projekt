using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GrassSpawner : MonoBehaviour
{
    #region Fields
    [SerializeField] private GameObject _grassPrefab;
    [SerializeField] private Vector2 _spawnArea = Vector2.zero;
    [SerializeField] private int _maxSpawns = 5;
    [SerializeField] private float _spawnTimer = 5f;
    private List<GameObject> _spawnedGrass;
    #endregion

    #region Properties
    public List<GameObject> SpawnedGrass { get => _spawnedGrass; } 
    #endregion

    private void Awake()
    {
        _spawnedGrass = new List<GameObject>();
        StartCoroutine(SpawnGrass());
    }

    /// <summary>
    /// Spawn a grass prefab at a random position inside of a box
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnGrass()
    {
        while (true)
        {
            CheckSpawnedGrass();
            if (_spawnedGrass.Count < _maxSpawns)
            {
                float xPos = transform.position.x + Random.Range(-_spawnArea.x * 0.5f, _spawnArea.x * 0.5f);
                float zPos = transform.position.z + Random.Range(-_spawnArea.y * 0.5f, _spawnArea.y * 0.5f);
                _spawnedGrass.Add(Instantiate(_grassPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity,this.transform));
            }

            yield return new WaitForSeconds(_spawnTimer);
        }
    }

    /// <summary>
    /// Removes all null references inside the grass list
    /// </summary>
    private void CheckSpawnedGrass()
    {
        _spawnedGrass.RemoveAll(grass => grass == null);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawCube(transform.position, new Vector3(_spawnArea.x, 1f, _spawnArea.y));
    }
}
