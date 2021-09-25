using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemySpawner : MonoBehaviour
{
    [SerializeField] private AnimationCurve spawnRate;
    private int _totalBigEnemiesSpawned;
    public Transform _enemy; //reference to the prefab that we want to instantiate
    public Transform[] spawnPoint; //array with all the possible spawn locations
    public GameObject _EnemyHolder; //enemies will spawn inside an already existing gameobject

    private void SpawnBigEnemy()
    {
        //TODO Instantiate Big enemy
        Transform _sp = spawnPoint[Random.Range(0, spawnPoint.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation, _EnemyHolder.transform);  //spawn enemy

        Debug.Log("Spawned a big enemy");
        _totalBigEnemiesSpawned++;
    }

    private int GetBigEnemiesByScore(float score) => Mathf.RoundToInt(spawnRate.Evaluate(score));
    private int GetBigEnemiesToSpawn(int wantedAmount) => wantedAmount - _totalBigEnemiesSpawned;

    public void UpdateScore(int score)
    {
        for (var i = 0; i < GetBigEnemiesToSpawn(GetBigEnemiesByScore(score)); i++) SpawnBigEnemy();
    }
}
