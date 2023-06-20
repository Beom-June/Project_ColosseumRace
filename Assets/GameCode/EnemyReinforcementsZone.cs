using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReinforcementsZone : MonoBehaviour
{
    [Header("Reinforcements Settings")]
    [SerializeField] private GameObject _target;
    [SerializeField] private GameObject _reinforcements;                             //  ������ ������
    [SerializeField] private int _spawnedReinforcementsCount;                        // ������ ������ ����

    [Header("Camp Settings")]
    [SerializeField] private float _radius = 3f;
    [SerializeField] private float _angleStep = 72f;                //  ���� ������

    public int spawnedReinforcementsCount
    {
        get { return _spawnedReinforcementsCount; }
        set { value = _spawnedReinforcementsCount; }
    }

    void Update()
    {
        if (_target != null)
        {
            // _target�� ��ġ�� ���󰡵��� �ش� ������Ʈ�� ��ġ�� ������Ʈ
            transform.position = _target.transform.position;
        }
    }

    private void SpawnReinforcements()
    {
        // �ð� �������� ������ ���� ���
        float _angle = _angleStep;
        Quaternion _rotation = Quaternion.Euler(0f, _angle, 0f);

        // ������ ������ ���� ����
        _spawnedReinforcementsCount++;

        // �÷��̾� �ֺ��� ������ ��ġ ���
        Vector3 _offset = Quaternion.Euler(0f, _angle, 0f) * (Vector3.forward * _radius);
        Vector3 _spawnPosition = transform.position + _offset;

        // ������ ���� �� �÷��̾��� �ڽ����� ����
        GameObject reinforcements = Instantiate(_reinforcements, _spawnPosition, _rotation);
        reinforcements.transform.SetParent(transform);

        // _angle ���� ���� (��ġ �����ϱ� ���ؼ�)
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
