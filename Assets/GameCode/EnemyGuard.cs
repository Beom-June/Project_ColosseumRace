using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyGuard : MonoBehaviour
{
    [SerializeField] private float _flyAwaySpeed = 10.0f;           //  날라가는 속도
    [SerializeField] private Vector3 _flyVector;                    //  날라가는 방향벡터
    [SerializeField] private int _guardLevel;                       //  해당 Enemy Level
    [SerializeField] private Text _txtguardLevel;                   //  해당 Enemy Level Text
    private Animator _enemyAnimator;
    private Rigidbody _enemyRigidbody;
    [SerializeField] GameManager _gameManager;

    #region Property
    public int guardLevel => _guardLevel;

    #endregion
    void Start()
    {
        _enemyAnimator = GetComponent<Animator>();
        _enemyRigidbody = GetComponent<Rigidbody>();
    }

        // EnemyGuard의 레벨 텍스트를 업데이트함
    private void UpdateGuardLevelText()
    {
        _txtguardLevel.text = _guardLevel.ToString();
    }

    // EnemyGuard의 레벨 텍스트 색상을 변경함
    public void SetGuardLevelColor(Color color)
    {
        _txtguardLevel.color = color;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Zone") && _gameManager.redCount >= _guardLevel)
        {
            FlyAway();
        }
    }

    private void FlyAway()
    {
        Debug.Log("옴?");
        _enemyRigidbody.useGravity = false;
        _enemyRigidbody.isKinematic = false;
        Vector3 fly = _flyVector * _flyAwaySpeed * Time.deltaTime;
        _enemyRigidbody.velocity = fly;
    }
}
