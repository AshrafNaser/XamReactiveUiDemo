using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_ReactiveUI_SearchDemo.ViewModels
{
    public class BaseViewModel : ReactiveObject, IRoutableViewModel, ISupportsActivation
    {
        public BaseViewModel(IScreen hostScreen = null)
        {
             
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
        }
        public string UrlPathSegment
        {
            get;
            protected set;
        }

        public IScreen HostScreen
        {
            get;
            protected set;
        }
        public ViewModelActivator Activator
        {
            get { return viewModelActivator; }
        }

        protected readonly ViewModelActivator viewModelActivator = new ViewModelActivator();

    }
}
