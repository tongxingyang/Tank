// ----------------------------------------------------------------------------
// <copyright file="MapEditorWindow.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>zhaowenpeng</author>
// <date>09/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Editor.SkillEditor
{
    using Assets.Framework.LetsScript.Editor;
    using UnityEditor;
    using UnityEngine;

    public class SkillEditor : MonoBehaviour
    {

        [MenuItem("Assets/SkillEidtor/Create")]
        public static void CreateTestScript()
        {
            LetsScriptEditor.CreateScript("SoliderSkillTemplate");
        }
    }
}
