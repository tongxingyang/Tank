using System.Collections;
using Assets.Tools.Script.Event;
using UnityEngine;

namespace Assets.Tools.Script.Animation.Curve
{
    /// <summary>
    /// AnimationCurve 动画
    /// 使用AnimationCurve曲线控制动画
    /// </summary>
    public abstract class CurveAnimation : MonoBehaviour
    {
        /// <summary>
        /// 动画轴
        /// </summary>
        public AnimationCurve curve;
        /// <summary>
        /// 是否正向播放
        /// </summary>
        public bool isForward { get { return _isForward; } }
        private bool _isForward;
        /// <summary>
        /// 开始了的(正在播放或者暂停中)
        /// </summary>
        public bool started { get { return _playing || paused; } }
        /// <summary>
        /// 播放中
        /// </summary>
        public bool playing { get { return _playing; } }
        private bool _playing;
        /// <summary>
        /// 暂停中
        /// </summary>
        public bool paused { get; private set; }
        //事件------------------------------------------------------------
        public readonly Signal<CurveAnimation> onFinishSignal = new Signal<CurveAnimation>();
        //上帧画播放时间
        private float _preFrameTime;
        //动画总流逝时间
        private float _passTime;
        //两端的延续时间轴方法
        protected float endPassTime;
        protected WrapMode postWrapMode;
        protected float startPassTime;
        protected WrapMode preWrapMode;
        //单个周期时间
        protected float cycleTime;
        //-------------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            paused = false;
            _playing = false;
            _passTime = 0;
        }

        /// <summary>
        /// 暂停动画
        /// </summary>
        public void Pause()
        {
            if (!_playing) return;
            paused = true;
            _playing = false;
            StopCoroutine("UpdateAnimation");
        }
        /// <summary>
        /// 结束动画
        /// </summary>
        public void Finish()
        {
            if (!_playing) return;
            paused = false;
            _playing = false;
            StopCoroutine("UpdateAnimation");
            OnFinish();
            onFinishSignal.Dispatch(this);
        }
        /// <summary>
        /// 播放，改变为正播或者倒播，结束暂停
        /// </summary>
        /// <param name="forward">时间轴正向播放，或者倒播</param>
        public void Play(bool forward)
        {
            _isForward = forward;
            if (_playing) return;
            if (paused)//是从暂停中恢复过来的
            {
                
            }
            else//是新的开始
            {
                endPassTime = curve.keys[curve.length - 1].time;
                postWrapMode = curve.postWrapMode;
                startPassTime = curve.keys[0].time;
                preWrapMode = curve.preWrapMode;
                cycleTime = endPassTime - startPassTime;
                _passTime = 0;
            }
            _preFrameTime = Time.time;
            paused = false;
            _playing = true;
            StartCoroutine("UpdateAnimation");
        }
        /// <summary>
        /// 播放
        /// </summary>
        [ContextMenu("PlayForward")]
        public void PlayForward()
        {
            Play(true);
        }
        /// <summary>
        /// 倒播
        /// </summary>
        [ContextMenu("PlayReverse")]
        public void PlayReverse()
        {
            Play(false);
        }
        //-------------------------------------------------------------------------------------------------------//
        /// <summary>
        /// 结束时
        /// </summary>
        protected  virtual void OnFinish(){}
        /// <summary>
        /// 值改变时如何表现
        /// </summary>
        /// <param name="time">当前时间</param>
        /// <param name="value">当前值</param>
        protected abstract void OnPlay(float time, float value);
        //-------------------------------------------------------------------------------------------------------//
        private IEnumerator UpdateAnimation()
        {
            while (true)
            {
                float dTime = (Time.time - _preFrameTime) * (_isForward?1:-1);
                _passTime = _passTime + dTime;
                _preFrameTime = Time.time;

                OnPlay(_passTime, curve.Evaluate(_passTime));
                //根据时间轴，判断是否会结束
                if (_isForward && _passTime >= endPassTime && (postWrapMode == WrapMode.ClampForever || postWrapMode == WrapMode.Clamp))
                {
                    break;
                }
                if (!_isForward && _passTime < startPassTime && (preWrapMode == WrapMode.ClampForever || preWrapMode == WrapMode.Clamp))
                {
                    break;
                }
                yield return null;
            }
            Finish();
        }
    }
}