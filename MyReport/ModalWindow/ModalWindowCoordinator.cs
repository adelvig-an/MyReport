using System;
using System.Threading.Tasks;
using System.Windows;

namespace MyReport.ModalWindow
{
    public class ModalWindowCoordinator : IModalWindowCoordinator
    {
        /// <summary>
        /// Gets the default instance if the dialog coordinator, which can be injected into a view model.
        /// </summary>
        public static readonly IModalWindowCoordinator Instance = new ModalWindowCoordinator();

        public Task<string> ShowInputAsync(object context, string title, string message, ModalWindowSettings settings = null)
        {
            var modalWindow = GetWindow(context);
            return modalWindow.Invoke(() => modalWindow.ShowInputAsync(title, message, settings));
        }

        public string ShowModalInputExternal(object context, string title, string message, ModalWindowSettings metroDialogSettings = null)
        {
            var modalWindow = GetWindow(context);
            return modalWindow.ShowModalInputExternal(title, message, metroDialogSettings);
        }

        public Task<ModalWindowResult> ShowMessageAsync(object context, string title, string message, ModalWindowSettings settings = null)
        {
            var modalWindow = GetWindow(context);
            return modalWindow.Invoke(() => modalWindow.ShowMessageAsync(title, message, settings));
        }

        public ModalWindowResult ShowModalMessageExternal(object context, string title, string message, ModalWindowSettings settings = null)
        {
            var modalWindow = GetWindow(context);
            return modalWindow.ShowModalMessageExternal(title, message, settings);
        }

        public Task ShowModalWindowAsync(object context, BaseModalWindow dialog, ModalWindowSettings settings = null)
        {
            var modalWindow = GetWindow(context);
            return modalWindow.Invoke(() => modalWindow.ShowMetroDialogAsync(dialog, settings));
        }

        public Task HideModalWindowAsync(object context, BaseModalWindow dialog, ModalWindowSettings settings = null)
        {
            var modalWindow = GetWindow(context);
            return modalWindow.Invoke(() => modalWindow.HideMetroDialogAsync(dialog, settings));
        }

        public Task<TDialog> GetCurrentDialogAsync<TDialog>(object context)
            where TDialog : BaseModalWindow
        {
            var mmodalWindow = GetWindow(context);
            return mmodalWindow.Invoke(() => mmodalWindow.GetCurrentDialogAsync<TDialog>());
        }

        private static Window GetWindow(object context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (!ModalWindowParticipation.IsRegistered(context))
            {
                throw new InvalidOperationException("Context is not registered. Consider using DialogParticipation.Register in XAML to bind in the DataContext.");
            }

            var association = ModalWindowParticipation.GetAssociation(context);
            var modalWindow = association.Invoke(() => Window.GetWindow(association) as Window);
            if (modalWindow == null)
            {
                throw new InvalidOperationException("Context is not inside a Window.");
            }

            return modalWindow;
        }
    }
}
