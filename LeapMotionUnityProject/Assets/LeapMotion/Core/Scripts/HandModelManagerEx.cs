using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;


public class HandModelManagerEx : HandModelManager
{
    // comment
    private int i = 0;
    private Hand hand,hand2;
    private Frame fm1, fm2;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("処理開始");
        
    }

    // Update is called once per frame
    void Update()
    {

        this.fm1 = leapProvider.CurrentFixedFrame;
        this.fm2 = leapProvider.CurrentFrame;


        Debug.Log("＝＝＝＝＝＝＝＝Current Fixed Frame [id] >>> " + fm1.Id + "＝＝＝＝＝＝＝＝＝＝＝");
        foreach (Hand tempHand in fm1.Hands)
        {
            Debug.Log("FrameId:" + tempHand.FrameId);
            Debug.Log("Id:" + tempHand.Id);
            Debug.Log("PalmPosition<掌の中心位置>:"
                + "x:" + tempHand.PalmPosition.x + "; "
                + "y:" + tempHand.PalmPosition.y + "; "
                + "z:" + tempHand.PalmPosition.z + "; ");
            Debug.Log("信頼度(0.0～1.0)" + tempHand.Confidence);
            Debug.Log("左手: " + tempHand.IsLeft + "  /  " + "右手: " + tempHand.IsRight);
            Debug.Log("----------区切り線--------------");
        }
        Debug.Log("＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝");

        Debug.Log("*****************Current Frame [id] >>> " + fm2.Id + "**************************");
        Debug.Log("Current Frame [id] >>>  :" + fm2.Id);
        foreach (Hand tempHand in fm2.Hands)
        {
            Debug.Log("FrameId:" + tempHand.FrameId);
            Debug.Log("Id:" + tempHand.Id);
            Debug.Log("PalmPosition<掌の中心位置>:"
                + "x:" + tempHand.PalmPosition.x + "; "
                + "y:" + tempHand.PalmPosition.y + "; "
                + "z:" + tempHand.PalmPosition.z + "; ");
            Debug.Log("信頼度(0.0～1.0)" + tempHand.Confidence);
            Debug.Log("左手: " + tempHand.IsLeft + "  /  " + "右手: " + tempHand.IsRight);
            Debug.Log("----------区切り線--------------");
        }
        Debug.Log("*********************************************************************************");






        #region テストするから避難
        //this.hand = leapProvider.CurrentFixedFrame.Hands[0];

        //foreach (Hand tempHand in leapProvider.CurrentFixedFrame.Hands){
        //    Debug.Log("FrameId:" + tempHand.FrameId);
        //    Debug.Log("Id:" + tempHand.Id);
        //    Debug.Log("PalmPosition<掌の中心位置>:" 
        //        + "x:" + tempHand.PalmPosition.x + "; "
        //        + "y:" + tempHand.PalmPosition.y + "; "
        //        + "z:" + tempHand.PalmPosition.z + "; ") ;
        //    Debug.Log("信頼度(0.0～1.0)" + tempHand.Confidence);
        //    Debug.Log("左手: " + tempHand.IsLeft + "  /  " + "右手: " + tempHand.IsRight);
        //    Debug.Log("----------区切り線--------------");
        //}
        //Debug.Log("＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝＝");
        //Debug.Log(this.hand.Fingers[(int)Finger.FingerType.TYPE_RING].Bone(Bone.BoneType.TYPE_METACARPAL).NextJoint.ToVector3().x);
        //Debug.Log(i++);
        #endregion

    }
}

