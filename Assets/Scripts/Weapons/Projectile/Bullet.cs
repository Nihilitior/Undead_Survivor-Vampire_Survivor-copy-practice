using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 _dir = new Vector3(1, 0, 0);
    
    public Vector3 BulletDir { set { _dir = value; } }

    public int _damage;
    public float _movSpeed;
    public int _penetrate;
    public float _force;
    private int piercing = 0;
    private float _lifeTime = 0.3f;
    private float _createTime = 0f;
    private void OnEnable()
    {
        _createTime = Managers.GameTime;
    }

    void FixedUpdate()
    {
        if (Managers.GameTime - _createTime > _lifeTime)
        {
            Managers.Resource.Destroy(gameObject);
        }
        OnMove();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject go = col.gameObject;
        if (go.CompareTag("Enemy"))
        {
            go.GetComponent<BaseController>().OnDamaged(_damage, _force);
            piercing += 1;
            if(piercing >= _penetrate)
                Managers.Resource.Destroy(gameObject);
        }
    }
    
    void OnMove()
    {
        float angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0,0,angle-90);
        transform.position += _dir * (_movSpeed * Time.fixedDeltaTime);
    }
}
