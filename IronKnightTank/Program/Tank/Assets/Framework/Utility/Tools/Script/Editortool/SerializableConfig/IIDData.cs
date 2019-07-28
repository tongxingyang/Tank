// ----------------------------------------------------------------------------
// <copyright file="IIDData.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>19/10/2015</date>
// ----------------------------------------------------------------------------
namespace Assets.Tools.Script.Editortool
{
    /// <summary>
    /// Interface IIDData
    /// </summary>
    public interface IIDData
    {
        /// <summary>
        /// Gets the data identifier.
        /// </summary>
        /// <returns>System.String.</returns>
        string GetDataId();

        /// <summary>
        /// Sets the data identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void SetDataId(string id);
    }
}