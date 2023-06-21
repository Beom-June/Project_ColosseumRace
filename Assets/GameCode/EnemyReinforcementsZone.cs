using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReinforcementsZone : MonoBehaviour
{
    [Header("Reinforcements Settings")]
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _reinforcements;                            
    [SerializeField] private int _spawnedReinforcementsCount;         
    [SerializeField] private List<GameObject> _spawnedReinforcements; // 생성된 애들을 저장할 리스트             

    [Header("Camp Settings")]
    [SerializeField] private float _radius = 3f;
    [SerializeField] private float _angleStep = 72f;                

    public int spawnedReinforcementsCount
    {
        get { return _spawnedReinforcementsCount; }
        set { _spawnedReinforcementsCount = value; } 
    }

    public List<GameObject> spawnedReinforcements
    {
                get { return _spawnedReinforcements; }
        set { _spawnedReinforcements = value; } 
    }

    void Update()
    {
        if (_target != null)
        {
            transform.position = _target.transform.position;
        }
    }

    private void SpawnReinforcements()
    {
        float _angle = _angleStep;
        Quaternion _rotation = Quaternion.Euler(0f, _angle, 0f);

        _spawnedReinforcementsCount++;

        Vector3 _offset = Quaternion.Euler(0f, _angle, 0f) * (Vector3.forward * _radius);
        Vector3 _spawnPosition = transform.position + _offset;

        GameObject reinforcements = Instantiate(_reinforcements, _spawnPosition, _rotation);
        reinforcements.transform.SetParent(transform);

        _spawnedReinforcements.Add(reinforcements); // 생성된 애들을 리스트에 추가

        _angleStep += _angleStep;
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Blue"))
        {
            SpawnReinforcements();
            Destroy(collider.gameObject);
        }
    }
}
