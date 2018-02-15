using System;
using System.Collections.Generic;
using EasyBudget.Business;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class MainPage : ContentPage
    {
        EasyBudgetDataService ds;

        public MainPage()
        {
            InitializeComponent();
            ds = EasyBudgetDataService.Instance;


        }
    }
}
