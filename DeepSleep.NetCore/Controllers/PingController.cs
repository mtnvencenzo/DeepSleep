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
        private Type pingModelType;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal Task<ApiResponse> Ping()
        {
            var pingModelType = this.CreatePingModelType();

            dynamic pingModel = Activator.CreateInstance(pingModelType);
            pingModel.Ping = "Pong";

            return Task.FromResult(new ApiResponse
            {
                StatusCode = 200,
                Body = pingModel
            });
        }

        private Type CreatePingModelType()
        {
            if (this.pingModelType == null)
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

                typeBuilder.DefineField("Ping", typeof(string), FieldAttributes.Public);

                this.pingModelType = typeBuilder.CreateType();
            }

            return this.pingModelType;
        }
    }
}
