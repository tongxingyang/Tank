using Assets.Tools.Script.Event;

namespace Assets.Tools.Script.Popupbox
{
    /// <summary>
    /// 弹出框
    /// </summary>
    public interface IPopBox
    {
        Signal<IPopBox> OnCloseSignal { get; }
        void Show();

        void SetActive(bool boxActive);
    }
}