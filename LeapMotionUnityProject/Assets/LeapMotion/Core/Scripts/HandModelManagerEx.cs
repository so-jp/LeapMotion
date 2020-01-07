using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;


public class HandModelManagerEx : HandModelManager
{
    // comment
    private int i = 0;
    private Hand hand;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("処理開始");
        
    }

    // Update is called once per frame
    void Update()
    {
        hand = leapProvider.CurrentFixedFrame.Hands[0];
        Debug.Log(this.hand.Fingers[(int)Finger.FingerType.TYPE_RING].Bone(Bone.BoneType.TYPE_METACARPAL).NextJoint.ToVector3().x);
        //Debug.Log(i++);
    }
}

