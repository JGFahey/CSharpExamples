using System;
using System.Reflection;
using System.Reflection.Emit;

namespace ObjectCreation
{
    public static class FastObjectAllocator<T> where T : new()
    {
        public static Func<T> ObjectCreator = null;
        public static T New() => ObjectCreator();

        static FastObjectAllocator()
        {
            Type objectType = typeof(T);
            ConstructorInfo defaultConstructor = objectType.GetConstructor(new Type[] { });

            DynamicMethod method = new DynamicMethod(
                name: string.Format("_{0:N}", Guid.NewGuid()),
                returnType: objectType,
                parameterTypes: null
            );

            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Newobj, defaultConstructor);
            il.Emit(OpCodes.Ret);

            ObjectCreator = method.CreateDelegate(typeof(Func<T>)) as Func<T>;
        }
    }
}
