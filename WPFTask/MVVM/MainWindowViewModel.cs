using Common.Commands;
using Common.Model;
using Common.Repositories;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Common.MVVM
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Employee> _employeeList;

        private Employee _selectedEmployee;
        private readonly IEmployeeRepository _employeeRepository;
        public MainWindowViewModel(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            LoadEmployeesAsync();
        }


        public ObservableCollection<Employee> EmployeeList
        {
            get { return _employeeList; }
            set
            {
                _employeeList = value;
                OnPropertyChanged(nameof(EmployeeList));
            }
        }

        public Employee SelectedEmployee
        {
            get { return _selectedEmployee; }
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public ICommand AddCommand => new RelayCommand(AddEmployee);
        public ICommand EditCommand => new RelayCommand(EditEmployee, CanEditOrDelete);
        public ICommand DeleteCommand => new RelayCommand(DeleteEmployee, CanEditOrDelete);
        public ICommand RefreshCommand => new RelayCommand(async () => await LoadEmployeesAsync());

        private string _searchTerm;

        public string SearchTerm
        {
            get { return _searchTerm; }
            set
            {
                if (_searchTerm != value)
                {
                    _searchTerm = value;
                    OnPropertyChanged(nameof(SearchTerm));
                }
            }
        }

        private RelayCommand _searchCommand;

        public RelayCommand SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new RelayCommand(async () =>
                {
                    await LoadEmployeesAsync(SearchTerm);
                }));
            }
        }

        private bool CanEditOrDelete() => SelectedEmployee != null;



        public event PropertyChangedEventHandler PropertyChanged;
        private async Task LoadEmployeesAsync(string searchName = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(searchName))
                {
                    EmployeeList = new ObservableCollection<Employee>(await _employeeRepository.GetEmployeesByNameAsync(searchName));
                }
                else
                {
                    EmployeeList = new ObservableCollection<Employee>(await _employeeRepository.GetEmployeesAsync());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private async void AddEmployee()
        {
            bool success = await _employeeRepository.AddEmployeeAsync(SelectedEmployee);

            if (success)
            {
                MessageBox.Show("Employee added successfully.");
                LoadEmployeesAsync();
            }
            else
            {
                MessageBox.Show("Failed to add employee. Please try again.");
            }
        }

        private async void EditEmployee()
        {
            if (SelectedEmployee != null)
            {

                bool success = await _employeeRepository.EditEmployeeAsync(SelectedEmployee);

                if (success)
                {
                    MessageBox.Show($"Employee {SelectedEmployee.Name} updated successfully.");
                    LoadEmployeesAsync();
                }
                else
                {
                    MessageBox.Show($"Failed to update {SelectedEmployee.Name}. Please try again.");
                }
            }
        }

        private async void DeleteEmployee()
        {
            if (SelectedEmployee != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete {SelectedEmployee.Name}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    bool success = await _employeeRepository.DeleteEmployeeAsync(SelectedEmployee.Id);

                    if (success)
                    {
                        MessageBox.Show($"{SelectedEmployee.Name} deleted successfully.");
                        LoadEmployeesAsync();
                    }
                    else
                    {
                        MessageBox.Show($"Failed to delete {SelectedEmployee.Name}. Please try again.");
                    }
                }
            }
        }

    }


}

