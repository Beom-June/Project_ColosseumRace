using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReinforcementsController : MonoBehaviour
{
    [SerializeField] private Animator _enemyAnimator;           //  ������ �ִϸ����� ������Ʈ
    void Start()
    {
        // �÷��̾��� Animator ������Ʈ ��������
        GameObject _enemy = GameObject.FindGameObjectWithTag("Computer");
        if (_enemy != null)
        {
            _enemyAnimator = _enemy.GetComponent<Animator>();
        }
    }

    void Update()
    {
        ImitateEnemyAnimator();
        ImitateEnemyLookat();
    }

    //  Enemy �ִϸ����� ���
    void ImitateEnemyAnimator()
    {
        if (_enemyAnimator != null)
        {
            // �÷��̾��� �ִϸ��̼� ���� �����ͼ� ���� ��ü�� �ִϸ��̼ǿ� ����
            float _isRun = _enemyAnimator.GetFloat("isRun");
            bool _doAttack = _enemyAnimator.GetBool("doAttack");

            // ���� ��ü�� Animator ������Ʈ�� �ִϸ��̼� ���� ����
            Animator _animator = GetComponent<Animator>();
            _animator.SetFloat("isRun", _isRun);

            // doAttack�� true�� ����Ǹ� Ʈ���� ����
            if (_doAttack)
            {
                _animator.SetTrigger("doAttack");
            }
        }
    }

    // Enemy�� �ٶ󺸴� ������ ������ ȸ���� ����
    void ImitateEnemyLookat()
    {
        GameObject _enemy = GameObject.FindGameObjectWithTag("Computer");
        if (_enemy != null)
        {
            Vector3 _lookDirection = _enemy.transform.forward;
            _lookDirection.y = 0f; // ���� �������� ȸ������ �ʵ��� y���� 0���� ����
            transform.rotation = Quaternion.LookRotation(_lookDirection);
        }
    }
}
