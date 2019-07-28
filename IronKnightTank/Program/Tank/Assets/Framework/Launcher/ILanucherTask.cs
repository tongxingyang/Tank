// ----------------------------------------------------------------------------
// <copyright file="ILanucherTask.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>10/04/2018</date>
// ----------------------------------------------------------------------------
namespace XQFramework.Laucher
{
    using System;

    public interface ILanucherTask
    {
        int Weight { get; }

        Action<ILanucherTask, float, string> SetTaskProgress { get; set; }

        void StartTask();
    }
}