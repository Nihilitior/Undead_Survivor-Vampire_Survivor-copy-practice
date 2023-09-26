using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class KnifeController : WeaponController
{
    public override int _weaponType { get { return (int)Define.Weapons.Knife; } }

    private float _termKnifeThrow = 0.1f;
    private bool _isCool = false;


    void Update()
    {
        if (!_isCool)
        {
            StartCoroutine(SpawnWeapon());
        }
    }


    IEnumerator StartKnifeCoolTime()
    {

        yield return new WaitForSeconds(_cooldown);
        _isCool = false;
    }

     
    IEnumerator SpawnWeapon()
    {
        _isCool = true;
        for (int i = 0; i < _countPerCreate; i++)
        {
            Managers.Sound.Play("Shoot_01");
            GameObject go = Managers.Game.Spawn(Define.WorldObject.Weapon, "Weapon/Knife");
            go.transform.position = transform.position;
            SetWeapon(go);
            if (i == _countPerCreate-1)
                break;
            yield return new WaitForSeconds(_termKnifeThrow);
        }

        StartCoroutine(StartKnifeCoolTime());
    }

    protected void SetWeapon(GameObject weapon)
    {
        Knife knife = weapon.GetOrAddComponent<Knife>();
        //Create Knife to ranmdom range position
        knife.transform.position += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);

        Vector2 dirOfPlayer = _player.GetComponent<PlayerController>()._lastDirVec;

        knife._dir = new Vector3(dirOfPlayer.x, dirOfPlayer.y, 0);

        knife._damage = _damage;
        knife._speed = _movSpeed;
        knife._force = _force;
    }
}