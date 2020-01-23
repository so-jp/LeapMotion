using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Leap;
using Leap.Unity;


public class HandModelManagerEx : HandModelManager
{
    // comment
    private GameObject go;



    #region LeapMotionで取得できる部位の座標
    // 手の部位全体の中での一意な名称
    public enum HandPartTypePosition
    {
        #region 左手
        /// <summary>
        /// 左手_掌_X座標
        /// </summary>
        LEFT_PALM_X,
        /// <summary>
        /// 左手_掌_Y座標
        /// </summary>
        LEFT_PALM_Y,
        /// <summary>
        /// 左手_掌_Z座標
        /// </summary>
        LEFT_PALM_Z,

        // =======================お試し領域 [start]
        /// <summary>
        /// 左手_掌_X軸ひねり
        /// </summary>
        LEFT_PALM_PITCH,
        /// <summary>
        /// 左手_掌_Y軸ひねり
        /// </summary>
        LEFT_PALM_YAW,
        /// <summary>
        /// 左手_掌_Z軸ひねり
        /// </summary>
        LEFT_PALM_ROLL,

        LEFT_THUMB_EXTENDED,
        LEFT_INDEX_EXTENDED,
        LEFT_MIDDLE_EXTENDED,
        LEFT_RING_EXTENDED,
        LEFT_PINKY_EXTENDED,


        // =======================お試し領域 [end]

        /// <summary>
        /// 左手_親指_X座標
        /// </summary>
        LEFT_THUMB_X,
        /// <summary>
        /// 左手_親指_Y座標
        /// </summary>
        LEFT_THUMB_Y,
        /// <summary>
        /// 左手_親指_Z座標
        /// </summary>
        LEFT_THUMB_Z,

        /// <summary>
        /// 左手_人差し指_X座標
        /// </summary>
        LEFT_INDEX_X,
        /// <summary>
        /// 左手_人差し指_Y座標
        /// </summary>
        LEFT_INDEX_Y,
        /// <summary>
        /// 左手_人差し指_Z座標
        /// </summary>
        LEFT_INDEX_Z,

        /// <summary>
        /// 左手_中指_X座標
        /// </summary>
        LEFT_MIDDLE_X,
        /// <summary>
        /// 左手_中指_Y座標
        /// </summary>
        LEFT_MIDDLE_Y,
        /// <summary>
        /// 左手_中指_Z座標
        /// </summary>
        LEFT_MIDDLE_Z,

        /// <summary>
        /// 左手_薬指_X座標
        /// </summary>
        LEFT_RING_X,
        /// <summary>
        /// 左手_薬指_Y座標
        /// </summary>
        LEFT_RING_Y,
        /// <summary>
        /// 左手_薬指_Z座標
        /// </summary>
        LEFT_RING_Z,

        /// <summary>
        /// 左手_小指_X座標
        /// </summary>
        LEFT_PINKY_X,
        /// <summary>
        /// 左手_小指_Y座標
        /// </summary>
        LEFT_PINKY_Y,
        /// <summary>
        /// 左手_小指_Z座標
        /// </summary>
        LEFT_PINKY_Z,
        #endregion

        #region 右手
        /// <summary>
        /// 右手_掌_X座標
        /// </summary>
        RIGHT_PALM_X,
        /// <summary>
        /// 右手_掌_Y座標
        /// </summary>
        RIGHT_PALM_Y,
        /// <summary>
        /// 右手_掌_Z座標
        /// </summary>
        RIGHT_PALM_Z,

        /// <summary>
        /// 右手_親指_X座標
        /// </summary>
        RIGHT_THUMB_X,
        /// <summary>
        /// 右手_親指_Y座標
        /// </summary>
        RIGHT_THUMB_Y,
        /// <summary>
        /// 右手_親指_Z座標
        /// </summary>
        RIGHT_THUMB_Z,

        /// <summary>
        /// 右手_人差し指_X座標
        /// </summary>
        RIGHT_INDEX_X,
        /// <summary>
        /// 右手_人差し指_Y座標
        /// </summary>
        RIGHT_INDEX_Y,
        /// <summary>
        /// 右手_人差し指_Z座標
        /// </summary>
        RIGHT_INDEX_Z,

        /// <summary>
        /// 右手_中指_X座標
        /// </summary>
        RIGHT_MIDDLE_X,
        /// <summary>
        /// 右手_中指_Y座標
        /// </summary>
        RIGHT_MIDDLE_Y,
        /// <summary>
        /// 右手_中指_Z座標
        /// </summary>
        RIGHT_MIDDLE_Z,

        /// <summary>
        /// 右手_薬指_X座標
        /// </summary>
        RIGHT_RING_X,
        /// <summary>
        /// 右手_薬指_Y座標
        /// </summary>
        RIGHT_RING_Y,
        /// <summary>
        /// 右手_薬指_Z座標
        /// </summary>
        RIGHT_RING_Z,

        /// <summary>
        /// 右手_小指_X座標
        /// </summary>
        RIGHT_PINKY_X,
        /// <summary>
        /// 右手_小指_Y座標
        /// </summary>
        RIGHT_PINKY_Y,
        /// <summary>
        /// 右手_小指_Z座標
        /// </summary>
        RIGHT_PINKY_Z,
        #endregion

    }
    #endregion


    #region グラフ設定関係
    /// <summary>
    /// グラフ表示下限値
    /// </summary>
    private static float DISPLAY_GRAPH_MIN = -0.5f;
    /// <summary>
    /// グラフ表示上限値
    /// </summary>
    private static float DISPLAY_GRAPH_MAX = 0.5f;
    /// <summary>
    /// グラフの線の色　5種類
    /// </summary>
    private List<Color> colorList = new List<Color>() { 
        Color.cyan, 
        Color.magenta,
        Color.yellow,
        Color.white, 
        Color.red };
    /// <summary>
    /// グラフ用の固有識別名称の命名規約で利用するprefix(エリア部分用)
    /// </summary>
    private const string PREFIX_GRAFH_ID_STRING_A = "Area_";
    /// <summary>
    /// グラフ用の固有識別名称の命名規約で利用するprefix(表示データ部分用)
    /// </summary>
    private const string PREFIX_GRAFH_ID_STRING_D = "-Data_";
    /// <summary>
    /// ラベル格納用のリスト：グラフ領域 0 - 4
    /// </summary>
    private List<HandPartTypePosition> labelList0,labelList1,labelList2,labelList3,labelList4;
    /// データ格納用のリスト：グラフ領域 0 - 4
    /// </summary>
    private List<float> valueList0,valueList1,valueList2,valueList3,valueList4;

    #endregion


    void Awake()
    {
        Debug.Log("HandModelManagerEx >>> Awake");
        this.go = GameObject.Find("Text");

        //ここの設定変えれば取得できる情報が変わる・・・予定。
        //グラフ領域 0 の初期設定
        labelList0 = new List<HandPartTypePosition>();
        labelList0.Add(HandPartTypePosition.LEFT_PALM_X);
        labelList0.Add(HandPartTypePosition.LEFT_PALM_Y);
        labelList0.Add(HandPartTypePosition.LEFT_PALM_Z);
        this.GraphInit(0,this.labelList0);

        //グラフ領域 1 の初期設定
        labelList1 = new List<HandPartTypePosition>();
        labelList1.Add(HandPartTypePosition.LEFT_THUMB_X);
        labelList1.Add(HandPartTypePosition.LEFT_INDEX_X);
        labelList1.Add(HandPartTypePosition.LEFT_MIDDLE_X);
        labelList1.Add(HandPartTypePosition.LEFT_RING_X);
        labelList1.Add(HandPartTypePosition.LEFT_PINKY_X);
        this.GraphInit(1, this.labelList1);

        //グラフ領域 2 の初期設定
        labelList2 = new List<HandPartTypePosition>();
        labelList2.Add(HandPartTypePosition.LEFT_THUMB_Y);
        labelList2.Add(HandPartTypePosition.LEFT_INDEX_Y);
        labelList2.Add(HandPartTypePosition.LEFT_MIDDLE_Y);
        labelList2.Add(HandPartTypePosition.LEFT_RING_Y);
        labelList2.Add(HandPartTypePosition.LEFT_PINKY_Y);
        this.GraphInit(2, this.labelList2);

        //グラフ領域 3 の初期設定
        labelList3 = new List<HandPartTypePosition>();
        labelList3.Add(HandPartTypePosition.LEFT_THUMB_Z);
        labelList3.Add(HandPartTypePosition.LEFT_INDEX_Z);
        labelList3.Add(HandPartTypePosition.LEFT_MIDDLE_Z);
        labelList3.Add(HandPartTypePosition.LEFT_RING_Z);
        labelList3.Add(HandPartTypePosition.LEFT_PINKY_Z);
        this.GraphInit(3, this.labelList3);

        //グラフ領域 4 の初期設定
        labelList4 = new List<HandPartTypePosition>();
        labelList4.Add(HandPartTypePosition.LEFT_THUMB_EXTENDED);
        labelList4.Add(HandPartTypePosition.LEFT_INDEX_EXTENDED);
        labelList4.Add(HandPartTypePosition.LEFT_MIDDLE_EXTENDED);
        labelList4.Add(HandPartTypePosition.LEFT_RING_EXTENDED);
        labelList4.Add(HandPartTypePosition.LEFT_PINKY_EXTENDED);
        this.GraphInit(4, this.labelList4);

    }

    void Start()
    {
        Debug.Log("HandModelManagerEx >>> Start");
        Debug.Log("Findで探した結果＞＞＞"+ this.go.GetComponent<Text>().text);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        // frame：時間取得
        Frame frame = leapProvider.CurrentFrame;   //handクラスのframeIDは常に1固定なので意味がないのでこっち
        DebugGUI.LogPersistent("frame", "frame:" + frame.Id);

        // 左右のHandを取得
        Hand leftHand = null;
        Hand rightHand = null;

        foreach(Hand hand in frame.Hands)
        {
            if (hand.IsLeft)
            {
                leftHand = hand;
                DebugGUI.LogPersistent("left hand status", "left hand status at frame:" + frame.Id);
            }
            if (hand.IsRight)
            {
                rightHand = hand;
                DebugGUI.LogPersistent("right hand status", "right hand status at frame:" + frame.Id);
            }
            if (leftHand != null && rightHand != null)
            {
                DebugGUI.LogPersistent("both hands status","both hands status at frame:"+frame.Id);
                break;
            }
        }

        valueList0 = new List<float>();
        foreach (HandPartTypePosition label in labelList0)
        {
            valueList0.Add(AnsHandPartTypePositionValue(label, leftHand, rightHand));
        
        }
        this.GraphUpdate(0, valueList0);

        valueList1 = new List<float>();
        foreach (HandPartTypePosition label in labelList1)
        {
            valueList1.Add(AnsHandPartTypePositionValue(label, leftHand, rightHand));

        }
        this.GraphUpdate(1, valueList1);

        valueList2 = new List<float>();
        foreach (HandPartTypePosition label in labelList2)
        {
            valueList2.Add(AnsHandPartTypePositionValue(label, leftHand, rightHand));

        }
        this.GraphUpdate(2, valueList2);

        valueList3 = new List<float>();
        foreach (HandPartTypePosition label in labelList3)
        {
            valueList3.Add(AnsHandPartTypePositionValue(label, leftHand, rightHand));

        }
        this.GraphUpdate(3, valueList3);

        valueList4 = new List<float>();
        foreach (HandPartTypePosition label in labelList4)
        {
            valueList4.Add(AnsHandPartTypePositionValue(label, leftHand, rightHand));

        }
        this.GraphUpdate(4, valueList4);

    }

    void OnDestroy()
    {
        // Clean up our logs and graphs when this object is destroyed
        //DebugGUI.RemoveGraph("frameRate");
        //DebugGUI.RemovePersistent("frameRate");


    }
    float AnsHandPartTypePositionValue(HandPartTypePosition type, Hand leftHand, Hand rightHand)
    {
        switch (type)
        {
            //左手
            case HandPartTypePosition.LEFT_PALM_X:
                return leftHand.PalmPosition.x;
            case HandPartTypePosition.LEFT_PALM_Y:
                return leftHand.PalmPosition.y;
            case HandPartTypePosition.LEFT_PALM_Z:
                return leftHand.PalmPosition.z;


             
            //====== otameshi start
            case HandPartTypePosition.LEFT_PALM_PITCH:
                return leftHand.PalmPosition.Pitch;
            case HandPartTypePosition.LEFT_PALM_YAW:
                return leftHand.PalmPosition.Yaw;
            case HandPartTypePosition.LEFT_PALM_ROLL:
                return leftHand.PalmPosition.Roll;

            case HandPartTypePosition.LEFT_THUMB_EXTENDED:
                return leftHand.Fingers[(int)Finger.FingerType.TYPE_THUMB].IsExtended ? DISPLAY_GRAPH_MAX : 0;
            case HandPartTypePosition.LEFT_INDEX_EXTENDED:
                return leftHand.Fingers[(int)Finger.FingerType.TYPE_INDEX].IsExtended ? DISPLAY_GRAPH_MAX : 0;
            case HandPartTypePosition.LEFT_MIDDLE_EXTENDED:
                return leftHand.Fingers[(int)Finger.FingerType.TYPE_MIDDLE].IsExtended ? DISPLAY_GRAPH_MAX : 0;
            case HandPartTypePosition.LEFT_RING_EXTENDED:
                return leftHand.Fingers[(int)Finger.FingerType.TYPE_RING].IsExtended ? DISPLAY_GRAPH_MAX : 0;
            case HandPartTypePosition.LEFT_PINKY_EXTENDED:
                return leftHand.Fingers[(int)Finger.FingerType.TYPE_PINKY].IsExtended ? DISPLAY_GRAPH_MAX : 0;

            //====== otameshi end


            case HandPartTypePosition.LEFT_THUMB_X:
                return leftHand.Fingers[(int)Finger.FingerType.TYPE_THUMB].TipPosition.x;
            case HandPartTypePosition.LEFT_THUMB_Y:
                return leftHand.Fingers[(int)Finger.FingerType.TYPE_THUMB].TipPosition.y;
            case HandPartTypePosition.LEFT_THUMB_Z:
                return leftHand.Fingers[(int)Finger.FingerType.TYPE_THUMB].TipPosition.z;

            case HandPartTypePosition.LEFT_INDEX_X:
                return leftHand.Fingers[(int)Finger.FingerType.TYPE_INDEX].TipPosition.x;
            case HandPartTypePosition.LEFT_INDEX_Y:
                return leftHand.Fingers[(int)Finger.FingerType.TYPE_INDEX].TipPosition.y;
            case HandPartTypePosition.LEFT_INDEX_Z:
                return leftHand.Fingers[(int)Finger.FingerType.TYPE_INDEX].TipPosition.z;

            case HandPartTypePosition.LEFT_MIDDLE_X:
                return leftHand.Fingers[(int)Finger.FingerType.TYPE_MIDDLE].TipPosition.x;
            case HandPartTypePosition.LEFT_MIDDLE_Y:
                return leftHand.Fingers[(int)Finger.FingerType.TYPE_MIDDLE].TipPosition.y;
            case HandPartTypePosition.LEFT_MIDDLE_Z:
                return leftHand.Fingers[(int)Finger.FingerType.TYPE_MIDDLE].TipPosition.z;

            case HandPartTypePosition.LEFT_RING_X:
                return leftHand.Fingers[(int)Finger.FingerType.TYPE_RING].TipPosition.x;
            case HandPartTypePosition.LEFT_RING_Y:
                return leftHand.Fingers[(int)Finger.FingerType.TYPE_RING].TipPosition.y;
            case HandPartTypePosition.LEFT_RING_Z:
                return leftHand.Fingers[(int)Finger.FingerType.TYPE_RING].TipPosition.z;

            case HandPartTypePosition.LEFT_PINKY_X:
                return leftHand.Fingers[(int)Finger.FingerType.TYPE_PINKY].TipPosition.x;
            case HandPartTypePosition.LEFT_PINKY_Y:
                return leftHand.Fingers[(int)Finger.FingerType.TYPE_PINKY].TipPosition.y;
            case HandPartTypePosition.LEFT_PINKY_Z:
                return leftHand.Fingers[(int)Finger.FingerType.TYPE_PINKY].TipPosition.z;

            //右手
            case HandPartTypePosition.RIGHT_PALM_X:
                return rightHand.PalmPosition.x;
            case HandPartTypePosition.RIGHT_PALM_Y:
                return rightHand.PalmPosition.y;
            case HandPartTypePosition.RIGHT_PALM_Z:
                return rightHand.PalmPosition.z;

            case HandPartTypePosition.RIGHT_THUMB_X:
                return rightHand.Fingers[(int)Finger.FingerType.TYPE_THUMB].TipPosition.x;
            case HandPartTypePosition.RIGHT_THUMB_Y:
                return rightHand.Fingers[(int)Finger.FingerType.TYPE_THUMB].TipPosition.y;
            case HandPartTypePosition.RIGHT_THUMB_Z:
                return rightHand.Fingers[(int)Finger.FingerType.TYPE_THUMB].TipPosition.z;

            case HandPartTypePosition.RIGHT_INDEX_X:
                return rightHand.Fingers[(int)Finger.FingerType.TYPE_INDEX].TipPosition.x;
            case HandPartTypePosition.RIGHT_INDEX_Y:
                return rightHand.Fingers[(int)Finger.FingerType.TYPE_INDEX].TipPosition.y;
            case HandPartTypePosition.RIGHT_INDEX_Z:
                return rightHand.Fingers[(int)Finger.FingerType.TYPE_INDEX].TipPosition.z;

            case HandPartTypePosition.RIGHT_MIDDLE_X:
                return rightHand.Fingers[(int)Finger.FingerType.TYPE_MIDDLE].TipPosition.x;
            case HandPartTypePosition.RIGHT_MIDDLE_Y:
                return rightHand.Fingers[(int)Finger.FingerType.TYPE_MIDDLE].TipPosition.y;
            case HandPartTypePosition.RIGHT_MIDDLE_Z:
                return rightHand.Fingers[(int)Finger.FingerType.TYPE_MIDDLE].TipPosition.z;

            case HandPartTypePosition.RIGHT_RING_X:
                return rightHand.Fingers[(int)Finger.FingerType.TYPE_RING].TipPosition.x;
            case HandPartTypePosition.RIGHT_RING_Y:
                return rightHand.Fingers[(int)Finger.FingerType.TYPE_RING].TipPosition.y;
            case HandPartTypePosition.RIGHT_RING_Z:
                return rightHand.Fingers[(int)Finger.FingerType.TYPE_RING].TipPosition.z;

            case HandPartTypePosition.RIGHT_PINKY_X:
                return rightHand.Fingers[(int)Finger.FingerType.TYPE_PINKY].TipPosition.x;
            case HandPartTypePosition.RIGHT_PINKY_Y:
                return rightHand.Fingers[(int)Finger.FingerType.TYPE_PINKY].TipPosition.y;
            case HandPartTypePosition.RIGHT_PINKY_Z:
                return rightHand.Fingers[(int)Finger.FingerType.TYPE_PINKY].TipPosition.z;

            default:
                return -1;
        }
    }
    /// <summary>
    /// グラフ領域の初期設定用メソッド
    /// </summary>
    /// <param name="graphArea">設定先のグラフ領域</param>
    /// <param name="nameList">設定値のラベル一覧</param>
    void GraphInit(int graphArea, List<HandPartTypePosition> labelList)
    {
        int i = 0;
        

        //グラフ領域の設定
        //DebugGUI.SetGraphProperties(固有識別名称, 画面に表示されるラベル名, グラフ下限値, グラフ上限値, 所属する表示領域グループ, 線の色, オートスケール可否)
        foreach(HandPartTypePosition label in labelList)
        {
            DebugGUI.SetGraphProperties(PREFIX_GRAFH_ID_STRING_A + graphArea + PREFIX_GRAFH_ID_STRING_D + i, label.ToString(), DISPLAY_GRAPH_MIN, DISPLAY_GRAPH_MAX, graphArea, colorList[i], false);
            i++;
        }

    }
    void GraphUpdate(int graphArea, List<float> valueList)
    {
        int i = 0;
        foreach (float value in valueList)
        {
            DebugGUI.Graph(PREFIX_GRAFH_ID_STRING_A + graphArea + PREFIX_GRAFH_ID_STRING_D + i, value);
            i++;
        }
    }



}

