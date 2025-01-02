using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Custom
{
    //If you use UnityEngine.Keyframe, they won't be saved.
    // Reference : https://github.com/Unity-Technologies/UnityCsReference/blob/master/Runtime/Export/Animation/AnimationCurve.bindings.cs
    [Serializable]
    public struct Keyframe
    {
        [SerializeField]
        private float m_Time;
        [SerializeField]
        private float m_Value;
        [SerializeField]
        private float m_InTangent;
        [SerializeField]
        private float m_OutTangent;
        [SerializeField]
        private int m_TangentMode;
        [SerializeField]
        private int m_WeightedMode;
        [SerializeField]
        private float m_InWeight;
        [SerializeField]
        private float m_OutWeight;

        //
        // 요약:
        //     The time of the keyframe.
        public float time
        {
            get
            {
                return m_Time;
            }
            set
            {
                m_Time = value;
            }
        }

        //
        // 요약:
        //     The value of the curve at keyframe.
        public float value
        {
            get
            {
                return m_Value;
            }
            set
            {
                m_Value = value;
            }
        }

        //
        // 요약:
        //     Sets the incoming tangent for this key. The incoming tangent affects the slope
        //     of the curve from the previous key to this key.
        public float inTangent
        {
            get
            {
                return m_InTangent;
            }
            set
            {
                m_InTangent = value;
            }
        }

        //
        // 요약:
        //     Sets the outgoing tangent for this key. The outgoing tangent affects the slope
        //     of the curve from this key to the next key.
        public float outTangent
        {
            get
            {
                return m_OutTangent;
            }
            set
            {
                m_OutTangent = value;
            }
        }

        //
        // 요약:
        //     Sets the incoming weight for this key. The incoming weight affects the slope
        //     of the curve from the previous key to this key.
        public float inWeight
        {
            get
            {
                return m_InWeight;
            }
            set
            {
                m_InWeight = value;
            }
        }

        //
        // 요약:
        //     Sets the outgoing weight for this key. The outgoing weight affects the slope
        //     of the curve from this key to the next key.
        public float outWeight
        {
            get
            {
                return m_OutWeight;
            }
            set
            {
                m_OutWeight = value;
            }
        }

        //
        // 요약:
        //     Weighted mode for the keyframe.
        public WeightedMode weightedMode
        {
            get
            {
                return (WeightedMode)m_WeightedMode;
            }
            set
            {
                m_WeightedMode = (int)value;
            }
        }

        //
        // 요약:
        //     TangentMode is deprecated. Use AnimationUtility.SetKeyLeftTangentMode or AnimationUtility.SetKeyRightTangentMode
        //     instead.
        [Obsolete("Use AnimationUtility.SetKeyLeftTangentMode, AnimationUtility.SetKeyRightTangentMode, AnimationUtility.GetKeyLeftTangentMode or AnimationUtility.GetKeyRightTangentMode instead.")]
        public int tangentMode
        {
            get
            {
                return tangentModeInternal;
            }
            set
            {
                tangentModeInternal = value;
            }
        }

        internal int tangentModeInternal
        {
            get
            {
                return m_TangentMode;
            }
            set
            {
                m_TangentMode = value;
            }
        }

        //
        // 요약:
        //     Create a keyframe.
        //
        // 매개 변수:
        //   time:
        //
        //   value:
        public Keyframe(float time, float value)
        {
            m_Time = time;
            m_Value = value;
            m_InTangent = 0f;
            m_OutTangent = 0f;
            m_WeightedMode = 0;
            m_InWeight = 0f;
            m_OutWeight = 0f;
            m_TangentMode = 0;
        }

        //
        // 요약:
        //     Create a keyframe.
        //
        // 매개 변수:
        //   time:
        //
        //   value:
        //
        //   inTangent:
        //
        //   outTangent:
        public Keyframe(float time, float value, float inTangent, float outTangent)
        {
            m_Time = time;
            m_Value = value;
            m_InTangent = inTangent;
            m_OutTangent = outTangent;
            m_WeightedMode = 0;
            m_InWeight = 0f;
            m_OutWeight = 0f;
            m_TangentMode = 0;
        }

        //
        // 요약:
        //     Create a keyframe.
        //
        // 매개 변수:
        //   time:
        //
        //   value:
        //
        //   inTangent:
        //
        //   outTangent:
        //
        //   inWeight:
        //
        //   outWeight:
        public Keyframe(float time, float value, float inTangent, float outTangent, float inWeight, float outWeight)
        {
            m_Time = time;
            m_Value = value;
            m_InTangent = inTangent;
            m_OutTangent = outTangent;
            m_WeightedMode = 3;
            m_InWeight = inWeight;
            m_OutWeight = outWeight;
            m_TangentMode = 0;
        }
        public Keyframe(UnityEngine.Keyframe keyframe)
        {
            m_Time = keyframe.time;
            m_Value = keyframe.value;
            m_InTangent = keyframe.inTangent;
            m_OutTangent = keyframe.outTangent;
            m_WeightedMode = (int)keyframe.weightedMode;
            m_InWeight = keyframe.inWeight;
            m_OutWeight = keyframe.outWeight;
            m_TangentMode = keyframe.tangentMode;
        }
        public UnityEngine.Keyframe Convert()
        {
            return new UnityEngine.Keyframe()
            {
                time = m_Time,
                value = m_Value,
                inTangent = m_InTangent,
                outTangent = m_OutTangent,
                inWeight = m_InWeight,
                outWeight = m_OutWeight,
                tangentMode = m_TangentMode,
                weightedMode = weightedMode
            };
        }

        public static implicit operator Keyframe(UnityEngine.Keyframe v)
        {
            return new Keyframe
            {
                m_Time = v.time,
                m_Value = v.value,
                m_InTangent = v.inTangent,
                m_OutTangent = v.outTangent,
                m_WeightedMode = (int)v.weightedMode,
                m_InWeight = v.inWeight,
                m_OutWeight = v.outWeight,
                m_TangentMode = v.tangentMode
            };
        }
        public static implicit operator UnityEngine.Keyframe(Keyframe v)
        {
            return v.Convert();
        }
    }
    

    [Serializable]
    public struct Curve : IComparer<Keyframe>
    {
        [SerializeField]// Not used directly, but doesn't work without it
        private Keyframe[] _keys;
        public Keyframe[] keys
        {
            get => GetKeys();
            set => SetKeys(value);
        }
        public AnimationCurve GetAnimationCurve
        {
            get => ToAnimationCurve;
            set => SetKeys(_keys);
        }

        private Keyframe[] GetKeys()
        {
            return _keys;
        }
        private void SetKeys(Keyframe[] keys)
        {
            _keys = keys;
        }
        public void SetKeys(AnimationCurve animationCurve)
        {
            if (animationCurve == null)
                return;

            _keys = new Keyframe[animationCurve.keys.Length];
            for(int i = 0; i < animationCurve.keys.Length; i++)
            {
                _keys[i] = animationCurve.keys[i];
            }
            //_keys = animationCurve.keys.Select(t => new Keyframe(t)).ToArray();
        }
        public static Curve SetKeys(Curve curve, AnimationCurve animationCurve)
        {
            curve.SetKeys(animationCurve);
            return curve;
        }
        private void SortKeys()
        {
            Array.Sort(_keys, Compare);
        }

        public void AddKey(float time, float value)
        {
            AddKey(new Keyframe(time, value));
        }
        public void AddKey(Keyframe keyframe)
        {
            if (_keys == null)
            {
                _keys = new Keyframe[1];
            }
            else
                Array.Resize(ref _keys, _keys.Length + 1);

            _keys[^1] = keyframe;
            SortKeys();
        }
        public void AddKey(params Keyframe[] keyframes)
        {
            if (_keys == null)
                _keys = new Keyframe[keyframes.Length];
            else
                Array.Resize(ref _keys, keys.Length + keyframes.Length);

            for (int i = keyframes.Length - 1; i >= 0; i--) 
            {
                _keys[^(i + 1)] = keyframes[i];
            }
            SortKeys();
        }
        public static Curve operator +(Curve curve, Keyframe key)
        {
            curve.AddKey(key);
            return curve;
        }

        public void MoveKey(int index, Keyframe key)
        {
            _keys[index] = key;
            SortKeys();
        }
        public void MoveKey(int index, float time, float value)
        {
            _keys[index] = new Keyframe(time, value);
            SortKeys();
        }
        public void RemoveKey(int index)
        {
            if (index >= _keys.Length || index < 0)
                return;

            var newKeys = new Keyframe[_keys.Length - 1];
            for (int i = 0, count = 0; i < _keys.Length; i++)
            {
                if (i != index)
                {
                    newKeys[count] = _keys[i];
                    count++;
                }
            }
            _keys = newKeys;
        }
        public void ClearKeys()
        {
            _keys = new Keyframe[0];
        }
        public int Length
        {
            get
            {
                if (_keys == null)
                {
                    return -1;
                }else
                {
                    return _keys.Length;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t">keyframe0 - keyframe1 to Processed Time</param>
        /// <param name="keyframe0"></param>
        /// <param name="keyframe1"></param>
        /// <returns></returns>
        float Evaluate(float t, Keyframe keyframe0, Keyframe keyframe1)
        {
            float dt = keyframe1.time - keyframe0.time;

            float m0 = keyframe0.outTangent * dt;
            float m1 = keyframe1.inTangent * dt;

            float t2 = t * t;
            float t3 = t2 * t;

            float a = 2 * t3 - 3 * t2 + 1;
            float b = t3 - 2 * t2 + t;
            float c = t3 - t2;
            float d = -2 * t3 + 3 * t2;

            return a * keyframe0.value + b * m0 + c * m1 + d * keyframe1.value;
        }
        public float Evaluate(float time)
        {
            int keyIndex = Array.FindLastIndex(_keys, (t => t.time <= time));
            if (keyIndex < 0)
                return 0f;
            if (keyIndex == _keys.Length - 1)
            {
                return _keys[^1].value;
            }

            float rate = (time - _keys[keyIndex].time) / (_keys[keyIndex + 1].time - _keys[keyIndex].time);

            return Evaluate(rate, _keys[keyIndex], _keys[keyIndex + 1]);
        }

        public int Compare(Keyframe x, Keyframe y)
        {
            if (x.time > y.time)
                return 1;
            if (x.time < y.time)
                return -1;
            else
                return 0;

        }
        public AnimationCurve ToAnimationCurve
        {
            get
            {
                if (_keys == null)
                {
                    return new AnimationCurve();
                }


                var Lkeys = new UnityEngine.Keyframe[_keys.Length];
                for (int i = 0; i < Lkeys.Length; i++)
                {
                    Lkeys[i] = _keys[i];
                }
                //Lkeys = _keys.Select(t => t.Convert()).ToArray();

                return new AnimationCurve(Lkeys);
            }
        }
        public NativeCurve ToNativeCurve(Allocator allocator)
        {
            return new NativeCurve(_keys, allocator);
        }
    }

    /// <summary>
    /// Use For Curve in JobSystem , Recommand ReadOnly
    /// </summary>
    public struct NativeCurve
    {
        private NativeArray<Keyframe> _keys;
        public NativeArray<Keyframe> Keys
        {
            get => _keys;
            set => _keys = value;
        }

        public NativeCurve(NativeArray<Keyframe> keyframes)
        {
            _keys = keyframes;
        }
        public NativeCurve(Keyframe[] keyframes, Allocator allocator)
        {
            _keys = new NativeArray<Keyframe>(keyframes, allocator);
        }
        public NativeCurve(Curve curve, Allocator allocator)
        {
            _keys = new NativeArray<Keyframe>(curve.keys, allocator);
        }

        public int Length
        {
            get
            {
                if (_keys.IsCreated)
                    return _keys.Length;
                else
                    return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t">keyframe0 - keyframe1 to Percent</param>
        /// <param name="keyframe0"></param>
        /// <param name="keyframe1"></param>
        /// <returns></returns>
        public static float Evaluate(float t, Keyframe keyframe0, Keyframe keyframe1)
        {
            float dt = keyframe1.time - keyframe0.time;

            float m0 = keyframe0.outTangent * dt;
            float m1 = keyframe1.inTangent * dt;

            float t2 = t * t;
            float t3 = t2 * t;

            float a = 2 * t3 - 3 * t2 + 1;
            float b = t3 - 2 * t2 + t;
            float c = t3 - t2;
            float d = -2 * t3 + 3 * t2;

            return a * keyframe0.value + b * m0 + c * m1 + d * keyframe1.value;
        }
        public float Evaluate(float time)
        {
            if (_keys.IsCreated == false)
                return 0;

            var Lkeys = _keys.AsReadOnly();

            int keyIndex = -1;
            //Array.FindLastIndex(_keys, (t => t.time <= time));
            for (int i = Length - 1; i >= 0; i--)
            {
                if (time >= Lkeys[i].time)
                {
                    keyIndex = i;
                    break;
                }
            }

            if (keyIndex < 0)
                return 0f;
            if (keyIndex == Lkeys.Length - 1)
            {
                return Lkeys[^1].value;
            }

            float rate = (time - Lkeys[keyIndex].time) / (Lkeys[keyIndex + 1].time - Lkeys[keyIndex].time);

            return Evaluate(rate, Lkeys[keyIndex], Lkeys[keyIndex + 1]);
        }
        public static float Evaluate(NativeArray<Keyframe>.ReadOnly frames, float time)
        {
            if (frames.IsCreated == false)
                return 0;

            int keyIndex = -1;
            for (int i = frames.Length - 1; i >= 0; i--)
            {
                if (time >= frames[i].time)
                {
                    keyIndex = i;
                    break;
                }
            }


            if (keyIndex < 0)
                return 0f;
            if (keyIndex == frames.Length - 1)
            {
                return frames[^1].value;
            }

            float rate = (time - frames[keyIndex].time) / (frames[keyIndex + 1].time - frames[keyIndex].time);

            return Evaluate(rate, frames[keyIndex], frames[keyIndex + 1]);
        }
        /// <summary>
        /// Do not Test yet
        /// </summary>
        public static float Evaluate(ref BlobArray<Keyframe> frames, float time)
        {
            if (frames.Length <= 0)
                return 0;

            int keyIndex = -1;
            for (int i = frames.Length - 1; i >= 0; i--)
            {
                if (time >= frames[i].time)
                {
                    keyIndex = i;
                    break;
                }
            }


            if (keyIndex < 0)
                return 0f;
            if (keyIndex == frames.Length - 1)
            {
                return frames[^1].value;
            }

            float rate = (time - frames[keyIndex].time) / (frames[keyIndex + 1].time - frames[keyIndex].time);

            return Evaluate(rate, frames[keyIndex], frames[keyIndex + 1]);
        }


        public void Dispose()
        {
            _keys.Dispose();
        }
        public void Dispose(JobHandle inputDeps)
        {
            _keys.Dispose(inputDeps);
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(Curve))]
    public class CurveEditor : PropertyDrawer
    {
        Curve onwer;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            onwer = (Curve)property.GetUnderlyingValue();

            //var keys = property.FindPropertyRelative("_keys");//NotWork

            if (property.GetUnderlyingValue() != null)
            {
                var curve = onwer.ToAnimationCurve;
                var animCurve = EditorGUI.CurveField(position, label, curve);
                property.SetUnderlyingValue(Curve.SetKeys(onwer, animCurve));

                if (AnimationCurve.Equals(curve, animCurve) == false)
                    EditorUtility.SetDirty(property.serializedObject.targetObject);//This is required to apply value changes

                //typeof(Curve).GetProperty("keys").SetValue(onwer, animCurve.keys);

            }
        }
    }
#endif
}