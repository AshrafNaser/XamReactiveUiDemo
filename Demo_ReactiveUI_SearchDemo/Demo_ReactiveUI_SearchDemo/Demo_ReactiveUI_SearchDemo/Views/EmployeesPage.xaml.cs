using Demo_ReactiveUI_SearchDemo.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Demo_ReactiveUI_SearchDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeesPage : BaseContentPage<EmployeesViewModel>
    {
        public EmployeesPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
           
        }

    }
}