using UnityEngine;

public class CS_SniperRifleWeapon : CS_BaseWeapon
{
    [Header("Æ€UI")]
    private CS_AimUI _aimUI;

    public override void Start()
    {
        weaponName = "SniperRifle";
        base.Start();
        _aimUI = GameObject.FindAnyObjectByType<CS_AimUI>();
    }

    protected override void Shot()
    {
        //ƒv[ƒ‹‚©‚ç’e‚ğæ“¾‚µ‚Ä”­Ë
        CS_BaseBullet bullet = base.ActivateBullet();

        //•W“I‚ğİ’è
        GameObject targetEnemy = FindTargetWithAim();

        if (targetEnemy != null)
        {
            // “G‚Ì•ûŒüƒw‚ÌƒxƒNƒgƒ‹‚ğŒvZ
            Vector3 directionToEnemy = (targetEnemy.transform.position - transform.position).normalized;

            bullet.Activate(firePoint.position, Quaternion.LookRotation(directionToEnemy));
        }
        else
        {
            // “G‚ªŒ©‚Â‚©‚ç‚È‚¢ê‡‚ÍA’Êí‚Ì”­Ë•ûŒü‚Å’e‚ğ”­Ë
            bullet.Activate(firePoint.position, firePoint.rotation);
        }

        // ’e‚Ìí—Ş‚É‰‚¶‚½İ’è
        if (bullet is CS_SimpleBullet)
        {
            CS_SimpleBullet simpleBullet = bullet as CS_SimpleBullet;
            simpleBullet.SetRange(weaponData.range);
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
