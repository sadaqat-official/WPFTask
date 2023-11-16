using Common.MVVM;
using Common.Repositories;
using System.Net.Http;
using System.Windows;

namespace WPFTask
{

    public partial class MainWindow : Window
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly HttpClient _httpClient;
        private readonly MainWindowViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            
            var httpClient = new HttpClient();
            var employeeRepository = new EmployeeRepository(httpClient);
            _viewModel = new MainWindowViewModel(employeeRepository);
            DataContext = _viewModel;
        }
    }
}
