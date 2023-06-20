using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReinforcementsController : MonoBehaviour
{
    [SerializeField] private Animator _enemyAnimator;           //  적의의 애니메이터 컴포넌트
    void Start()
    {
        // 플레이어의 Animator 컴포넌트 가져오기
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

    //  Enemy 애니메이터 모방
    void ImitateEnemyAnimator()
    {
        if (_enemyAnimator != null)
        {
            // 플레이어의 애니메이션 값을 가져와서 현재 객체의 애니메이션에 적용
            float _isRun = _enemyAnimator.GetFloat("isRun");
            bool _doAttack = _enemyAnimator.GetBool("doAttack");

            // 현재 객체의 Animator 컴포넌트에 애니메이션 값을 설정
            Animator _animator = GetComponent<Animator>();
            _animator.SetFloat("isRun", _isRun);

            // doAttack이 true로 변경되면 트리거 설정
            if (_doAttack)
            {
                _animator.SetTrigger("doAttack");
            }
        }
    }

    // Enemy가 바라보는 방향을 보도록 회전을 설정
    void ImitateEnemyLookat()
    {
        GameObject _enemy = GameObject.FindGameObjectWithTag("Computer");
        if (_enemy != null)
        {
            Vector3 _lookDirection = _enemy.transform.forward;
            _lookDirection.y = 0f; // 수직 방향으로 회전되지 않도록 y값을 0으로 설정
            transform.rotation = Quaternion.LookRotation(_lookDirection);
        }
    }
}
