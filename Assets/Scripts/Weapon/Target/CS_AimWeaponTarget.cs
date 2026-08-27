using UnityEngine;

public class CS_AimWeaponTarget : CS_WeaponTargetBase
{
    [SerializeField]
    [Header("è∆èÄUI")]
    private CS_AimUI _aimUI;

    private void Start()
    {
        if (_aimUI == null)
            _aimUI = GameObject.FindAnyObjectByType<CS_AimUI>();
    }

    public override GameObject FindTarget()
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