using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class GameManager : MonoBehaviour
{

    [Header("Level Check")]
    [SerializeField] private ReinforcementsZone _reinforcementsZone;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private int _spawnRedCount;

    private void Update()
    {
        IncreaseSpawnedReinforcementsCount();
    }

    // 지원군 생성 시 호출하여 생성된 개수를 증가시킴
    public void IncreaseSpawnedReinforcementsCount()
    {
         _spawnRedCount = _reinforcementsZone.spawnedReinforcementsCount;
        _uiManager.UpdateRedLevelText(_spawnRedCount);
        Debug.Log(_spawnRedCount);
    }
}
