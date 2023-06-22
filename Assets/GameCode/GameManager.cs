using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameManager : MonoBehaviour
{

    [Header("Level Check")]
    [SerializeField] private ReinforcementsZone _reinforcementsZone;
    [SerializeField] private EnemyReinforcementsZone _enemyReinforcementsZone;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private int _spawnRedCount;
    [SerializeField] private int _spawnBlueCount;
    [SerializeField] private List<EnemyGuard> _redEnemyGuards;
    [SerializeField] private List<EnemyGuard> _blueEnemyGuards;

    [Header("Event")]
    [SerializeField] private GameObject _uIRetry;

    #region  Property
    public int redCount => _spawnRedCount;
    #endregion

    private void Update()
    {
        IncreaseSpawnedReinforcementsCount();
        IncreaseSpawnedEnemyReinforcementsCount();
        CheckRedGuardLevel();
        CheckBlueGuardLevel();

        // _spawnRedCount가 0일 때 _uIRetry 활성화
        if (_spawnRedCount < 0)
        {
            _uIRetry.SetActive(true);
        }
        else if (_spawnRedCount >= 0)
        {
            _uIRetry.SetActive(false);
        }
    }

    // Player -> 지원군 생성 시 호출하여 생성된 개수를 증가시킴
    public void IncreaseSpawnedReinforcementsCount()
    {
        _spawnRedCount = _reinforcementsZone.spawnedReinforcementsCount;
        _uiManager.UpdateRedLevelText(_spawnRedCount);
    }

    // Enemy -> 지원군 생성 시 호출하여 생성된 개수를 증가시킴
    public void IncreaseSpawnedEnemyReinforcementsCount()
    {
        _spawnBlueCount = _enemyReinforcementsZone.spawnedReinforcementsCount;
        _uiManager.UpdateBlueLevelText(_spawnBlueCount);
    }

    // RedEnemyGuard의 레벨과 생성된 지원군의 개수를 비교하여 텍스트 색상을 변경함
    public void CheckRedGuardLevel()
    {
        foreach (EnemyGuard enemyGuard in _redEnemyGuards)
        {
            if (_reinforcementsZone.spawnedReinforcementsCount >= enemyGuard.guardLevel)
            {
                enemyGuard.SetGuardLevelColor(Color.white);
            }
            else if (_reinforcementsZone.spawnedReinforcementsCount < enemyGuard.guardLevel)
            {
                enemyGuard.SetGuardLevelColor(Color.red);
            }
        }
    }
    // BlueEnemyGuard의 레벨과 생성된 지원군의 개수를 비교하여 텍스트 색상을 변경함
    public void CheckBlueGuardLevel()
    {
        foreach (EnemyGuard enemyGuard in _blueEnemyGuards)
        {
            if (_enemyReinforcementsZone.spawnedReinforcementsCount >= enemyGuard.guardLevel)
            {
                enemyGuard.SetGuardLevelColor(Color.white);
            }
            else if (_enemyReinforcementsZone.spawnedReinforcementsCount < enemyGuard.guardLevel)
            {
                enemyGuard.SetGuardLevelColor(Color.blue);
            }
        }
    }
}
