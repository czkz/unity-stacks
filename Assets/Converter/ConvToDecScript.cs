using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvToDecScript : MonoBehaviour {

    public float stageTime = 1.5f;
    public float midStageDelay = 3.0f;
    private float stageTimeCurr;
    public float stageTimeMod = 1.0f;
    public float maxDecomp = 1e7f;

    public ConvItemContainerScript itemContainer1;
    public ConvItemContainerScript itemContainer2;
    public ConvItemContainerScript itemContainer3;
    public ConvItemContainerScript itemContainer4;

    float triggerTime;


    string whatInitial;
    Queue<char> what;
    int from;
    int currPow;

    public void ToDecimal(string what, int from) {
        if (state != State.idle) { return; }
        this.whatInitial = what;
        this.what = new Queue<char>(what.ToCharArray());
        this.from = from;
        currPow = what.Length - 1;
        state = State.stage1;
        stageTimeCurr = stageTime;
        triggerTime = Time.time;
        itemContainer1.Clear();
        itemContainer2.Clear();
        itemContainer3.Clear();
        itemContainer4.Clear();
        itemContainer1.SetSize(what.Length);
        itemContainer2.SetSize(what.Length);
        itemContainer3.SetSize(what.Length);
        itemContainer4.SetSize(1);
    }

    Stack<Transform> nums = new Stack<Transform>();

    enum State { idle, stage1, stage2, stage3, stage4 }
    State state = State.idle;

    void Update() {
        if (state == State.idle) { return; }
        if (Time.time >= triggerTime) {
            stageTimeCurr *= stageTimeMod;
            triggerTime = Time.time + stageTimeCurr;
            if (state == State.stage1) {
                SpawnDigit();
            } else if (state == State.stage2) {
                ConvertDigit1();
            } else if (state == State.stage3) {
                ConvertDigit2();
            } else if (state == State.stage4) {
                PrintDigit();
            }
        }
    }

    void SpawnDigit() {
        if (what.Count != 0) {
            char c = what.Dequeue();
            string s = c.ToString() + " * " + from.ToString() + "^" + currPow--;
            itemContainer1.PushNum(s);
        } else {
            state = from > 10 ? State.stage2 : State.stage3;
            triggerTime = triggerTime - stageTimeCurr + midStageDelay;
            stageTimeCurr = stageTime;
            what = new Queue<char>(whatInitial.ToCharArray());
            currPow = whatInitial.Length - 1;
        }
    }

    void ConvertDigit1() {
        const string digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        if (what.Count != 0) {
            char c = what.Dequeue();
            string s = digits.IndexOf(c).ToString() + " * " + from.ToString() + "^" + currPow--;
            itemContainer2.PushNum(
                s,
                new Vector3(0, itemContainer1.queueMiddlePos.position.y - itemContainer2.queueMiddlePos.position.y, 0),
                true,
                Quaternion.identity
            );
        } else {
            state = State.stage3;
            triggerTime = triggerTime - stageTimeCurr + midStageDelay;
            stageTimeCurr = stageTime;
            what = new Queue<char>(whatInitial.ToCharArray());
            currPow = whatInitial.Length - 1;
        }
    }

    void ConvertDigit2() {
        const string digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        if (what.Count != 0) {
            char c = what.Dequeue();
            ulong v = 1;
            for (int i = 0; i < currPow; i++) { v *= (ulong)from; }
            string s = digits.IndexOf(c).ToString() + " * " + (v < maxDecomp ? v.ToString() : from.ToString() + "^" + currPow);
            currPow--;
            itemContainer3.PushNum(
                s,
                new Vector3(0, itemContainer2.queueMiddlePos.position.y - itemContainer3.queueMiddlePos.position.y, 0),
                true,
                Quaternion.identity
            );
        } else {
            state = State.stage4;
            triggerTime = triggerTime - stageTimeCurr + midStageDelay;
            stageTimeCurr = stageTime;
            what = new Queue<char>(whatInitial.ToCharArray());
            currPow = whatInitial.Length - 1;
        }
    }

    void PrintDigit() {
        const string digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        Stack<char> s = new Stack<char>(whatInitial.ToCharArray());
        ulong val = 0;
        for (ulong pw = 1; s.Count != 0; pw *= (ulong)from) {
            char c = s.Pop();
            val += (ulong)digits.IndexOf(c) * pw;
        }
        for (int i = 0; i < itemContainer3.items.Count; i += 2) {
            itemContainer4.PushNum(
                val.ToString(),
                itemContainer3.items.ToArray()[i].transform.position,
                false,
                Quaternion.identity
            );
        }
        state = State.idle;
    }
}
