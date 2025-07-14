using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private WaveData[] waveData;

    [SerializeField] private Hero hero;

    [SerializeField] private Wall wall;

    [SerializeField] private EnemiesController enemiesController;

    private int currentWave = 0;

    [ContextMenu("Start Game")]
    public void StartGame()
    {
        hero.Initialize();
        wall.Initialize();

        currentWave = 0;

        Invoke(nameof(StartWave), 1);
    }

    private void StartWave()
    {
        StartCoroutine(StartSpawn());
    }

    private IEnumerator StartSpawn()
    {
        for (int j = 0; j < waveData.Length; j++)
        {
            WaveData wave = waveData[currentWave];

            for (int i = 0; i < wave.enemies.Length; i++)
            {
                yield return new WaitForSeconds(2);
                enemiesController.SpawnEnemy("Slime");
            }

            yield return new WaitForSeconds(10);
        }
    }
}