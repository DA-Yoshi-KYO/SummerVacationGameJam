using System.Collections.Generic;
using UnityEngine;

public class CS_ShotgunWeapon : CS_BaseWeapon
{
    [Header("Aim用UI")]
    private CS_AimUI _aimUI;

    [SerializeField]
    [Header("ペレット数")]
    private int _bulletCount;

    [SerializeField]
    [Header("拡散率(小さいほど直進しやすい)")]
    private float _spreadRate;

    public override void Start()
    {
        weaponName = "Shotgun";

        base.Start();

        _aimUI = GameObject.FindAnyObjectByType<CS_AimUI>();
    }

    protected override void Shot()
    {
        //�v�[������e���擾���Ĕ���
        List<CS_BaseBullet> bulletList = new List<CS_BaseBullet>();

        for (int i = 0; i < _bulletCount; i++)
        {
            bulletList.Add(base.ActivateBullet());
        }


        //�W�I��ݒ�
        GameObject targetEnemy = FindTargetWithAim();

        Vector3 baseDirection = transform.forward;

        if (targetEnemy != null)
        {
            // �G�̕����w�̃x�N�g�����v�Z
            baseDirection = (targetEnemy.transform.position - transform.position).normalized;
        }

        // �e�𔭎˂���
        foreach (var bullet in bulletList)
        {
            // �����_���Ȋg�U�p�x���v�Z
            bullet.transform.position = baseDirection;

            float randomAngleX = Random.Range(-_spreadRate, _spreadRate);
            float randomAngleY = Random.Range(-_spreadRate, _spreadRate);

            Quaternion spreadRotation = Quaternion.Euler(randomAngleX, randomAngleY, 0);

            // �g�U�������v�Z
            Vector3 spreadDirection = spreadRotation * baseDirection;
            bullet.Activate(firePoint.position, Quaternion.LookRotation(spreadDirection));

            // �e�̎�ނɉ������ݒ�
            if (bullet is CS_SimpleBullet)
            {
                CS_SimpleBullet simpleBullet = bullet as CS_SimpleBullet;
                simpleBullet.SetRange(weaponData.range);
            }
        }
    }

    //�Ə������̈�ԋ߂��G��_������
    public GameObject FindTargetWithAim()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        GameObject lockonGameObject = null;
        float lockonScore = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);

            Vector3 dirToEnemy = (enemy.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToEnemy);

            float score = dist + angle;

            if (score < lockonScore)
            {
                lockonScore = score;
                lockonGameObject = enemy;
            }
        }

        return lockonGameObject;
    }
}
