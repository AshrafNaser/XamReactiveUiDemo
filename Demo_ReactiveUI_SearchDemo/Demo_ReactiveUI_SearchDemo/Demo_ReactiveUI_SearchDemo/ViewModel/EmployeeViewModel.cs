using Demo_ReactiveUI_SearchDemo.Model;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Xamarin.Forms;

namespace Demo_ReactiveUI_SearchDemo.ViewModel
{
    public class EmployeeViewModel : BaseViewModel
    {
        
        public EmployeeViewModel()
        {
            SelectedEmployee = new Employee();
            Employees = MockList();
            this.WhenAnyValue(vm => vm.SearchText).Throttle(TimeSpan.FromSeconds(2)).Where(x => !string.IsNullOrEmpty(x)).Subscribe(vm =>
            {
                var filteredList = Employees.Where(employee => employee.Id.Contains(SearchText)).ToList();
                Device.BeginInvokeOnMainThread(() => { Employees = new ReactiveList<Employee>(filteredList); });
            });
            this.WhenAnyValue(vm => vm.SearchText).Where(x => string.IsNullOrEmpty(x))
                .Subscribe(vm =>
            {
                Employees = MockList();
            });
          

            this.WhenAnyValue(vm => vm.SelectedEmployee).Where(x => x != null).Subscribe(vm =>
               {
                   HostScreen.Router.Navigate.Execute(new SingleEmployeeViewModel(SelectedEmployee)).Subscribe();
               });
            
        }

        private ReactiveList<Employee> _employees;

        public ReactiveList<Employee> Employees
        {
            get => _employees;
            set => this.RaiseAndSetIfChanged(ref _employees, value);
        }

        private Employee _selectedEmployee;
        public Employee SelectedEmployee
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


        public ReactiveList<Employee> MockList()
        {

            Employees = new ReactiveList<Employee>();
            Employees.Add(new Employee { Department = "IT", FullName = "Ashraf Naser", Id = "1", Title = "Software Developer" });
            Employees.Add(new Employee { Department = "IT", FullName = "Ahmed Naser", Id = "2", Title = "Software Developer" });
            Employees.Add(new Employee { Department = "IT", FullName = "Omar Naser", Id = "3", Title = "Software Developer" });
            return Employees;
        }

    }
}
