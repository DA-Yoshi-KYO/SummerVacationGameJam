using System.Collections.Generic;
using UnityEngine;

public class CS_ShotgunWeapon : CS_BaseWeapon
{
    [Header("Æ€UI")]
    private CS_AimUI _aimUI;

    [SerializeField]
    [Header("Æ€‚Ìd‚İ")]
    private float _angleWeight;

    [SerializeField]
    [Header("ƒyƒŒƒbƒg”")]
    private int _bulletCount;

    [SerializeField]
    [Header("ƒVƒ‡ƒbƒgƒKƒ“‚Ì’e‚ÌŠgU—¦(”’l‚ª‚‚¢‚Ù‚ÇL‚­ŠgU‚·‚é)")]
    private float _spreadRate;

    public override void Start()
    {
        weaponName = "Shotgun";

        base.Start();

        _aimUI = GameObject.FindAnyObjectByType<CS_AimUI>();
    }

    protected override void Shot()
    {
        //ƒv[ƒ‹‚©‚ç’e‚ğæ“¾‚µ‚Ä”­Ë
        List<CS_BaseBullet> bulletList = new List<CS_BaseBullet>();

        for (int i = 0; i < _bulletCount; i++)
        {
            bulletList.Add(base.ActivateBullet());
        }
        

        //•W“I‚ğİ’è
        GameObject targetEnemy = FindTargetWithAim();

        Vector3 baseDirection = transform.forward;

        if (targetEnemy != null)
        {
            // “G‚Ì•ûŒüƒw‚ÌƒxƒNƒgƒ‹‚ğŒvZ
            baseDirection = (targetEnemy.transform.position - transform.position).normalized;
        }

        // ’e‚ğ”­Ë‚·‚é
        foreach (var bullet in bulletList)
        {
            // ƒ‰ƒ“ƒ_ƒ€‚ÈŠgUŠp“x‚ğŒvZ
            bullet.transform.position = baseDirection;

            float randomAngleX = Random.Range(-_spreadRate, _spreadRate);
            float randomAngleY = Random.Range(-_spreadRate, _spreadRate);

            Quaternion spreadRotation = Quaternion.Euler(randomAngleX, randomAngleY, 0);

            // ŠgU•ûŒü‚ğŒvZ
            Vector3 spreadDirection = spreadRotation * baseDirection;
            bullet.Activate(firePoint.position, Quaternion.LookRotation(spreadDirection));
        }
    }

    //Æ€•ûŒü‚Ìˆê”Ô‹ß‚¢“G‚ğ‘_‚¤ˆ—
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

            float score = dist + angle * _angleWeight;

            if (score < lockonScore)
            {
                lockonScore = score;
                lockonGameObject = enemy;
            }
        }

        return lockonGameObject;
    }

}
