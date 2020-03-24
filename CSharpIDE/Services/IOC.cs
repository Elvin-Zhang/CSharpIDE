using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace CSharpIDE.Services
{
    public class IOC
    {
        public IOC Register<TImplementation, TInterface>()
        {
            builder.RegisterType<TImplementation>().As<TInterface>();
            return Reference;
        }

        public IOC Register<TImplementation>()
        {
            builder.RegisterType<TImplementation>();
            return Reference;
        }

        public void Build()
        {
            container = builder.Build();
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }

        private IOC()
        {
            builder = new ContainerBuilder();
        }

        private static IOC reference = new IOC();
        public static IOC Reference { get { return reference; } }

        private IContainer container;
        private ContainerBuilder builder;
    }
}
