using Demo_ReactiveUI_SearchDemo.Model;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_ReactiveUI_SearchDemo.ViewModel
{
    public class SingleEmployeeViewModel : BaseViewModel
    {
        public SingleEmployeeViewModel()
        {

        }
        public SingleEmployeeViewModel(Employee employee)
        {
            SelectedEmployee = employee;
        }


        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set => this.RaiseAndSetIfChanged(ref _selectedEmployee, value);
        }
    }
}
