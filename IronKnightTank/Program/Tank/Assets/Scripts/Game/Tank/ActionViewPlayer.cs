namespace Game.Tank
{
    using System;
    using Game.Battle.ActionView;
    using Game.Tools;
    using UnityEngine;


    public class ActionViewPlayer : MonoBehaviour {
        public ActionViewDictionary ActionViewDic = new ActionViewDictionary();

        public void PlayOnceView(string actionName, Action callBack = null)
        {
            BaseActionView view;
            if (this.ActionViewDic.TryGetValue(actionName, out view))
            {
                OnceActionView onceBaseActionView = view as OnceActionView;
                if (onceBaseActionView != null)
                {
                    onceBaseActionView.Play(callBack);
                }
                else
                {
                    if (callBack != null)
                    {
                        callBack();
                    }
                
                }
            }
            else
            {
                Debug.Log("not once action view"+actionName);
                if (callBack != null)
                {
                    callBack();
                }
                //TODO 打印日志到控制臺
            }
        }

        public void PlayLoopView(string actionName)
        {
            BaseActionView view;
            if (this.ActionViewDic.TryGetValue(actionName, out view))
            {
                view.Play();
            }
            else
            {
                //TODO 打印日志到控制臺
            }
        }

        public void StopLoopView(string actionName)
        {
            BaseActionView view;
            if (this.ActionViewDic.TryGetValue(actionName, out view))
            {
                LoopActionView loopBase = view as LoopActionView;
                if (loopBase!=null)
                {
                    loopBase.Stop();
                }
            }
            else
            {
                //TODO 打印日志到控制臺
            }
        }
    

        [Serializable]
        public class ActionViewDictionary : SerializableDictionary<string , BaseActionView>
        {
            
        }
    
    }
}
