using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class AnyKeyAddRndText : MonoBehaviour
{
    TextMeshProUGUI rndText;

    string[] randomStrings =
    {
        "(⌐■_■)",
        "(T_T)",
        "<(^-^)>",
        "<(^_^)>",
        "<.<",
        ">.>",
        "UwU",
        "rdy",
        "ready",
        "pls",
        "please",
        "ok?",
        "ok",
        "okay",
        "hmm",
        "hmmm",
        "mhmm",
        "haha",
        "haha!",
        "lol",
        "omg",
        //":)",
        //":D",
        //";)",
        //":-)",
        //";-)",
        "O_O",
        "o_o",
        "^o^",
        "^.^",
        "^_^",
        "^-^",
        "^^",
        "easy",
        "ez",
        "easily",
        "with ease",
        "simple",
        "or don't",
        "except Alt+F4",
        "with glory",
        "with vengeance",
        "with your forehead",
        "with your nose",
        "with your pinky",
        "with your toe",
        "with your toes",
        "with your elbow",
        "with your knee",
        "take your time",
        "when you feel like it",
        "when the time is right",
        "when the time has come",
        "whenever the time has come",
        "whenever the time is right",
        "if you dare",
        "if you so desire",
        "if it's meant to be",
        "if you want",
        "if you please",
        "if you will",
        "rn!",
        "now!",
        "right now!",
        "go!",
        "GO!",
        "hurry!",
        "quick!",
        "ogogog",
        "before it's too late!",
        "<i>any</i> button?",
        "not <i>any</i> button...",
        "press press",
        "peacefully",
        "emotionally",
        "gently",
        "softly",
        "warmly",
        "yay",
        "yay!",
        "yaay!!",
        "well.",
    };

    bool sameIndex = true;

    private void OnEnable()
    {
        rndText = GetComponent<TextMeshProUGUI>();
        rndText.text = TryGetRandomText();
    }

    private string TryGetRandomText()
    {
        if (UIManager.Instance.inIntro)
            return "";

        sameIndex = true;

        while (sameIndex)
        {
            int randomIndex = Random.Range(0, randomStrings.Length);
            if (randomIndex != PlayerPrefs.GetInt("RandomStringIndex"))
            {
                sameIndex = false;
                PlayerPrefs.SetInt("RandomStringIndex", randomIndex);
                return randomStrings[randomIndex];
            }
        }

        return "WTF";
    }
}
