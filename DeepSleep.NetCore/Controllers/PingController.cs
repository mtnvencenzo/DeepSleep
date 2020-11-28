namespace DeepSleep.NetCore.Controllers
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Reflection.Emit;

    /// <summary>
    /// 
    /// </summary>
    internal class PingController
    {
        private static Type pingModelType;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal Task<dynamic> Ping()
        {
            var pingModelType = PingTypeDescriptor();

            dynamic pingModel = Activator.CreateInstance(pingModelType);
            pingModel.Ping = "Pong";

            return Task.FromResult(pingModel);
        }

        private static Type PingTypeDescriptor()
        {
            if (pingModelType == null)
            {
                var assemblyName = new AssemblyName("PingDynamicAssembly");

                var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

                var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

                var typeBuilder = moduleBuilder.DefineType(
                    "PingModel",
                    (
                        TypeAttributes.Public |
                        TypeAttributes.Sealed |
                        TypeAttributes.SequentialLayout |
                        TypeAttributes.Serializable
                    ),
                    typeof(ValueType)
                );

                typeBuilder.AddProperty("Ping", typeof(string));

                pingModelType = typeBuilder.CreateType();
            }

            return pingModelType;
        }
    }
}
