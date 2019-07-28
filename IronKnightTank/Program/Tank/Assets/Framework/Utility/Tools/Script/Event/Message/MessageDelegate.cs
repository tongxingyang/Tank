#if UNITY_EDITOR || (!UNITY_FLASH && !NETFX_CORE)
#define REFLECTION_SUPPORT
#endif

#if REFLECTION_SUPPORT
#endif
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Assets.Tools.Script.Event.Message
{
    /// <summary>
    /// Delegate callback that Unity can serialize and set via Inspector.
    /// </summary>

    [System.Serializable]
    public class MessageDelegate:IMessageDelegate
    {
        [SerializeField]
        MonoBehaviour mTarget;
        [SerializeField]
        string mMethodName;

        /// <summary>
        /// Whether the event delegate will be removed after execution.
        /// </summary>

        public bool oneShot = false;

//    public delegate void MessageCallBack(object obj);
        MessageCallBack mCachedCallback;
        bool mRawDelegate = false;

        /// <summary>
        /// Event delegate's target object.
        /// </summary>

        public MonoBehaviour target { get { return mTarget; } set { mTarget = value; mCachedCallback = null; mRawDelegate = false; } }

        /// <summary>
        /// Event delegate's method name.
        /// </summary>

        public string methodName { get { return mMethodName; } set { mMethodName = value; mCachedCallback = null; mRawDelegate = false; } }

        /// <summary>
        /// Whether this delegate's values have been set.
        /// </summary>

        public bool isValid { get { return (mRawDelegate && mCachedCallback != null) || (mTarget != null && !string.IsNullOrEmpty(mMethodName)); } }

        /// <summary>
        /// Whether the target script is actually enabled.
        /// </summary>

        public bool isEnabled
        {
            get
            {
                if (mRawDelegate && mCachedCallback != null) return true;
                if (mTarget == null) return false;
                MonoBehaviour mb = (mTarget as MonoBehaviour);
                return (mb == null || mb.enabled);
            }
        }

        public MessageDelegate() { }
        public MessageDelegate(MessageCallBack call) { Set(call); }
        public MessageDelegate(MonoBehaviour target, string methodName) { Set(target, methodName); }

        /// <summary>
        /// GetMethodName is not supported on some platforms.
        /// </summary>

#if REFLECTION_SUPPORT
#if !UNITY_EDITOR && UNITY_WP8
		static string GetMethodName (MessageCallBack callback)
		{
			System.Delegate d = callback as System.Delegate;
			return d.Method.Name;
		}

		static bool IsValid (MessageCallBack callback)
		{
			System.Delegate d = callback as System.Delegate;
			return d != null && d.Method != null;
		}
#elif !UNITY_EDITOR && UNITY_METRO
		static string GetMethodName (MessageCallBack callback)
		{
			System.Delegate d = callback as System.Delegate;
			return d.GetMethodInfo().Name;
		}

		static bool IsValid (MessageCallBack callback)
		{
			System.Delegate d = callback as System.Delegate;
			return d != null && d.GetMethodInfo() != null;
		}
#else
        static string GetMethodName(MessageCallBack callback) { return callback.Method.Name; }
        static bool IsValid(MessageCallBack callback) { return callback != null && callback.Method != null; }
#endif
#else
	static bool IsValid (MessageCallBack callback) { return callback != null; }
#endif

        /// <summary>
        /// Equality operator.
        /// </summary>

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return !isValid;
            }

            if (obj is MessageCallBack)
            {
                MessageCallBack callback = obj as MessageCallBack;
#if REFLECTION_SUPPORT
                if (callback.Equals(mCachedCallback)) return true;
                MonoBehaviour mb = callback.Target as MonoBehaviour;
                return (mTarget == mb && string.Equals(mMethodName, GetMethodName(callback)));
#elif UNITY_FLASH
			return (callback == mCachedCallback);
#else
			return callback.Equals(mCachedCallback);
#endif
            }

            if (obj is MessageDelegate)
            {
                MessageDelegate del = obj as MessageDelegate;
                return (mTarget == del.mTarget && string.Equals(mMethodName, del.mMethodName));
            }
            return false;
        }

        static int s_Hash = "MessageDelegate".GetHashCode();

        /// <summary>
        /// Used in equality operators.
        /// </summary>

        public override int GetHashCode() { return s_Hash; }

        /// <summary>
        /// Convert the saved target and method name into an actual delegate.
        /// </summary>

        MessageCallBack Get()
        {
#if REFLECTION_SUPPORT
            if (!mRawDelegate && (mCachedCallback == null || (mCachedCallback.Target as MonoBehaviour) != mTarget || GetMethodName(mCachedCallback) != mMethodName))
            {
                if (mTarget != null && !string.IsNullOrEmpty(mMethodName))
                {
                    mCachedCallback = MessageCallBack.Create(mTarget, mMethodName);
                }
                else return null;
            }
#endif
            if (mTarget == null) return null;
            return mCachedCallback;
        }

        /// <summary>
        /// Set the delegate callback directly.
        /// </summary>

        void Set(MessageCallBack call)
        {
            if (call == null || !IsValid(call))
            {
                mTarget = null;
                mMethodName = null;
                mCachedCallback = null;
                mRawDelegate = false;
            }
            else
            {
#if REFLECTION_SUPPORT
                mTarget = call.Target as MonoBehaviour;

                if (mTarget == null)
                {
                    mRawDelegate = true;
                    mCachedCallback = call;
                    mMethodName = null;
                }
                else
                {
                    mMethodName = GetMethodName(call);
                    mRawDelegate = false;
                }
#else
			mRawDelegate = true;
			mCachedCallback = call;
			mMethodName = null;
			mTarget = null;
#endif
            }
        }

        /// <summary>
        /// Set the delegate callback using the target and method names.
        /// </summary>

        public void Set(MonoBehaviour target, string methodName)
        {
            this.mTarget = target;
            this.mMethodName = methodName;
            mCachedCallback = null;
            mRawDelegate = false;
        }

        /// <summary>
        /// Execute the delegate, if possible.
        /// This will only be used when the application is playing in order to prevent unintentional state changes.
        /// </summary>

        public bool Execute(object arg)
        {
            MessageCallBack call = null;
            try
            {
                call = Get();
            }
            catch (Exception)
            {
            
            }

            if (call != null)
            {
#if UNITY_EDITOR
                if (Application.isPlaying)
                {
                    call.Call(arg);
                }
                else if (call.Target != null)
                {
                    System.Type type = call.Target.GetType();
                    object[] objs = type.GetCustomAttributes(typeof(ExecuteInEditMode), true);
                    if (objs != null && objs.Length > 0) call.Call(arg);
                }
#else
			call.Call(arg);
#endif
                return true;
            }
            return false;
        }

        public bool Execute()
        {
            return Execute(null);
        }

        public string messageName { get; set; }

        /// <summary>
        /// Clear the event delegate.
        /// </summary>

        public void Clear()
        {
            mTarget = null;
            mMethodName = null;
            mRawDelegate = false;
            mCachedCallback = null;
        }

        /// <summary>
        /// Convert the delegate to its string representation.
        /// </summary>

        public override string ToString()
        {
            if (mTarget != null)
            {
                string typeName = mTarget.GetType().ToString();
                int period = typeName.LastIndexOf('.');
                if (period > 0) typeName = typeName.Substring(period + 1);

                if (!string.IsNullOrEmpty(methodName)) return typeName + "." + methodName;
                else return typeName + ".[delegate]";
            }
            return mRawDelegate ? "[delegate]" : null;
        }

        /// <summary>
        /// Execute an entire list of delegates.
        /// </summary>

        static public void Execute(List<MessageDelegate> list,object arg)
        {
            if (list != null)
            {
                for (int i = 0; i < list.Count; )
                {
                    MessageDelegate del = list[i];

                    if (del != null)
                    {
                        del.Execute(arg);

                        if (i >= list.Count) break;
                        if (list[i] != del) continue;

                        if (del.oneShot)
                        {
                            list.RemoveAt(i);
                            continue;
                        }
                    }
                    ++i;
                }
            }
        }

        /// <summary>
        /// Convenience function to check if the specified list of delegates can be executed.
        /// </summary>

        static public bool IsValid(List<MessageDelegate> list)
        {
            if (list != null)
            {
                for (int i = 0, imax = list.Count; i < imax; ++i)
                {
                    MessageDelegate del = list[i];
                    if (del != null && del.isValid)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Assign a new event delegate.
        /// </summary>

        static public void Set(List<MessageDelegate> list, MessageCallBack callback)
        {
            if (list != null)
            {
                list.Clear();
                list.Add(new MessageDelegate(callback));
            }
        }

        /// <summary>
        /// Assign a new event delegate.
        /// </summary>

        static public void Set(List<MessageDelegate> list, MessageDelegate del)
        {
            if (list != null)
            {
                list.Clear();
                list.Add(del);
            }
        }

        /// <summary>
        /// Append a new event delegate to the list.
        /// </summary>

        static public void Add(List<MessageDelegate> list, MessageCallBack callback) { Add(list, callback, false); }

        /// <summary>
        /// Append a new event delegate to the list.
        /// </summary>

        static public void Add(List<MessageDelegate> list, MessageCallBack callback, bool oneShot)
        {
            if (list != null)
            {
                for (int i = 0, imax = list.Count; i < imax; ++i)
                {
                    MessageDelegate del = list[i];
                    if (del != null && del.Equals(callback))
                        return;
                }

                MessageDelegate ed = new MessageDelegate(callback);
                ed.oneShot = oneShot;
                list.Add(ed);
            }
            else
            {
                UnityEngine.Debug.LogWarning("Attempting to add a callback to a list that's null");
            }
        }

        /// <summary>
        /// Append a new event delegate to the list.
        /// </summary>

        static public void Add(List<MessageDelegate> list, MessageDelegate ev) { Add(list, ev, ev.oneShot); }

        /// <summary>
        /// Append a new event delegate to the list.
        /// </summary>

        static public void Add(List<MessageDelegate> list, MessageDelegate ev, bool oneShot)
        {
            if (ev.mRawDelegate || ev.target == null || string.IsNullOrEmpty(ev.methodName))
            {
                Add(list, ev.mCachedCallback, oneShot);
            }
            else if (list != null)
            {
                for (int i = 0, imax = list.Count; i < imax; ++i)
                {
                    MessageDelegate del = list[i];
                    if (del != null && del.Equals(ev))
                        return;
                }

                MessageDelegate ed = new MessageDelegate(ev.target, ev.methodName);
                ed.oneShot = oneShot;
                list.Add(ed);
            }
            else
            {
                UnityEngine.Debug.LogWarning("Attempting to add a callback to a list that's null");
            }
        }

        /// <summary>
        /// Remove an existing event delegate from the list.
        /// </summary>

        static public bool Remove(List<MessageDelegate> list, MessageCallBack callback)
        {
            if (list != null)
            {
                for (int i = 0, imax = list.Count; i < imax; ++i)
                {
                    MessageDelegate del = list[i];

                    if (del != null && del.Equals(callback))
                    {
                        list.RemoveAt(i);
                        return true;
                    }
                }
            }
            return false;
        }

        public class MessageCallBack
        {
            public static MessageCallBack Create(MonoBehaviour monoBehaviour, string methodName)
            {
                MessageCallBack cb=null;
                try
                {
                    Callback callback = (Callback)System.Delegate.CreateDelegate(typeof(Callback), monoBehaviour, methodName);
                    cb = new MessageCallBack(callback);
                }
                catch (Exception)
                {
                    CallbackNoArg callbackNoArg = (CallbackNoArg)System.Delegate.CreateDelegate(typeof(CallbackNoArg), monoBehaviour, methodName);
                    cb = new MessageCallBack(callbackNoArg);
                }
            
                return cb;
            }

            public delegate void Callback(object obj);
            Callback mCachedCallback;
            public delegate void CallbackNoArg();
            CallbackNoArg mCachedCallbackNoArg;

//        public MessageCallBack()
//        {
//            
//        }

            public MessageCallBack(Callback callback)
            {
                mCachedCallback = callback;
            }

            public MessageCallBack(CallbackNoArg callback)
            {
                mCachedCallbackNoArg = callback;
            }

            public MethodInfo Method
            {
                get
                {
                    if (mCachedCallback != null)
                        return mCachedCallback.Method;
                    else
                        return mCachedCallbackNoArg.Method;
                }

            }

            public object Target
            {
                get
                {
                    if (mCachedCallback != null)
                        return mCachedCallback.Target;
                    else
                        return mCachedCallbackNoArg.Target;
                }

            }

            public void Call(object arg)
            {
                if (mCachedCallback != null)
                {
                    mCachedCallback(arg);
                }
                else
                {
                    mCachedCallbackNoArg();
                }
            }
        }
    }
}
