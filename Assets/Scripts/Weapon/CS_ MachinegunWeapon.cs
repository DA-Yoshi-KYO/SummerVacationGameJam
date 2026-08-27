using UnityEngine;

public class CS_MachinegunWeapon : CS_BaseWeapon
{
    public override void Start()
    {
        weaponName = "Machinegun";
        base.Start();
    }

    protected override void Shot()
    {
        //ƒv[ƒ‹‚©‚ç’e‚ğæ“¾‚µ‚Ä”­Ë
        CS_BaseBullet bullet = base.ActivateBullet();

        //•W“I‚ğİ’è
        GameObject targetEnemy = _weaponTarget.FindTarget();

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
}
