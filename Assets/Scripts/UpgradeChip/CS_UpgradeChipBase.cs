using UnityEngine;

public abstract class CS_UpgradeChipBase : MonoBehaviour
{
    // レベル0：未取得
    // レベル1～5：対応した効果を発揮
    [Tooltip("レベル")]
    protected int _level = 0;

    [Tooltip("チップマネージャーの参照")]
    protected CS_UpgradeChipManager _chipManager;

    [Tooltip("プレイヤーの参照")]
    protected Transform _player;

    private void Awake()
    {
        _chipManager = GameObject.FindAnyObjectByType<CS_UpgradeChipManager>();
        
        _player = GameObject.FindAnyObjectByType<CS_PlayerStatus>().transform;
    }

    public void LevelUp()
    {
        _level++;
    }

    public void ApplyEffect()
    {
        switch (_level)
        {
            case 1: ApplyEffectLevel1(); break;
            case 2: ApplyEffectLevel2(); break;
            case 3: ApplyEffectLevel3(); break;
            case 4: ApplyEffectLevel4(); break;
            case 5: ApplyEffectLevel5(); break;
            default: break;
        }
    }

    abstract protected void ApplyEffectLevel1();
    abstract protected void ApplyEffectLevel2();
    abstract protected void ApplyEffectLevel3();
    abstract protected void ApplyEffectLevel4();
    abstract protected void ApplyEffectLevel5();

}
