# EasyBudget.Forms
Xamarin.Forms app for EasyBudget app using Sqlite-net-pcl, a simple DI container and OxyPlot

## Notes
1) This application is very much a Work In Progress and utilizes Sqlite-net-pcl and OxyPlot nuget packages.
2) A DataServiceHelper registered in the Android and iOS projects to provide the EasyBudgetDataService class with the local path to the location where the Sqlite database is located. 
3) A class in each platform-specific project named FileAccessHelper provides the platform-specific path.
4) The EasyBudgetDataService class provides methods to get instances of the View Model classes and each View Model class provides methods to Save changes or Delete referenced data objects.

## Screenshots
No screenshots yet...
