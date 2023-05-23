using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    public float Temperature;
    public float PH;
    public float LeftBorder;
    public float RightBorder;
    public float UpBorder;
    public float DownBorder;
    public int StartFoodCount;
    public float MinFoodValue;
    public float MaxFoodValue;
    public float FoodSpawnPeriod;

    private void Start()
    {
        for (int i = 0; i < StartFoodCount; i++)
        {
            SpawnFood();
        }

        //InvokeRepeating("SpawnFood", FoodSpawnPeriod, FoodSpawnPeriod);
        StartCoroutine(FoodSpawner());
    }

    IEnumerator FoodSpawner()
    {
        yield return new WaitForSeconds(FoodSpawnPeriod);
        SpawnFood();
        StartCoroutine(FoodSpawner());
    }

    void SpawnFood()
    {
        Food food = Instantiate(GameManager.instance.FoodPrefab, new Vector3(Random.Range(LeftBorder, RightBorder), Random.Range(DownBorder, UpBorder), 0f), Quaternion.identity).GetComponent<Food>();
        food.Energy = Random.Range(MinFoodValue, MaxFoodValue);
    }
}
