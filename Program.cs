// Copyright (C) Microsoft Corporation. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

using Autofac;
using HgmViewer.Classes;
using HgmViewer.Service;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HgmViewer
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();

            IContainer container = BuildContainer();
            var scope = container.BeginLifetimeScope();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(scope.Resolve<HgmViewerForm>());
        }

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<HgmViewerForm>();
            builder.RegisterType<SetupControl>();
            builder.RegisterType<SetupControl.SetupPage>();
            builder.RegisterType<PackBrowserControl>();
            builder.RegisterType<PackBrowserControl.PackBrowserPage>();
            builder.RegisterType<ModelViewerControl>();
            builder.RegisterType<ModelViewerControl.ModelViewerPage>();
            builder.RegisterType<WebViewContext>();

            builder.RegisterType<ApplicationService>().SingleInstance();
            builder.RegisterType<ConfigService>().SingleInstance();
            builder.RegisterType<PackService>().SingleInstance();
            builder.RegisterType<ExportService>().SingleInstance();
            builder.RegisterType<ViewerService>().SingleInstance();

            var container = builder.Build();
            return container;
        }
    }
}