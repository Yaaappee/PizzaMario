﻿using MvvmDialogs;
using System;

namespace PizzaSanMorino.ViewModels
{
    class AboutViewModel : ViewModelBase, IModalDialogViewModel
    {
        public bool? DialogResult { get { return false; } }

        public string Content
        {
            get
            {
                return "PizzaSanMorino" + Environment.NewLine +
                        "Created by isoko" + Environment.NewLine +
                        "Address" + Environment.NewLine +
                        "2018";
            }
        }

        public string VersionText
        {
            get
            {
                var version1 = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                // For external assemblies
                // var ver2 = typeof(Assembly1.ClassOfAssembly1).Assembly.GetName().Version;
                // var ver3 = typeof(Assembly2.ClassOfAssembly2).Assembly.GetName().Version;

                return "PizzaSanMorino v" + version1.ToString();
            }
        }
    }
}
