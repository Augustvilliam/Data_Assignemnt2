﻿using MobileApp.Pages;

namespace MobileApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ProjectPage), typeof(ProjectPage));
        }
    }
}
