using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvFakeContainer : ConvItemContainerScript {
    public override void PushNum(string val, Vector3 spawnAt, bool isOffset, Quaternion rot) {
        PushAny(itemPref, val, spawnAt, isOffset, rot);
    }

    protected override void PushAny(ConvItemScript pref, string val, Vector3 spawnAt, bool isOffset, Quaternion rot) {
        Vector3 pos = queueStartPos;
        ConvItemScript newItem = ConvItemScript.Instantiate(pref, isOffset ? pos + spawnAt : spawnAt, spawnPos.rotation);
        newItem.Init(pos, rot, val);
        itemsInternal.Push(newItem);
    }
}
