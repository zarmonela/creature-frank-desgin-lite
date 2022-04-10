/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-12-20 11:07:33
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;

namespace EZhex1991.EZSoftBone
{
    [CreateAssetMenu(fileName = "SBMat", menuName = "EZSoftBone/SBMaterial")]
    public class EZSoftBoneMaterial : ScriptableObject
    {
        [SerializeField, Range(0, 1)]
        private float m_Damping = 0.2f;
        public float damping { get { return m_Damping; } set { m_Damping = Mathf.Clamp01(value); } }
        [SerializeField, EZCurveRect(0, 0, 1, 1)]
        private AnimationCurve m_DampingCurve = AnimationCurve.EaseInOut(0, 0.5f, 1, 1);
        public AnimationCurve dampingCurve { get { return m_DampingCurve; } }

        [SerializeField, Range(0, 1)]
        private float m_Stiffness = 0.1f;
        public float stiffness { get { return m_Stiffness; } set { m_Stiffness = Mathf.Clamp01(value); } }
        [SerializeField, EZCurveRect(0, 0, 1, 1)]
        private AnimationCurve m_StiffnessCurve = AnimationCurve.Linear(0, 1, 1, 1);
        public AnimationCurve stiffnessCurve { get { return m_StiffnessCurve; } }

        [SerializeField, Range(0, 1)]
        private float m_Resistance = 0.9f;
        public float resistance { get { return m_Resistance; } set { m_Resistance = Mathf.Clamp01(value); } }
        [SerializeField, EZCurveRect(0, 0, 1, 1)]
        private AnimationCurve m_ResistanceCurve = AnimationCurve.Linear(0, 1, 1, 0);
        public AnimationCurve resistanceCurve { get { return m_ResistanceCurve; } }

        [SerializeField, Range(0, 1)]
        private float m_Slackness = 0.1f;
        public float slackness { get { return m_Slackness; } set { m_Slackness = Mathf.Clamp01(value); } }
        [SerializeField, EZCurveRect(0, 0, 1, 1)]
        private AnimationCurve m_SlacknessCurve = AnimationCurve.Linear(0, 1, 1, 0.8f);
        public AnimationCurve slacknessCurve { get { return m_SlacknessCurve; } }

        private static EZSoftBoneMaterial m_DefaultMaterial;
        public static EZSoftBoneMaterial defaultMaterial
        {
            get
            {
                if (m_DefaultMaterial == null)
                    m_DefaultMaterial = CreateInstance<EZSoftBoneMaterial>();
                m_DefaultMaterial.name = "SBMat_Default";
                return m_DefaultMaterial;
            }
        }


        public float GetStiffness(float t)
        {
            return stiffness * stiffnessCurve.Evaluate(t);
        }
        public float GetResistance(float t)
        {
            return resistance * resistanceCurve.Evaluate(t);
        }
        public float GetSlackness(float t)
        {
            return slackness * slacknessCurve.Evaluate(t);
        }

        public AnimationCurve frictionCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0.0f) });
        public float frictionMin = 0;
        public float frictionMax = 1;
        public float frictionValue = 0f;//OYM:摩擦力比值
        public bool isfrictionCurve = false;

        public AnimationCurve addForceScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0.0f) });
        public float addForceScaleMin = 0;
        public float addForceScaleMax = 2;
        public float addForceScaleValue = 1f;//OYM:力量比
        public bool isaddForceScaleCurve = false;


        public AnimationCurve gravityScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0.0f) });
        public float gravityScaleMin = 0;
        public float gravityScaleMax = 10;
        public float gravityScaleValue = 1f;//OYM:重力比值
        public bool isgravityScaleCurve = false;

        public AnimationCurve moveInertCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0.0f) });
        public float moveInertMin = 0;
        public float moveInertMax = 1;
        public float moveInertValue = 0f;//OYM:fixed节点传递下来的速度
        public bool ismoveInertCurve = false;

        public AnimationCurve SdampingCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0.0f) });
        public float dampingMin = 0;
        public float dampingMax = 1;
        public float dampingValue = 0.99f;//OYM:怠速
        public bool isdampingCurve = false;

        public AnimationCurve elasticityCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0.0f) });
        public float elasticityMin = 0;
        public float elasticityMax = 1;
        public float elasticityValue = 0f;//OYM:parent节点传递下来的速度
        public bool iselasticityCurve = false;

        public AnimationCurve velocityIncreaseCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0.0f) });
        public float velocityIncreaseMin = 0;
        public float velocityIncreaseMax = 10;
        public float velocityIncreaseValue = 0f;//OYM:位移距离压缩
        public bool isvelocityIncreaseCurve = false;


        public AnimationCurve stiffnessWorldCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0.0f) });
        public float stiffnessWorldMin = 0;
        public float stiffnessWorldMax = 10;
        public float stiffnessWorldValue = 0f;//OYM:世界刚性(位置刚性)
        public bool isstiffnessWorldCurve = false;

        public AnimationCurve stiffnessLocalCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0.0f) });
        public float stiffnessLocalMin = 0;
        public float stiffnessLocalMax = 1;
        public float stiffnessLocalValue;//OYM:局部刚性(角度刚性)
        public bool isstiffnessLocalCurve = false;

        public AnimationCurve lengthLimitForceScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0.0f) });
        public float lengthLimitForceScaleMin = 0;
        public float lengthLimitForceScaleMax = 1;
        public float lengthLimitForceScaleValue = 0;//OYM:父节点对子节点的拉力产生的力
        public bool islengthLimitForceScaleCurve = false;

        public AnimationCurve elasticityVelocityCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0.0f) });
        public float elasticityVelocityMin = 0;
        public float elasticityVelocityMax = 1;
        public float elasticityVelocityValue = 0;//OYM:弹性所产生的速度
        public bool iselasticityVelocityCurve = false;

        public AnimationCurve structuralShrinkVerticalScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 1.0f) });
        public float structuralShrinkVerticalScaleMin = 0;
        public float structuralShrinkVerticalScaleMax = 1;
        public float structuralShrinkVerticalScaleValue = 1.0f;
        public bool isstructuralShrinkVerticalScaleCurve = false;

        public AnimationCurve structuralStretchVerticalScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(1.0f, 1.0f) });
        public float structuralStretchVerticalScaleMin = 0;
        public float structuralStretchVerticalScaleMax = 1;
        public float structuralStretchVerticalScaleValue = 1.0f;
        public bool isstructuralStretchVerticalScaleCurve = false;

        public AnimationCurve structuralShrinkHorizontalScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(1.0f, 1.0f) });
        public float structuralShrinkHorizontalScaleMin = 0;
        public float structuralShrinkHorizontalScaleMax = 1;
        public float structuralShrinkHorizontalScaleValue = 1.0f;
        public bool isstructuralShrinkHorizontalScaleCurve = false;

        public AnimationCurve structuralStretchHorizontalScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(1.0f, 1.0f) });
        public float structuralStretchHorizontalScaleMin = 0;
        public float structuralStretchHorizontalScaleMax = 1;
        public float structuralStretchHorizontalScaleValue = 1.0f;
        public bool isstructuralStretchHorizontalScaleCurve = false;

        public AnimationCurve shearShrinkScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(1.0f, 1.0f) });
        public float shearShrinkScaleMin = 0;
        public float shearShrinkScaleMax = 1;
        public float shearShrinkScaleValue = 1.0f;
        public bool isshearShrinkScaleCurve = false;

        public AnimationCurve shearStretchScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(1.0f, 1.0f) });
        public float shearStretchScaleMin = 0;
        public float shearStretchScaleMax = 1;
        public float shearStretchScaleValue = 1.0f;
        public bool isshearStretchScaleCurve = false;

        public AnimationCurve bendingShrinkVerticalScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(1.0f, 1.0f) });
        public float bendingShrinkVerticalScaleMin = 0;
        public float bendingShrinkVerticalScaleMax = 1;
        public float bendingShrinkVerticalScaleValue = 1.0f;
        public bool isbendingShrinkVerticalScaleCurve = false;


        public AnimationCurve bendingStretchVerticalScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(1.0f, 1.0f) });
        public float bendingStretchVerticalScaleMin = 0;
        public float bendingStretchVerticalScaleMax = 1;
        public float bendingStretchVerticalScaleValue = 1.0f;
        public bool isbendingStretchVerticalScaleCurve = false;

        public AnimationCurve bendingShrinkHorizontalScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(1.0f, 1.0f) });
        public float bendingShrinkHorizontalScaleMin = 0;
        public float bendingShrinkHorizontalScaleMax = 1;
        public float bendingShrinkHorizontalScaleValue = 1.0f;
        public bool isbendingShrinkHorizontalScaleCurve = false;

        public AnimationCurve bendingStretchHorizontalScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(1.0f, 1.0f) });
        public float bendingStretchHorizontalScaleMin = 0;
        public float bendingStretchHorizontalScaleMax = 1;
        public float bendingStretchHorizontalScaleValue = 1.0f;
        public bool isbendingStretchHorizontalScaleCurve = false;

        public AnimationCurve circumferenceShrinkScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(1.0f, 1.0f) });
        public float circumferenceShrinkScaleMin = 0;
        public float circumferenceShrinkScaleMax = 1;
        public float circumferenceShrinkScaleValue = 1.0f;
        public bool iscircumferenceShrinkScaleCurve = false;

        public AnimationCurve circumferenceStretchScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(1.0f, 1.0f) });
        public float circumferenceStretchScaleMin = 0;
        public float circumferenceStretchScaleMax = 1;
        public float circumferenceStretchScaleValue = 1.0f;
        public bool iscircumferenceStretchScaleCurve = false;

        public AnimationCurve pointRadiuCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 0.0f) });
        public float pointRadiuMin = 0;
        public float pointRadiuMax = 1;
        public float pointRadiuValue = 0;
        public bool ispointRadiuCurve = false;

        //OYM:闲置值,以后会用上
        public AnimationCurve value2Curve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0f), new Keyframe(1.0f, 0f) });
        public float value2Value = 0;
        public AnimationCurve value3Curve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0f), new Keyframe(1.0f, 0f) });
        public float value3Value = 0;
        public AnimationCurve value4Curve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0f), new Keyframe(1.0f, 0f) });
        public float value4Value = 0;
        public AnimationCurve value5Curve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0f), new Keyframe(1.0f, 0f) });
        public float value5Value = 0;
        public AnimationCurve value6Curve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0f), new Keyframe(1.0f, 0f) });
        public float value6Value = 0;
        public AnimationCurve value7Curve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0f), new Keyframe(1.0f, 0f) });
        public float value7Value = 0;
        public AnimationCurve value8Curve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0f), new Keyframe(1.0f, 0f) });
        public float value8Value = 0;
        public AnimationCurve value9Curve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0f), new Keyframe(1.0f, 0f) });
        public float value9Value = 0;



        //OYM:闲置值,以后会用上

        public float structuralShrinkVertical = 1.0f;//OYM：垂直结构收缩
        public float structuralStretchVertical = 1.0f;//OYM：垂直结构拉伸
        public float structuralShrinkHorizontal = 1.0f;//OYM：水平结构收缩
        public float structuralStretchHorizontal = 1.0f;//OYM：水平结构拉伸
        public float shearShrink = 1.0f;//OYM：剪切力收缩
        public float shearStretch = 1.0f;//OYM：剪切力拉伸
        public float bendingShrinkVertical = 1.0f;//OYM：垂直弯曲应力收缩
        public float bendingStretchVertical = 1.0f;//OYM：垂直弯曲应力拉伸
        public float bendingShrinkHorizontal = 1.0f;//OYM：水平弯曲应力收缩
        public float bendingStretchHorizontal = 1.0f;//OYM：水平弯曲应力拉伸
        public float circumferenceShrink = 1.0f;
        public float circumferenceStretch = 1.0f;

        //各种设定
        public bool isComputeVirtual = true;//OYM：计算虚拟
        public bool isAllowComputeOtherConstraint = false;
        public float virtualPointAxisLength = 0.1f;
        public bool ForceLookDown = false;//OYM:强制朝下
        public bool isFixedPointFreezeRotation;//OYM:fixed节点固定旋转,用来解决一些坑爹的头发
        //质量
        public bool isAutoComputeWeight = true;//OYM：算质量
        public AnimationCurve weightCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0.0f), new Keyframe(1.0f, 10.0f) });

        public bool isComputeStructuralVertical = true;//OYM：要计算垂直
        public bool isComputeStructuralHorizontal = false;//OYM：要计算水平
        public bool isComputeShear = false;//OYM：要计算剪切
        public bool isComputeBendingVertical = false;//OYM：要计算垂直弯曲
        public bool isComputeBendingHorizontal = false;//OYM：要计算水平弯曲
        public bool isComputeCircumference = false;//OYM：计算fixPoint与point
        public bool isCollideStructuralVertical = true;
        public bool isCollideStructuralHorizontal = true;
        public bool isCollideShear = true;
        public bool isLoopRootPoints = true;//OYM：与首节点循环链接（非刚体尽量别点

        public bool isDebugDraw = true;//OYM:debug绘制,,废弃
        public bool isFixGravityAxis = true;//OYM:废弃
        public Vector3 gravity = new Vector3(0.0f, -9.81f, 0.0f);//OYM：重力(注意会跟随角色旋转而旋转)
        public ColliderChoice colliderChoice = (ColliderChoice)(1 << 10 - 1);//OYM:collider选择

        public void Deserialize(GameObject go, string keyword)
        {


        }



    }
    public enum ColliderChoice
    {
        Head = 1 << 0,
        UpperBody = 1 << 1,
        LowerBody = 1 << 2,
        UpperLeg = 1 << 3,
        LowerLeg = 1 << 4,
        UpperArm = 1 << 5,
        LowerArm = 1 << 6,
        Hand = 1 << 7,
        Foot = 1 << 8,
        Other = 1 << 9,
    }
}

