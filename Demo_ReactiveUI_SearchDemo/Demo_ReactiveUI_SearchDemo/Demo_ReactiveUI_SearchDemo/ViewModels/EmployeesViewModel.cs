using Demo_ReactiveUI_SearchDemo.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Xamarin.Forms;

namespace Demo_ReactiveUI_SearchDemo.ViewModels
{
    public class EmployeesViewModel : BaseViewModel
    {
        
        public EmployeesViewModel()
        {
            SelectedEmployee = new EmployeeModel();
            Employees = MockList();
            this.WhenAnyValue(vm => vm.SearchText).Throttle(TimeSpan.FromMilliseconds(100)).Where(x => !string.IsNullOrEmpty(x)).Subscribe(vm =>
            {
                var filteredList = Employees.Where(employee => employee.Id.Contains(SearchText)).ToList();
                Device.BeginInvokeOnMainThread(() => { Employees = new ReactiveList<EmployeeModel>(filteredList); });
            });
            this.WhenAnyValue(vm => vm.SearchText).Where(x => string.IsNullOrEmpty(x))
                .Subscribe(vm =>
            {
                Employees = MockList();
            });
          

            this.WhenAnyValue(vm => vm.SelectedEmployee).Where(x => x != null).Subscribe(vm =>
               {
                   HostScreen.Router.Navigate.Execute(new EmployeeViewModel(SelectedEmployee)).Subscribe();
               });
            
        }

        private ReactiveList<EmployeeModel> _employees;

        public ReactiveList<EmployeeModel> Employees
        {
            get => _employees;
            set => this.RaiseAndSetIfChanged(ref _employees, value);
        }

        private EmployeeModel _selectedEmployee;
        public EmployeeModel SelectedEmployee
        {
            get => _selectedEmployee;
            set => this.RaiseAndSetIfChanged(ref _selectedEmployee, value);
        }

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }


        public ReactiveList<EmployeeModel> MockList()
        {

            Employees = new ReactiveList<EmployeeModel>();
            Employees.Add(new EmployeeModel { Department = "IT", FullName = "Ashraf Naser", Id = "1", Title = "Software Developer" });
            Employees.Add(new EmployeeModel { Department = "IT", FullName = "Ahmed Naser", Id = "2", Title = "Software Developer" });
            Employees.Add(new EmployeeModel { Department = "IT", FullName = "Omar Naser", Id = "3", Title = "Software Developer" });
            return Employees;
        }

    }
}
