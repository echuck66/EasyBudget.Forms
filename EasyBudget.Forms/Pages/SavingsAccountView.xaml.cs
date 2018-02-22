﻿using System;
using System.Collections.Generic;
using EasyBudget.Business;
using EasyBudget.Business.ViewModels;
using EasyBudget.Models;
using Xamarin.Forms;

namespace EasyBudget.Forms.Pages
{
    public partial class SavingsAccountView : ContentPage
    {

        public SavingsAccountView()
        {
            InitializeComponent();
        }

        public async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
