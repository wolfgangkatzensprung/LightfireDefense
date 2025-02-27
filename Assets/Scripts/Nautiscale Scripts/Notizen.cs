using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notizen : MonoBehaviour
{
    [TextArea(minLines:1, maxLines:25)] public string description;
}
