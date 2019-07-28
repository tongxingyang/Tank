// ----------------------------------------------------------------------------
// <copyright file="IObjectAnalyseDisplayer.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>25/12/2015</date>
// ----------------------------------------------------------------------------
namespace Assets.Extends.EXTools.Debug.Console
{
    public interface IObjectAnalyseDisplayer
    {
        void ShowNewObject(object obj, string objName);

        bool Back();

        void Show();
    }
}