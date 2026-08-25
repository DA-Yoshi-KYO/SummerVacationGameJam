using UnityEngine;

public abstract class CS_UpgradeChipBase : MonoBehaviour
{
    // レベル0：未取得
    // レベル1～5：対応した効果を発揮
    [Tooltip("レベル")]
    protected int _level = 0;

    [Tooltip("チップ名")]
    protected string _chipName;

    [Tooltip("チップマネージャーの参照")]
    protected CS_UpgradeChipManager _chipManager;

    [Tooltip("プレイヤーの参照")]
    protected Transform _player;

    [Tooltip("プレイヤーの装備の参照")]
    protected CS_PlayerEquipment _playerEquipment;

    private void Awake()
    {
        _chipManager = GameObject.FindAnyObjectByType<CS_UpgradeChipManager>();
        
        _player = GameObject.FindAnyObjectByType<CS_PlayerStatus>().transform;

        _playerEquipment = _player.GetComponent<CS_PlayerEquipment>();
    }

    public void LevelUp()
    {
        _level++;
    }

    // チップの効果を適用する際に、同じ名前の効果がすでに適用されているかどうかを確認するメソッド
    protected bool UpgradeStatus_NameCheck(string EffectName)
    {
        string chipName = _chipName + "_" + EffectName;

        // すでに同じ名前のチップが適用されている場合は、効果を重複させないようにする
        if (_chipManager.upgradeStatus.activatedChipEffectNames.Contains(chipName)) return true;

        // 効果が適用されたことを記録するために、チップの名前を追加
        _chipManager.upgradeStatus.activatedChipEffectNames.Add(chipName);

        return false;

    }

    public void ApplyEffect()
    {
        // レベル数以下の効果を適用する
        for (int i = 1; i <= _level; i++)
        {
            switch (i)
            {
                case 1:
                    ApplyEffectLevel1();
                    break;
                case 2:
                    ApplyEffectLevel2();
                    break;
                case 3:
                    ApplyEffectLevel3();
                    break;
                case 4:
                    ApplyEffectLevel4();
                    break;
                case 5:
                    ApplyEffectLevel5();
                    break;
            }
        }
    }

    abstract protected void ApplyEffectLevel1();
    abstract protected void ApplyEffectLevel2();
    abstract protected void ApplyEffectLevel3();
    abstract protected void ApplyEffectLevel4();
    abstract protected void ApplyEffectLevel5();

}
