using Demo_ReactiveUI_SearchDemo.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_ReactiveUI_SearchDemo.ViewModels
{
    public class EmployeeViewModel : BaseViewModel
    {
        public EmployeeViewModel()
        {

        }
        public EmployeeViewModel(EmployeeModel employee)
        {
            SelectedEmployee = employee;
        }


        private EmployeeModel _selectedEmployee;
        public EmployeeModel SelectedEmployee
        {
            get => _selectedEmployee;
            set => this.RaiseAndSetIfChanged(ref _selectedEmployee, value);
        }
    }
}
