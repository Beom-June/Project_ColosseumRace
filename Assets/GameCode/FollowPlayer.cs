using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _target;

    void Update()
    {
               if (_target != null)
        {
            // _target의 위치를 따라가도록 해당 오브젝트의 위치를 업데이트
            transform.position = _target.transform.position;
        }
    }
}
