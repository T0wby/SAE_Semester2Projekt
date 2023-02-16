using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    #region Fields
    private TerrainSide[] _terrainSides;
    [SerializeField, HideInInspector] private MeshFilter[] _meshFilters;

    [SerializeField, Range(2, 255)] private int _resolution = 100;
    [SerializeField] private ShapeSettings _shapeSettings;
    [SerializeField] private ColourSettings _colourSettings;
    [SerializeField] private bool _autoUpdate = true;
    [SerializeField] private bool _useMultiThreading = true;

    [HideInInspector] public bool ColourSettingsFoldout = true;
    [HideInInspector] public bool ShapeSettingsFoldout = true;

    private ShapeGeneratorTwo _shapeGenerator = new ShapeGeneratorTwo();
    private ColourGenerator _colourGenerator = new ColourGenerator();

    private List<GameObject> _meshes;

    private static Vector3[] DIRECTIONS = new Vector3[]
    {
            Vector3.forward,
            Vector3.back,
            Vector3.right,
            Vector3.left,
            Vector3.up,
            Vector3.down
    }; 
    #endregion

    #region Properties
    public ShapeSettings ShapeSettings { get => _shapeSettings; }
    public ColourSettings ColourSettings { get => _colourSettings; }
    public bool UseMultiThreading { get => _useMultiThreading; set => _useMultiThreading = value; }
    public bool AutoUpdate { get => _autoUpdate; }
    #endregion

    #region Unity
    private void Awake()
    {
        _meshes = new List<GameObject>();
    }
    #endregion

    #region Methods

    /// <summary>
    /// Create/Update all important Planet settings, variables and reset the mesh positions
    /// </summary>
    private void Initialize()
    {
        _shapeGenerator.UpdateSettings(_shapeSettings);
        _colourGenerator.UpdateSettings(_colourSettings);
        if (_meshFilters == null || _meshFilters.Length == 0)
        {
            _meshFilters = new MeshFilter[6];
        }
        _terrainSides = new TerrainSide[6];

        for (int i = 0; i < 6; i++)
        {
            if (_meshFilters[i] == null)
            {
                if(_meshes == null)
                    _meshes = new List<GameObject>();
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;
                _meshes.Add(meshObj);
                meshObj.AddComponent<MeshRenderer>();
                _meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                _meshFilters[i].sharedMesh = new Mesh();
            }
            _meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = _colourSettings.PlanetMaterial;

            _terrainSides[i] = new TerrainSide(_shapeGenerator, _meshFilters[i].sharedMesh, _resolution, DIRECTIONS[i]);
        }
        ResetPos();
    }

    /// <summary>
    /// Calling if everything needs to be changed or updated
    /// </summary>
    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColour();
    }

    /// <summary>
    /// Calling if only the Shape needs to change
    /// </summary>
    public void OnShapeSettingsUpdated()
    {
        if (!_autoUpdate)
            return;
        Initialize();
        GenerateMesh();
    }

    /// <summary>
    /// Calling if only the Colour needs to change
    /// </summary>
    public void OnColourSettingsUpdated()
    {
        if (!_autoUpdate)
            return;
        Initialize();
        GenerateColour();
    }

    /// <summary>
    /// Generate a mesh for each of the 6 sides and update the Elevation values for our Material
    /// </summary>
    private void GenerateMesh()
    {
        foreach (TerrainSide terrainSide in _terrainSides)
        {
            terrainSide.GenerateMesh(_useMultiThreading, _shapeSettings.UseFancySphere);
            StartCoroutine(CheckTerrainFaces(terrainSide));
        }
        _colourGenerator.UpdateElevation(_shapeGenerator.ElevationMinMax);
    }

    /// <summary>
    /// Update colours for our Planet and the UVs of each mesh
    /// </summary>
    private void GenerateColour()
    {
        _colourGenerator.UpdateColours();
        foreach (TerrainSide terrainSide in _terrainSides)
        {
            terrainSide.UpdateUV(_colourGenerator);
        }
    }

    /// <summary>
    /// Resets the local mesh position to Zero
    /// </summary>
    private void ResetPos()
    {
        if (_meshes == null) return;

        _meshes.RemoveAll(x => x == null);


        foreach (GameObject mesh in _meshes)
        {
            if (mesh.transform.localPosition != Vector3.zero)
            {
                mesh.transform.localPosition = Vector3.zero;
            }
        }
    }
    #endregion

    #region Enumerators
    /// <summary>
    /// Running as long as the mesh values are not set yet
    /// Prevents errors in case of the usage of MultiThreading
    /// </summary>
    /// <param name="terrainSide">Mesh to calculate</param>
    /// <returns>null</returns>
    private IEnumerator CheckTerrainFaces(TerrainSide terrainSide)
    {
        while (terrainSide.SetMeshValues())
        {
            yield return null;
        }
    }
    #endregion
}
