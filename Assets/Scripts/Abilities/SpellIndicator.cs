using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SpellIndicator : MonoBehaviour
{
    int levelReq = 3;

    MeshRenderer mr;
    Material mat;
    MainCamRaycast mcr;
    GlobalInfo gi;

    private void Start()
    {
        mr = GetComponent<MeshRenderer>();
        mat = mr.material;
        mcr = MainCamRaycast.Instance;
        gi = GlobalInfo.Instance;

        mr.enabled = false;
    }
    private void LateUpdate()
    {
        if (PlayerExp.Instance.level[(int)ElementalScroll.Instance.GetCurrentDamageType()] < levelReq || PlayerMana.Instance.currentMana < PlayerMana.Instance.spellCost)
        {
            mr.enabled = false;
            return;
        }

        if ((/*Input.GetButton("SpellIndicator") ||*/ Input.GetMouseButton(1)))
        {
            mr.enabled = true;
            mat.SetColor("_Color", ElementalScroll.Instance.GetCurrentSpellColor());

            Vector3 targetPosition = mcr.GetLastHit().point;
            if (Vector3.Distance(targetPosition, gi.playerTrans.position) > PlayerSpells.Instance.maxSpellRange || !mcr.aimingAtAnything)
            {
                targetPosition = gi.firePoint.position + mcr.GetRay().direction.normalized * PlayerSpells.Instance.maxSpellRange;
            }

            transform.position = targetPosition;
        }
        if (/*Input.GetButtonUp("SpellIndicator") ||*/ Input.GetMouseButtonUp(1))
        {
            mr.enabled = false;
        }
    }
}
