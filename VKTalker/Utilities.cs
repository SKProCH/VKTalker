using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Runtime.Serialization;
using System.Text;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Validation.Abstractions;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Formatters;
using ReactiveUI.Validation.Formatters.Abstractions;
using ReactiveUI.Validation.ValidationBindings;
using Splat;

namespace VKTalker {
    public static class Utilities {
        public static string CreateMD5(string input) {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create()) {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++) {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public static void OpenUrlInBrowser(string url) {
            Process.Start(new ProcessStartInfo() { FileName = url, UseShellExecute = true });
        }

        public static IDisposable BindValidationToTextBoxErrors<TView, TViewModel, TViewModelProperty>(
            this TView view,
            TViewModel? viewModel,
            Expression<Func<TViewModel, TViewModelProperty>> viewModelProperty,
            Expression<Func<TView, Control>> targetControl,
            bool suppressInitialError,
            IValidationTextFormatter<string>? formatter = null)
            where TView : IViewFor<TViewModel>
            where TViewModel : class, IReactiveObject, IValidatableViewModel {
            if (viewModelProperty is null) {
                throw new ArgumentNullException(nameof(viewModelProperty));
            }

            if (targetControl is null) {
                throw new ArgumentNullException(nameof(targetControl));
            }

            if (view is null) {
                throw new ArgumentNullException(nameof(view));
            }

            formatter ??= Locator.Current.GetService<IValidationTextFormatter<string>>() ??
                          SingleLineFormatter.Default;

            var getTargetControl = targetControl.Compile();

            var obs = view
                .WhenAnyValue(v => v.ViewModel)
                .Where(vm => vm is not null)
                .SelectMany(vm => vm!.ValidationContext.ObserveFor(viewModelProperty))
                .Select(states => states
                    .Select(state => formatter.Format(state.Text))
                    .FirstOrDefault(msg => !string.IsNullOrEmpty(msg)) ?? string.Empty);
            if (suppressInitialError) {
                obs = obs.Skip(2);
            }
            return obs.Subscribe(s => {
                if (string.IsNullOrWhiteSpace(s))
                    DataValidationErrors.ClearErrors(getTargetControl(view));
                else
                    DataValidationErrors.SetError(getTargetControl(view), new TinyException(s));
            });

        }

        private class TinyException : Exception {
            public TinyException(string message) : base(message) { }
            public override string ToString() {
                return Message;
            }
        }
    }
}