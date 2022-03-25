using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvItemContainerScript : MonoBehaviour {

    public Transform queueMiddlePos;
    public Transform spawnPos;
    public float itemSize = 1;
    public ConvItemScript itemPref;
    public ConvItemScript plusPref;

    public Vector3 queueStartPos { get; private set; }

    void Start() {
        queueStartPos = queueMiddlePos.position;
    }

    protected Stack<ConvItemScript> itemsInternal = new Stack<ConvItemScript>();
    public Stack<ConvItemScript> items { get { return itemsInternal; } }

    public void SetSize(int size) {
        Vector3 pos = queueMiddlePos.position;
        pos.x -= (float)(size - 1) * itemSize;
        queueStartPos = pos;
    }

    public void PushNum(string val) {
        PushNum(val, spawnPos.position, false, Quaternion.identity);
    }

    public virtual void PushNum(string val, Vector3 spawnAt, bool isOffset, Quaternion rot) {
        if (itemsInternal.Count != 0) {
            PushAny(plusPref, "+", spawnAt, isOffset, rot);
        }
        PushAny(itemPref, val, spawnAt, isOffset, rot);
    }

    public void Clear() {
        while (itemsInternal.Count != 0) {
            Destroy(itemsInternal.Pop().gameObject);
        }
    }

    public string Last() {
        return itemsInternal.Peek().value;
    }

    protected virtual void PushAny(ConvItemScript pref, string val, Vector3 spawnAt, bool isOffset, Quaternion rot) {
        Vector3 pos = queueStartPos;
        pos.x += itemsInternal.Count * itemSize;
        ConvItemScript newItem = ConvItemScript.Instantiate(pref, isOffset ? pos + spawnAt : spawnAt, spawnPos.rotation);
        newItem.Init(pos, rot, val);
        itemsInternal.Push(newItem);
    }
}
