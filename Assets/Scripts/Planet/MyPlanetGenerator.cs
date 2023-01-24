using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

public class MyPlanetGenerator : MonoBehaviour
{
    #region Fields
    private TerrainFace[] terrainFaces;
    private MeshFilter[] terrainFilters;

    [SerializeField] private ShapeSettings shapeSettings;
    [SerializeField] private ColourSettings colourSettings;
    public ShapeSettings ShapeSettings => shapeSettings;
    [HideInInspector] public bool ShapeSettingsFoldout;

    private ShapeGenerator shapeGenerator = new ShapeGenerator();
    private ColourGenerator colourGenerator = new ColourGenerator();

    private static Vector3[] DIRECTIONS = new Vector3[]
    {
            Vector3.forward,
            Vector3.back,
            Vector3.right,
            Vector3.left,
            Vector3.up,
            Vector3.down
    };

    [SerializeField] private bool mAutoUpdatePlanet;

    [SerializeField] private Vector3 mPosition;
    [SerializeField] private Vector3 mRotation;
    [SerializeField] private Vector3 mScale;

    private Material mMeshMat;
    [SerializeField, Range(2, 255)] private int mResolution;
    #endregion

    #region Unity
    private void Awake()
    {
        mMeshMat = colourSettings.PlanetMaterial;
    }
    #endregion

    #region Methods
    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColour();
    }

    private void Initialize()
    {
        shapeGenerator.UpdateShapeSettings(shapeSettings, mPosition, mRotation, mScale);

        colourGenerator.UpdateSettings(colourSettings);

        terrainFaces = new TerrainFace[6];

        if (terrainFilters == null || terrainFilters.Length != 6)
            terrainFilters = new MeshFilter[6];

        GameObject newFaceObj;
        for (int i = 0; i < terrainFaces.Length; i++)
        {
            if (terrainFilters[i] == null)
            {
                newFaceObj = new GameObject($"TerrainFace_{i}");
                newFaceObj.transform.SetParent(this.transform);

                MeshRenderer newRenderer = newFaceObj.AddComponent<MeshRenderer>();
                newRenderer.sharedMaterial = mMeshMat;
                terrainFilters[i] = newFaceObj.AddComponent<MeshFilter>();

                Mesh newFaceMesh = new Mesh();
                newFaceMesh.name = $"TerrainFace_{i}";

                terrainFilters[i].sharedMesh = newFaceMesh;
            }

            terrainFaces[i] = new TerrainFace(terrainFilters[i].sharedMesh, shapeGenerator, mResolution, DIRECTIONS[i], shapeSettings.UseFancySphere);
        }
    }

    private void GenerateMesh()
    {
        for (int i = 0; i < terrainFaces.Length; i++)
        {
            terrainFaces[i].GenerateMesh(GameManager.Instance.UseMultiThreading);
            StartCoroutine(CheckTerrainFaces(i));
        }
        colourGenerator.UpdateElevation(shapeGenerator.ElevationMinMax);
    }

    private void GenerateColour()
    {
        colourGenerator.UpdateColours();
    }

    public void OnBaseInfoUpdate()
    {
        if (mAutoUpdatePlanet)
        {
            GeneratePlanet();
        }
    }

    public void OnShapeSettingsUpdate()
    {
        if (mAutoUpdatePlanet)
        {
            GeneratePlanet();
        }
    }
    #endregion


    #region Enumerators
    private IEnumerator CheckTerrainFaces(int value)
    {
        while (terrainFaces[value].SetMeshValues())
        {
            yield return new WaitForEndOfFrame();
        }
    } 
    #endregion
}
