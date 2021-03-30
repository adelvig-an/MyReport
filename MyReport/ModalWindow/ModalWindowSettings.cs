﻿using System;
using System.Threading;
using System.Windows;

namespace MyReport.ModalWindow
{
    public class ModalWindowSettings
    {
        public ModalWindowSettings()
        {
            this.OwnerCanCloseWithDialog = false;

            this.AffirmativeButtonText = "OK";
            this.NegativeButtonText = "Cancel";

            this.AnimateShow = this.AnimateHide = true;

            this.MaximumBodyHeight = Double.NaN;

            this.DefaultText = "";
            this.DefaultButtonFocus = ModalWindowResult.Negative;
            this.CancellationToken = CancellationToken.None;
            this.DialogTitleFontSize = Double.NaN;
            this.DialogMessageFontSize = Double.NaN;
            this.DialogButtonFontSize = Double.NaN;
            this.DialogResultOnCancel = null;
        }

        /// <summary>
        /// Gets or sets wheater the owner of the dialog can be closed.
        /// </summary>
        public bool OwnerCanCloseWithDialog { get; set; }

        /// <summary>
        /// Gets or sets the text used for the Affirmative button. For example: "OK" or "Yes".
        /// </summary>
        public string AffirmativeButtonText { get; set; }

        /// <summary>
        /// Enable or disable dialog hiding animation
        /// "True" - play hiding animation.
        /// "False" - skip hiding animation.
        /// </summary>
        public bool AnimateHide { get; set; }

        /// <summary>
        /// Enable or disable dialog showing animation.
        /// "True" - play showing animation.
        /// "False" - skip showing animation.
        /// </summary>
        public bool AnimateShow { get; set; }

        /// <summary>
        /// Gets or sets a token to cancel the dialog.
        /// </summary>
        public CancellationToken CancellationToken { get; set; }

        /// <summary>
        /// Gets or sets a custom resource dictionary which can contains custom styles, brushes or something else.
        /// </summary>
        public ResourceDictionary CustomResourceDictionary { get; set; }

        /// <summary>
        /// Gets or sets which button should be focused by default
        /// </summary>
        public ModalWindowResult DefaultButtonFocus { get; set; }

        /// <summary>
        /// Gets or sets the default text (just the inputdialog needed)
        /// </summary>
        public string DefaultText { get; set; }

        /// <summary>
        /// Gets or sets the size of the dialog message font.
        /// </summary>
        /// <value>
        /// The size of the dialog message font.
        /// </value>
        public double DialogMessageFontSize { get; set; }

        /// <summary>
        /// Gets or sets the size of the dialog button font.
        /// </summary>
        /// <value>
        /// The size of the dialog button font.
        /// </value>
        public double DialogButtonFontSize { get; set; }

        /// <summary>
        /// Gets or sets the dialog result when the user cancelled the dialog with 'ESC' key
        /// </summary>
        /// <remarks>If the value is <see langword="null"/> the default behavior is determined 
        /// by the <see cref="MessageDialogStyle"/>.
        /// <table>
        /// <tr><td><see cref="MessageDialogStyle"/></td><td><see cref="MessageDialogResult"/></td></tr>
        /// <tr><td><see cref="MessageDialogStyle.Affirmative"/></td><td><see cref="MessageDialogResult.Affirmative"/></td></tr>
        /// <tr><td>
        /// <list type="bullet">
        /// <item><see cref="MessageDialogStyle.AffirmativeAndNegative"/></item>
        /// <item><see cref="MessageDialogStyle.AffirmativeAndNegativeAndSingleAuxiliary"/></item>
        /// <item><see cref="MessageDialogStyle.AffirmativeAndNegativeAndDoubleAuxiliary"/></item>
        /// </list></td>
        /// <td><see cref="MessageDialogResult.Negative"/></td></tr></table></remarks>
        public ModalWindowResult? DialogResultOnCancel { get; set; }

        /// <summary>
        /// Gets or sets the size of the dialog title font.
        /// </summary>
        /// <value>
        /// The size of the dialog title font.
        /// </value>
        public double DialogTitleFontSize { get; set; }

        /// <summary>
        /// Gets or sets the text used for the first auxiliary button.
        /// </summary>
        public string FirstAuxiliaryButtonText { get; set; }

        /// <summary>
        /// Gets or sets the maximum height. (Default is unlimited height, <a href="http://msdn.microsoft.com/de-de/library/system.double.nan">Double.NaN</a>)
        /// </summary>
        public double MaximumBodyHeight { get; set; }

        /// <summary>
        /// Gets or sets the text used for the Negative button. For example: "Cancel" or "No".
        /// </summary>
        public string NegativeButtonText { get; set; }

        /// <summary>
        /// Gets or sets the text used for the second auxiliary button.
        /// </summary>
        public string SecondAuxiliaryButtonText { get; set; }
    }
}
