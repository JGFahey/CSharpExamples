using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ObjectCreation;

namespace UnitTests
{
    [TestClass]
    public class FastObjectAllocator
    {
        static object CreateNativeObject() => new object();
        static T CreateDynamicObject<T>() where T : new() => new T();
        static object CreateReflectiveObject(Type objectType) => Activator.CreateInstance(objectType);
        static T CreateFastObject<T>() where T : new() => FastObjectAllocator<T>.New();

        [TestMethod]
        public void TestObjectCreation()
        {
            int sets = 5;
            int repititions = 100000;
            
            TestTools.Measure("Create Native Objects", sets, repititions, () => { CreateNativeObject(); });
            TestTools.Measure("Create Dynamic Objects", sets, repititions, () => { CreateDynamicObject<object>(); });
            TestTools.Measure("Create Reflective Objects", sets, repititions, () => { CreateReflectiveObject(typeof(Object)); });
            TestTools.Measure("Create Fast Objects", sets, repititions, () => { CreateFastObject<object>(); });
        }
    }
}
