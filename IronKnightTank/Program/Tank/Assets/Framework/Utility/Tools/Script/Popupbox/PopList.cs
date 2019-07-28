using System.Collections.Generic;
using Assets.Tools.Script.Event;

namespace Assets.Tools.Script.Popupbox
{
    /// <summary>
    /// 按顺序弹出窗口的弹窗队列
    /// </summary>
    public class PopList
    {
        /// <summary>
        /// 共用的一个弹出队列
        /// </summary>
        public static PopList globalPopList=new PopList();
        /// <summary>
        /// 在队列中一个窗口被关闭
        /// </summary>
        public Signal<IPopBox> onOneClosesSignal { get; private set; }
        /// <summary>
        /// 队列中窗口全部弹出关闭的时候
        /// </summary>
        public SimpleSignal allEndSignal { get; private set; }
        /// <summary>
        /// 当前显示的弹窗
        /// </summary>
        public IPopBox currPopBox { get { return _currPopBox; } }

        //弹出窗口队列
        private List<IPopBox> _popBoxs=new List<IPopBox>();
        //当前的弹出窗口
        private IPopBox _currPopBox;

        public PopList()
        {
            onOneClosesSignal=new Signal<IPopBox>();
            allEndSignal=new SimpleSignal();
        }
        /// <summary>
        /// 添加一个弹窗
        /// </summary>
        /// <param name="popBox"></param>
        public void Add(IPopBox popBox)
        {
            
            _popBoxs.Add(popBox);
            popBox.SetActive(false);
            Show();
        }
        /// <summary>
        /// 添加弹窗在指定位序
        /// </summary>
        /// <param name="popBox"></param>
        /// <param name="index"></param>
        public void AddAt(IPopBox popBox,int index)
        {
            _popBoxs.Insert(index, popBox);
            popBox.SetActive(false);
            Show();
        }
        /// <summary>
        /// 开始显示弹出
        /// </summary>
        public void Show()
        {
            if (_currPopBox != null || _popBoxs.Count==0) return;
            ShowNext();
        }
        /// <summary>
        /// 清除所有弹窗
        /// </summary>
        public void Clear()
        {
            _popBoxs.Clear();
        }
        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            
        }

        private void OnOneCloseHandler(IPopBox obj)
        {
            onOneClosesSignal.Dispatch(obj);
            if (_popBoxs.Count > 0)
            {
                ShowNext();
            }
            else
            {
                _currPopBox = null;
                allEndSignal.Dispatch();
            }
        }

        private void ShowNext()
        {
            _currPopBox = _popBoxs[0];
            _popBoxs.RemoveAt(0);
            if (_currPopBox != null)
            {
                _currPopBox.SetActive(true);
                _currPopBox.Show();
                _currPopBox.OnCloseSignal.AddEventListener(OnOneCloseHandler);
            }
            else
            {
                OnOneCloseHandler(_currPopBox);//在打开之前就被销毁了
            }
        }
    }
}