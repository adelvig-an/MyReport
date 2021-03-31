using System.Windows;

namespace MyReport.ModalWindow
{
    public class CustomModalWindow : BaseModalWindow
    {
        public CustomModalWindow()
            : this(null, null)
        {
        }

        public CustomModalWindow(Window parentWindow)
            : this(parentWindow, null)
        {
        }

        public CustomModalWindow(ModalWindowSettings settings)
            : this(null, settings)
        {
        }

        public CustomModalWindow(Window parentWindow, ModalWindowSettings settings)
            : base(parentWindow, settings)
        {
        }
    }
}
