// ----------------------------------------------------------------------------
// <copyright file="NativeEventToSignal.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>03/11/2015</date>
// ----------------------------------------------------------------------------
namespace Assets.Tools.Script.Event
{
    using UnityEngine;

    /// <summary>
    /// Class NativeEventToSignal.
    /// </summary>
    public class NativeEventToSignal : MonoBehaviour
    {
        /// <summary>
        /// The on native event
        /// </summary>
        public SimpleSignal OnNativeEvent = new SimpleSignal();

        /// <summary>
        /// Unities the native event.
        /// </summary>
        public void UnityNativeEvent()
        {
            this.OnNativeEvent.Dispatch();
        }
    }
}