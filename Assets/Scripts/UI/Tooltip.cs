using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI tooltipText;
    public RectTransform backgroundRectTransform;

    internal void HideTooltipOnMenuToggle()
    {
        if (!UIManager.Instance.inMenu)
            HideTooltip();
    }

    public void ShowTooltip(GameObject go)
    {
        //Debug.Log($"ShowTooltip for {go.name}");

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        TooltipTextSwitch(go);
    }

    private void TooltipTextSwitch(GameObject go)
    {
        if (go.TryGetComponent(out TooltipTarget tt))
        {
            tooltipText.text = tt.tooltipText;
            return;
        }

        string toolTipString = " ";
        string name = go.name;

        if (name.Contains("Turret"))
        {
            toolTipString = "Buy a Turret. \n" +
                "Maximum amount of turrets is 15.";
        }
        //else if (name.Contains("Player EXP"))
        //{
        //    toolTipString = "Player Level";
        //}
        else if (name.Contains("Tooltip Earth"))
        {
            toolTipString = $"Left Click: Earth Shot\n" +
                $"Damage/Heal: {PlayerExp.Instance.level[(int)Damage.DamageType.Earth]}\n" +
                $"Right Click: AoE Spell (unlocked at Level 3)\n" +
                $"Spell Effect: Root";
        }
        else if (name.Contains("Tooltip Fire"))
        {
            toolTipString = $"Left Click: Flamethrower\n" +
                $"Damage: {PlayerExp.Instance.level[(int)Damage.DamageType.Fire]}\n" +
                $"Right Click: AoE Spell (unlocked at Level 3)\n" +
                $"Spell Effect: Burn";
        }
        else if (name.Contains("Tooltip Water"))
        {
            toolTipString = $"Left Click: Water Shot\n" +
                $"Damage: {PlayerExp.Instance.level[(int)Damage.DamageType.Water]}\n" +
                $"Right Click: AoE Spell (unlocked at Level 3)\n" +
                $"Spell Effect: Floatiness";
        }
        else if (name.Contains("Tooltip Air"))
        {
            toolTipString = $"Left Click: Air Beam\n" +
                $"Damage: {PlayerExp.Instance.level[(int)Damage.DamageType.Air]}\n" +
                $"Right Click: AoE Spell (unlocked at Level 3)\n" +
                $"Spell Effect: Upwind";
        }
        else if (name.Contains("PlayerHealth"))
        {
            toolTipString = "Upgrade maximum Health Points";
        }   
        else if (name.Contains("PlayerMana"))
        {
            toolTipString = "Upgrade maximum Mana";
        }
        else if (name.Contains("LighthouseHp"))
        {
            toolTipString = "Upgrade Lightfire Beacon Hit Points";
        }
        else if (name.Contains("LighthouseDmg"))
        {
            toolTipString = "Upgrade Damage of Lightfire Beacon's defensive Laser Beam";
        } 
        else if (name.Contains("LighthouseRange"))
        {
            toolTipString = "Upgrade Lightfire Beacon Height and Zone Radius";
        }   
        else if (name.Contains("Controls Button"))
        {
            toolTipString = "View Controls";
        }    
        else if (name.Contains("Save Game Button"))
        {
            toolTipString = "Save Game";
        }     
        else if (name.Contains("Music Slider"))
        {
            toolTipString = "Music Volume";
        }
        else if (name.Contains("Sound Slider"))
        {
            toolTipString = "Sound Volume";
        }   
        else if (name.Contains("Controls"))
        {
            toolTipString = "Show Controls";
        }
        //else if (name.Contains("SelectedElement"))
        //{
        //    toolTipString = "This shows the currently selected element in this order: Earth, Fire, Water, Air.";
        //}   
        //else if (name.Contains("WavePanel"))
        //{
        //    toolTipString = "This panel shows your Lightfire Beacon's TP as well as the number of the current wave you need to defend. Below it you can see how many enemies are still remaining.";
        //}
        tooltipText.text = toolTipString;
    }

    public void HideTooltip()
    {
        //Debug.Log("HideTooltip");
        gameObject.SetActive(false);
    }
}
