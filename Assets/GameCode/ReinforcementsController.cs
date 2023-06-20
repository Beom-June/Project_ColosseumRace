using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforcementsController : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;           //  플레이어의 애니메이터 컴포넌트
    void Start()
    {
        // 플레이어의 Animator 컴포넌트 가져오기
        GameObject _player = GameObject.FindGameObjectWithTag("Player");
        if (_player != null)
        {
            _playerAnimator = _player.GetComponent<Animator>();
        }
    }

    void Update()
    {
        ImitatePlayerAnimator();
        ImitatePlayerLookat();
    }

    //  플레이어 애니메이터 모방
    void ImitatePlayerAnimator()
    {
        if (_playerAnimator != null)
        {
            // 플레이어의 애니메이션 값을 가져와서 현재 객체의 애니메이션에 적용
            float _isRun = _playerAnimator.GetFloat("isRun");
            bool _doAttack = _playerAnimator.GetBool("doAttack");

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

    // 플레이어가 바라보는 방향을 보도록 회전을 설정
    void ImitatePlayerLookat()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Debug.Log(player != null);
            Vector3 _lookDirection = player.transform.forward;
            _lookDirection.y = 0f; // 수직 방향으로 회전되지 않도록 y값을 0으로 설정
            transform.rotation = Quaternion.LookRotation(_lookDirection);
        }
    }
}
