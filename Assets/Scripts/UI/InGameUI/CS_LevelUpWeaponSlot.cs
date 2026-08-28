using UnityEngine;
using UnityEngine.EventSystems;

public class CS_LevelUpWeaponSlot : MonoBehaviour, IDropHandler
{
    [Header("ÉXÉçÉbÉgî‘çÜ")]public int slotIndex;

    public void OnDrop(PointerEventData eventData)
    {
        var selectUI = eventData.pointerDrag.GetComponent<CS_SelectUISet>();
        if (selectUI == null) return;

        //îΩâfÇ∑ÇÈ
        CS_LevelUpManager.Instance.SetWeapon(slotIndex, selectUI);
    }
}