using System;
using Assets.Tools.Script.Go;
using Assets.Tools.Script.Helper;
using UnityEngine;

namespace Assets.Tools.Script.Animation.Curve
{
    /// <summary>
    /// CurveAnimation动画表现由一个指定的回调处理
    /// </summary>
    public class ExternalCurveAnimation:CurveAnimation
    {
        /// <summary>
        /// 表现回调
        /// </summary>
        public Action<float, float> externalPlayer;
        /// <summary>
        /// 克隆一个和当前一样的动画，播放时回调onPlay，由外部来展现动画
        /// </summary>
        /// <param name="attachTo">如果没有指定添加到的gameobject，则新生成一个gameobject到parasiteHost</param>
        /// <returns></returns>
        public ExternalCurveAnimation CloneCurveAnimation(GameObject attachTo=null)
        {
            attachTo = attachTo ?? ParasiticComponent.parasiteHost;
            var curveAnimationHost = GameObjectUtilities.AddInstantiate(attachTo,gameObject).GetComponent<ExternalCurveAnimation>();
            curveAnimationHost.Reset();
            return curveAnimationHost;
        }
        /// <summary>
        /// 设置播放回调
        /// </summary>
        /// <param name="externalPlayer">float time,float vallue</param>
        /// <returns></returns>
        public ExternalCurveAnimation SetExternalPlayer(Action<float, float> externalPlayer)
        {
            this.externalPlayer = externalPlayer;
            return this;
        }
        protected override void OnPlay(float time,float value)
        {
            if (externalPlayer != null) externalPlayer(time, value);
        }
    }
}