using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReinforcementsController : MonoBehaviour
{
    [SerializeField] private Animator _enemyAnimator;          
    void Start()
    {
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

    //  Enemy, 부모의 애니메이션 받아옴
    void ImitateEnemyAnimator()
    {
        if (_enemyAnimator != null)
        {
            float _isRun = _enemyAnimator.GetFloat("isRun");
            bool _doAttack = _enemyAnimator.GetBool("doAttack");
            bool _doKnockedDown = _enemyAnimator.GetBool("isKnockedDown");

            Animator _animator = GetComponent<Animator>();
            _animator.SetFloat("isRun", _isRun);

            if (_doAttack)
            {
                _animator.SetTrigger("doAttack");
            }

            if(_doKnockedDown)
            {
                _animator.SetTrigger("isKnockedDown");
            }
        }
    }

    
    void ImitateEnemyLookat()
    {
        GameObject _enemy = GameObject.FindGameObjectWithTag("Computer");
        if (_enemy != null)
        {
            Vector3 _lookDirection = _enemy.transform.forward;
            _lookDirection.y = 0f; 
            transform.rotation = Quaternion.LookRotation(_lookDirection);
        }
    }
}
