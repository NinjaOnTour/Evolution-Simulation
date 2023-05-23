using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public GameObject CellPrefab;
    public GameObject FoodPrefab;
    public List<Cell> Cells;
    public int PopulationCapacity = 1000;

    [Header("Cell Constants")] // Proportionality constants of some variables in cell
    [Tooltip("EnergySpending = (Size + TemperatureRange + MovingForce * C_MoveEnergy) * C_EnergySpending")] 
    public float C_EnergySpending = 1.0f / 2.0f;
    [Tooltip("EnergyCapacity = Size * C_EnergyCapacity")] 
    public float C_EnergyCapacity = 1.0f / 1.6f;
    [Tooltip("Speed = MovingForce / Size * C_Speed")] 
    public float C_Speed = 1.0f;
    [Tooltip("EnergySpending = (Size + TemperatureRange + MovingForce * C_MoveEnergy) * C_EnergySpending")] 
    public float C_MoveEnergy = 0.1f;
    [Tooltip("SplitCost = ChildEnergy + Size * C_EnergyOfSize")] 
    public float C_EnergyOfSize = 10.0f;

    [Header("Mutation Constants")]
    public float M_MinTemp = 1f;
    public float M_MaxTemp = 1f;
    public float M_Size = 0.1f;
    public float M_SplitEnergy = 12f;
    public float M_ChildEnergy = 10f;
    public float M_MovingForce = 0.1f;
    public float M_MovingAngle = 2.5f;
    public int M_MutationCount = 1;
    public float M_GeneticStability = 1.5f;

    public static Enviroment GlobalEnviroment;

    public bool isPopulationCapacityFull
    {
        get
        {
            return Cells.Count >= PopulationCapacity;
        }
    }

    protected new void Awake()
    {
        base.Awake();
        GlobalEnviroment = GameObject.FindGameObjectWithTag("Global Enviroment").GetComponent<Enviroment>();
    }

    
}