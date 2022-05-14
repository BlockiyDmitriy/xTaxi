﻿using Xamarin.Forms;

namespace xTaxi.Client.Utils
{
    public static class DependencyResolver
    {
        public static T Get<T>() where T : class
        {
            return DependencyService.Get<T>();
        }

        public static void Register<T>() where T : class
        {
            DependencyService.Register<T>();
        }
    }
}
