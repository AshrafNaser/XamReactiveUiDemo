# ReactiveUI with Xamarin.Forms

[ReactiveUI](https://github.com/reactiveui/reactiveui) is a composable, cross-platform model-view-viewmodel framework inspired by functional reactive programming and we are going to see this powerful framework intact with Xamarin.Forms.
## What we built
**Search App** searches on a list of employees using their IDs,
that also you can select an employee from the list to show his full information in a new page. 

this sample demonstrates several features of the framework to get you ready to build an MVVM based application using [ReactiveUI](https://github.com/reactiveui/reactiveui) 
## ReactiveUI features used in this demo
* [ReactiveObject](https://reactiveui.net/api/reactiveui/reactiveobject/)  
* [ReactiveCommand](https://reactiveui.net/api/reactiveui/reactivecommand/)
* [Data Binding](https://reactiveui.net/docs/handbook/data-binding/)
* [OAPH: ObservableAsPropertyHelper](https://reactiveui.net/docs/handbook/oaph/)
* [Routing](https://reactiveui.net/docs/handbook/routing/)
* [Collections: Reactive List ](https://reactiveui.net/docs/handbook/collections/reactive-list)
* [View Models](https://reactiveui.net/docs/handbook/view-models/)
* [Views:View Location](https://reactiveui.net/docs/handbook/views/view-location)
* [When Activated](https://reactiveui.net/docs/handbook/when-activated/)
* [When Any](https://reactiveui.net/docs/handbook/when-any/)

 # Dependencies
  **inorder to start using ReactiveUI features you have to install the following NuGet packages**
 * [reactiveui](https://www.nuget.org/packages/reactiveui) 
 * [ReactiveUI.Events.XamForms](https://www.nuget.org/packages/ReactiveUI.Events.XamForms/) 
 * [ReactiveUI.XamForms](https://www.nuget.org/packages/ReactiveUI.XamForms/)
 * [Splat](https://www.nuget.org/packages/Splat/)
 
# Application Skeleton 
#### The shared project contains the following
- Models (Domain objects)
  * EmployeeModel.cs
- ViewModels (View logic)
  * BaseViewModel.cs
  * EmployeeViewModel.cs
  * EmployeesViewModel.cs 
- Views (User Interface (Xaml))
  * BaseContentPage.cs  
  * EmployeePage  (ContentPage C#/Xaml)
  * EmployeesPage (ContentPage C#/Xaml)
 - AppBootstrapper.cs (is where we will perform all the initial setup for our app such as registering services and routing to perform navigation.)
### now let's tackle it bit by bit
# Models
 EmployeeModel represents the employee data as follows

```C#
        public string Id { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; } 
        public string Department { get; set; }
```
# ViewModels 

* All our **ViewModels** will be going to inherit from **BaseViewModel**,
  that will be going to inherit and implement the following
  
  - [ReactiveObject](https://reactiveui.net/api/reactiveui/reactiveobject/)  
  - [IRoutableViewModel](https://reactiveui.net/api/reactiveui/iroutableviewmodel/)  
  - [ISupportsActivation](https://reactiveui.net/api/reactiveui/isupportsactivation/)  
  

```C#
  public class BaseViewModel : ReactiveObject, IRoutableViewModel, ISupportsActivation
    {  
        //ReactiveObject: implements INotifyPropertyChanged. 
        //In addition, provides Changing and Changed Observables to monitor object changes.

       //IRoutableViewModel: implement this interface  to be able to access IScreen from the ViewModel
       //and AppBootstrapper which controls the Application Navigation that has the implementation
       //for IScreen interface

        public BaseViewModel(IScreen hostScreen = null)
        { 
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
        }
        
        public IScreen HostScreen
        {
            get;
            protected set;
        }
        
      //UrlPathSegment: A string token representing the current ViewModel, 
      //such as 'EmployeesViewModel' or 'EmployeeViewModel' 
      
        public string UrlPathSegment
        {
            get;
            protected set;
        }
        
        //ISupportsActivation: implement this interface to get this.WhenActivated 
        //and have a way to track ViewModel disposables.
        //Instantiate the Activator, then call WhenActivated on 
        //your class to register what you want to happen when the 
        //View is activated  

        public ViewModelActivator Activator
        {
            get { return viewModelActivator; }
        }

        protected readonly ViewModelActivator viewModelActivator = new ViewModelActivator();
    }
```

   * **EmployeesViewModel** as we mentioned previously
    all  the **ViewModels** will be inheriting from **BaseViewModel** and gonna be responsible for
                            
      
      *   Display list of employees
      *   filter the list based on the employee Id 
       

```C#
  public class EmployeesViewModel : BaseViewModel
   {
        
        public EmployeesViewModel()
        {
            //objects instantiation
            SelectedEmployee = new EmployeeModel();
            Employees = MockList();

               //state is continually changing in response to user actions and application events so 
               //this.WhenAnyValue: help you work with properties as an observable stream
               //in the following example this.WhenAnyValue will watch the  properties 
               //whenever it chnages  it will gonna perform a certain action. 

            this.WhenAnyValue(vm => vm.SearchText).Throttle(TimeSpan.FromMilliseconds(100))
            .Where(x => !string.IsNullOrEmpty(x)).Subscribe(vm =>
            {
                var filteredList = Employees.Where(employee => employee.Id.Contains(SearchText))
                .ToList();
                Device.BeginInvokeOnMainThread(() =>
                { Employees = new ReactiveList<EmployeeModel>(filteredList); });
            });

            this.WhenAnyValue(vm => vm.SearchText).Where(x => string.IsNullOrEmpty(x))
                .Subscribe(vm =>
            {
                Employees = MockList();
            });
           
            this.WhenAnyValue( vm => vm.SelectedEmployee).
            Where(x => x != null).Subscribe(vm =>  
            { 
                HostScreen.Router.Navigate.Execute(new EmployeeViewModel(SelectedEmployee))
                .Subscribe();
             }
             );
            
        }
          //should be used in any place that you would normally use a List or ObservableCollection,
         // as it has additional useful Rx features.
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
            //We need to get notified when the value changes
            //RaiseAndSetIfChanged : implements a Setter for a read-write property using CallerMemberName
            //to raise the notification and the ref to the backing field to set the property. 
            set => this.RaiseAndSetIfChanged(ref _selectedEmployee, value);
        }

        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set => this.RaiseAndSetIfChanged(ref _searchText, value);
        }

       //Supply our List with initial data
        public ReactiveList<EmployeeModel> MockList()
        {

            Employees = new ReactiveList<EmployeeModel>();
            Employees.Add(new EmployeeModel { Department = "IT", FullName = "Ashraf Naser", 
            Id = "1", Title = "Software Developer" });

            Employees.Add(new EmployeeModel { Department = "IT", FullName = "Ahmed Naser", 
            Id = "2", Title = "Software Developer" });

            Employees.Add(new EmployeeModel { Department = "IT", FullName = "Omar Naser",
             Id = "3", Title = "Software Developer" });

            return Employees;
        }

    }
```

   - EmployeeViewModel and gonna be responsible for.. 
   
        *   display the data of a single employee.
 
      
      
```C#
  public class EmployeeViewModel : BaseViewModel
    {
        public EmployeeViewModel()
        {

        }
        public EmployeeViewModel(EmployeeModel employee)
        {
            //setting the Selected Employee value with the 
            //employee object coming from the ctor throughout the navigation.
            SelectedEmployee = employee;
        }


        private EmployeeModel _selectedEmployee;
        public EmployeeModel SelectedEmployee
        {
            get => _selectedEmployee;
            //We need to get notified when the value changes
            //RaiseAndSetIfChanged : implements a Setter for a read-write property using CallerMemberName
            //to raise the notification and the ref to the backing field to set the property.
            set => this.RaiseAndSetIfChanged(ref _selectedEmployee, value);
        }
    }
```
# Views
First things first.
 * create a class called **BaseContentPage** that inherits from **ReactiveContentPage**
this class going to be responsible for associating ViewModel with the View.

```C#
  //TViewModel is the type of the ViewModel associated with the view 
   public class BaseContentPage<TViewModel> : ReactiveContentPage<TViewModel> where TViewModel : class
    {

    }
```
# Content Pages 
- **Content Page: is a typical c# partial class writen with c# and Xaml
(Xaml is a markup language used to create an application's user interface  and will be compiled to c# at runtime)**

- **both c# and Xaml are going to inherit from BaseContentPage**

## EmployeesPage.xaml 

```XML
<?xml version="1.0" encoding="utf-8" ?>
<ui:BaseContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="Demo_ReactiveUI_SearchDemo.Views.EmployeesPage"
             xmlns:ui="clr-namespace:Demo_ReactiveUI_SearchDemo.Views"
             xmlns:rxui="clr-namespace:ReactiveUI.XamForms;assembly=ReactiveUI.XamForms"
             xmlns:vm="clr-namespace:Demo_ReactiveUI_SearchDemo.ViewModels"
             x:TypeArguments="vm:EmployeesViewModel">
    <ContentPage.Content>
       <!--this view for binding the view with Employees list and the
        Search Text Queries from the EmployeesViewModel-->
        <Grid Padding="5" Grid.Row="2">
            <StackLayout> 
                <Entry HorizontalOptions="FillAndExpand" Placeholder="Enter employee Id"   
                       Text="{Binding SearchText,Mode=TwoWay}"/>
                <ListView  SelectedItem="{Binding SelectedEmployee,Mode=TwoWay}"
                 ItemsSource="{Binding Employees}" BackgroundColor="Transparent" 
                     SeparatorVisibility="None" RowHeight="100">
                    <ListView.ItemTemplate>
                        <DataTemplate>
                            <ViewCell>
                                <Grid>
                                    <StackLayout>
                                        <Label Text="{Binding Title}" />
                                        <Label Text="{Binding Department}" />
                                        <Label Text="{Binding FullName}" />
                                        <Label Text="{Binding Id}" />
                                    </StackLayout>
                                </Grid>
                            </ViewCell>
                        </DataTemplate>
                    </ListView.ItemTemplate>
                </ListView>
            </StackLayout>
        </Grid>
    </ContentPage.Content>
</ui:BaseContentPage>
```
## EmployeesPage.xaml.cs
```C# 
     [XamlCompilation(XamlCompilationOptions.Compile)]
     //here wiring the View With ViewModel as we mentioned before.
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
```

## EmployeePage.xaml 

```XML
<?xml version="1.0" encoding="utf-8" ?>
<ui:BaseContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="Demo_ReactiveUI_SearchDemo.Views.EmployeePage"
             xmlns:ui="clr-namespace:Demo_ReactiveUI_SearchDemo.Views"
             xmlns:rxui="clr-namespace:ReactiveUI.XamForms;assembly=ReactiveUI.XamForms"
             xmlns:vm="clr-namespace:Demo_ReactiveUI_SearchDemo.ViewModels"
             x:TypeArguments="vm:EmployeeViewModel">
    <ContentPage.Content>
      <!--this view for binding the view with SelectedEmployee from 
        the EmployeeViewModel to show the selected employee full details -->
        <StackLayout>
            <Label Text="{Binding SelectedEmployee.Id}"
                VerticalOptions="CenterAndExpand" 
                HorizontalOptions="CenterAndExpand" />
            <Label Text="{Binding SelectedEmployee.Title}"
                VerticalOptions="CenterAndExpand" 
                HorizontalOptions="CenterAndExpand" />
            <Label Text="{Binding SelectedEmployee.FullName}"
                VerticalOptions="CenterAndExpand" 
                HorizontalOptions="CenterAndExpand" />
            <Label Text="{Binding SelectedEmployee.Department}"
                VerticalOptions="CenterAndExpand" 
                HorizontalOptions="CenterAndExpand" />
        </StackLayout>
    </ContentPage.Content>
</ui:BaseContentPage>

```
## EmployeePage.xaml.cs
```C# 
    [XamlCompilation(XamlCompilationOptions.Compile)]
     //here wiring the View With ViewModel as we mentioned before. 
	 public partial class EmployeePage : BaseContentPage<EmployeeViewModel>
    {
		public EmployeePage ()
		{
			InitializeComponent ();
		}
	}
```
## last but not least
# AppBootsrapper
 
**at the root of our application we are goinng to find the AppBootsrapper class**
**where we will perform all the initial setup for our app such as registering services and routing to perform navigation**,
**AppBootsrapper will inherit and implement [IScreen](https://reactiveui.net/api/reactiveui/iscreen/)**
 
```C# 
    public class AppBootsrapper : IScreen
    {
        public RoutingState Router { get; protected set; }

        public AppBootsrapper()
        {
            Router = new RoutingState();

            ///You must register This as IScreen to represent your app's main screen
            Locator.CurrentMutable.RegisterConstant(this, typeof(IScreen));
 
            //Register the views  
            Locator.CurrentMutable.Register(() => new EmployeesPage(), typeof(IViewFor<EmployeesViewModel>));
            Locator.CurrentMutable.Register(() => new EmployeePage(), typeof(IViewFor<EmployeeViewModel>));

            this.Router.NavigateAndReset.Execute(new EmployeesViewModel()).Subscribe();
        }

        public Page CreateMainPage()
        { 
           //RoutedViewHost is a ReactiveNavigationController
           //that monitors its RoutingState and keeps the navigation stack in line with it
            return new ReactiveUI.XamForms.RoutedViewHost();
        }
    }
```
# at the very end  
in the constructor of App.xaml.cs do not forget
to use CreateMainPage() Method we declared previously at AppBootsrapper.
  
```C# 
   public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();
            var bootstrapper = new AppBootsrapper();
            MainPage = bootstrapper.CreateMainPage(); 
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
```

## It is time we drew this to a close, but I have to mention some blogs that helped me a lot
 - [Introduction to ReactiveUI with Xamarin.Forms - Part 1](https://ramonesteban78.github.io/es/tutorials/introduccion-reactiveui-xamarin-forms.html)
 - [Introduction to ReactiveUI with Xamarin.Forms - Part 2](https://ramonesteban78.github.io/es/tutorials/introduccion-reactiveui-xamarin-forms.2.html)
 
 ## Acknowledgments

* Xamarin.Forms Vesrsion 3.1.0.697729
* Visual Studio 2017 version 15.7
* Code Sharing Strategy: .NET Standard

 ### License

This project is licensed under the MIT License - see the [LICENSE.md](https://github.com/AshrafNaser/XamReactiveUiDemo/blob/master/LICENSE) file for details



 #    Happy Coding! :blush: :v:




