namespace DeepSleep.NetCore.Controllers
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Reflection.Emit;

    /// <summary>
    /// 
    /// </summary>
    internal class EnnvironmentController
    {
        private Type environmentModelType;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal Task<ApiResponse> Env()
        {
            var environmentModelType = CreateEnvironmentModel();

            dynamic environmentModel = Activator.CreateInstance(environmentModelType);

            environmentModel.Is64BitOperatingSystem = Environment.Is64BitOperatingSystem;
            environmentModel.Is64BitProcess = Environment.Is64BitProcess;
            environmentModel.MachineName = Environment.MachineName;
            environmentModel.OSVersion = Environment.OSVersion.VersionString;
            environmentModel.TickCount = Environment.TickCount;
            environmentModel.ProcessorCount = Environment.ProcessorCount;
            environmentModel.ClrVersion = Environment.Version.ToString();
            environmentModel.WorkingSet = Environment.WorkingSet;

            return Task.FromResult(new ApiResponse
            {
                StatusCode = 200,
                Body = environmentModel
            });
        }

        private Type CreateEnvironmentModel()
        {
            if (this.environmentModelType == null)
            {
                var assemblyName = new AssemblyName("EnvironmentDynamicAssembly");

                var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);

                var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);

                var typeBuilder = moduleBuilder.DefineType(
                    "EnvironmentModel",
                    (
                        TypeAttributes.Public |
                        TypeAttributes.Sealed |
                        TypeAttributes.SequentialLayout |
                        TypeAttributes.Serializable
                    ),
                    typeof(ValueType)
                );

                typeBuilder.DefineField("Is64BitOperatingSystem", typeof(bool), FieldAttributes.Public);
                typeBuilder.DefineField("Is64BitProcess", typeof(bool), FieldAttributes.Public);
                typeBuilder.DefineField("MachineName", typeof(string), FieldAttributes.Public);
                typeBuilder.DefineField("OSVersion", typeof(string), FieldAttributes.Public);
                typeBuilder.DefineField("TickCount", typeof(int), FieldAttributes.Public);
                typeBuilder.DefineField("ProcessorCount", typeof(int), FieldAttributes.Public);
                typeBuilder.DefineField("ClrVersion", typeof(string), FieldAttributes.Public);
                typeBuilder.DefineField("WorkingSet", typeof(long), FieldAttributes.Public);

                this.environmentModelType = typeBuilder.CreateType();
            }

            return this.environmentModelType;
        }
    }
}
