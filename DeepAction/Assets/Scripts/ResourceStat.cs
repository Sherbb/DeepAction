using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[HideReferenceObjectPicker][InlineProperty]
public class ResourceStat
{
    //Used for stuff like HP. Has special scalings.
    [ProgressBar(0f,100f)][HideLabel]
    public float value;

}
