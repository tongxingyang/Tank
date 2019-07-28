// ----------------------------------------------------------------------------
// <copyright file="SoundUtilitiesListener.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>06/06/2016</date>
// ----------------------------------------------------------------------------
namespace Assets.Tools.Script.Sound
{
    using System;
    using System.Collections.Generic;

    using Assets.Script.Mvc.Pool;
    using Assets.Tools.Script.Go;

    using UnityEngine;

    using Object = UnityEngine.Object;

    public class Audio2DManager : MonoBehaviour
    {
        public static GameObject Audio2DManagerRoot;

        public static Audio2DManager Init()
        {
            var secondaryHost = ParasiticComponent.GetSecondaryHost<Audio2DManager>("Audio2DManager");
            Audio2DManagerRoot = secondaryHost.gameObject;
            secondaryHost.Listener = Audio2DManagerRoot.AddComponent<AudioListener>();
            return secondaryHost;
        }

        public AudioListener Listener;

        public List<AduioData> PlayingList = new List<AduioData>();

        private AudioSourcePool audioSourcePool = new AudioSourcePool();
        

        public AudioSource Play(AudioClip clip,float volume, float pitch, bool loop,bool ignoreTimeScale, Action onEnd)
        {
            var aduioData = this.audioSourcePool.GetInstance();

            aduioData.IgnoreTimeScale = ignoreTimeScale;
            aduioData.OnEnd = onEnd;
            aduioData.Pitch = pitch;

            var source = aduioData.Source;
            source.pitch = ignoreTimeScale ? pitch: pitch * Time.timeScale;
            source.clip = clip;
            source.volume = volume;
            source.loop = loop;
            source.Play();
            
            this.PlayingList.Add(aduioData);
            return source;
        }

        private void Update()
        {
            for (int i = this.PlayingList.Count - 1; i >= 0; i--)
            {
                var aduioData = this.PlayingList[i];

                if (!aduioData.IgnoreTimeScale)
                {
                    aduioData.Source.pitch = aduioData.Pitch * Time.timeScale;
                }

                if (!aduioData.Source.isPlaying)
                {
                    aduioData.Source.clip = null;
                    if (aduioData.OnEnd != null)
                    {
                        aduioData.OnEnd();
                    }
                    this.PlayingList.RemoveAt(i);
                    var returnInstance = this.audioSourcePool.ReturnInstance(aduioData);
                    if (!returnInstance)
                    {
                        aduioData.Dispose();
                    }
                }
            }
        }

        public class AduioData
        {
            public AudioSource Source;

            public Action OnEnd;

            public float Pitch;

            public bool IgnoreTimeScale;

            public void Dispose()
            {
                Object.Destroy(this.Source);
            }

            public AduioData Create()
            {
                this.Source = Audio2DManagerRoot.AddComponent<AudioSource>();
                return this;
            }
        }

        public class AudioSourcePool : Pool<AduioData>
        {
            public AudioSourcePool()
            {
                this.MaxCount = 20;
            }

            protected override object CreateObject()
            {
                return new AduioData().Create();
            }
        }
    }
}