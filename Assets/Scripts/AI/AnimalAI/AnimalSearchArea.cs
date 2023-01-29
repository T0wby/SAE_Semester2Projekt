using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(SphereCollider))]
public class AnimalSearchArea : MonoBehaviour
{
    #region Fields
    [SerializeField] private AnimalAISettings _settings;
    private SphereCollider _sphereCollider;
    private List<Grass> _grassInRange;
    private List<AAnimal> _animalInRange;
    private List<GameObject> _waterInRange;

    #endregion

    #region Properties
    public List<Grass> GrassInRange { get => _grassInRange; }
    public List<AAnimal> AnimalInRange { get => _animalInRange; }
    public List<GameObject> WaterInRange { get => _waterInRange; }

    #endregion

    #region Unity
    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.radius = _settings.SearchRange;
        _grassInRange = new List<Grass>();
        _animalInRange = new List<AAnimal>();
        _waterInRange = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Grass>())
        {
            AddRemoveGrass(other.GetComponent<Grass>(), _grassInRange, true);
        }
        else if (other.GetComponent<AAnimal>())
        {
            AddRemoveAnimal(other.GetComponent<AAnimal>(), _animalInRange, true);
        }
        else if (other.CompareTag("Water"))
        {
            AddRemoveWater(other.gameObject, _waterInRange, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Grass>())
        {
            AddRemoveGrass(other.GetComponent<Grass>(), _grassInRange, false);
        }
        else if (other.GetComponent<AAnimal>())
        {
            AddRemoveAnimal(other.GetComponent<AAnimal>(), _animalInRange, false);
        }
        else if (other.CompareTag("Water"))
        {
            AddRemoveWater(other.gameObject, _waterInRange, false);
        }
    }
    #endregion

    #region Methods
    private void AddRemoveGrass(Grass grass, List<Grass> grassList, bool add)
    {
        if (add)
        {
            grassList.Add(grass);
        }
        else
        {
            grassList.Remove(grass);
        }
    }

    private void AddRemoveWater(GameObject water, List<GameObject> waterList, bool add)
    {
        if (add)
        {
            waterList.Add(water);
        }
        else
        {
            waterList.Remove(water);
        }
    }

    private void AddRemoveAnimal(AAnimal animal, List<AAnimal> animalList, bool add)
    {
        if (add)
        {
            animalList.Add(animal);
        }
        else
        {
            animalList.Remove(animal);
        }
    } 
    #endregion
}
