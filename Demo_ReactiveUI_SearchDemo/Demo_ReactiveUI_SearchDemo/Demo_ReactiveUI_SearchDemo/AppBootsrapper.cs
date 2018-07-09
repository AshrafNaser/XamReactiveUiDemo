using Demo_ReactiveUI_SearchDemo.ViewModel;
using Demo_ReactiveUI_SearchDemo.Views;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Demo_ReactiveUI_SearchDemo
{
    public class AppBootsrapper : ReactiveObject, IScreen
    {
        public RoutingState Router { get; protected set; }

        public AppBootsrapper()
        {
            Router = new RoutingState();

            ///You much register This as IScreen to represent your app's main screen
            Locator.CurrentMutable.RegisterConstant(this, typeof(IScreen));

            //We register the service in the locator
             

            //Register the views  
            Locator.CurrentMutable.Register(() => new EmployeesPage(), typeof(IViewFor<EmployeeViewModel>));
            Locator.CurrentMutable.Register(() => new EmployeePage(), typeof(IViewFor<SingleEmployeeViewModel>));

            this.Router.NavigateAndReset.Execute(new EmployeeViewModel()).Subscribe();
        }

        public Page CreateMainPage()
        {
           
             
            return new ReactiveUI.XamForms.RoutedViewHost();
        }
    }
}
