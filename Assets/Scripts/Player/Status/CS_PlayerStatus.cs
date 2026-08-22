using UnityEngine;

public class CS_PlayerStatus : MonoBehaviour
{
    [SerializeField]
    [Header("参照するDB")]
    private CSO_PlayerStatusDataBase _playerStatusDataBase;

    [Tooltip("プレイヤーの最大体力")]
    private float _maxHealth = 100f;
    public float maxHealth => _maxHealth;

    [Tooltip("プレイヤーの現在の体力")]
    private float _currentHealth;
    public float currentHealth => _currentHealth;

    [Tooltip("プレイヤーのレベル")]
    private int _level = 1;
    public float level => _level;

    [Tooltip("プレイヤーの経験値")]
    private float _exp = 0f;
    public float exp => _exp;

    [Tooltip("プレイヤーの次のレベルまでの経験値")]
    private float _nextLevelExp = 5;
    public float nextLevelExp => _nextLevelExp;

    private void Start()
    {
        //--- データベースから初期値を取得してステータスを設定 ---//

        // 体力
        _maxHealth = _playerStatusDataBase.initialHealth;
        _currentHealth = _playerStatusDataBase.initialHealth;

        // レベル
        _exp = 0.0f;
        _level = _playerStatusDataBase.initialLevel;
        _nextLevelExp = _playerStatusDataBase.initialNextLevelExp;
    }

    public void AddExp(float exp)
    {
        _exp += exp;
    }

    public void LevelUp(int upLevel)
    {
        if (CanLevelUp()) return;

        _level += upLevel;

        // 次のレベルまでの経験値を更新
        _nextLevelExp *= _playerStatusDataBase.expGrowthRate;


        // レベルアップ時の処理をここに追加することもできます
    }

    public bool CanLevelUp()
    {
        // 経験値が足りてない
        if (_exp < _nextLevelExp) return false;
        // レベルが最大値に達している
        if (_level >= _playerStatusDataBase.maxLevel) return false;

        return true;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth < 0)
        {
            _currentHealth = 0;
        }
    }

    public void Regenerate(float amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
    }
}
