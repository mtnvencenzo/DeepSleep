namespace DeepSleep.NetCore.Controllers
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Reflection.Emit;
    using DeepSleep.NetCore;

    /// <summary>
    /// 
    /// </summary>
    internal class EnnvironmentController
    {
        private static Type environmentModelType;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal Task<dynamic> Env()
        {
            var environmentModelType = EnvTypeDescriptor();

            dynamic environmentModel = Activator.CreateInstance(environmentModelType);

            environmentModel.Is64BitOperatingSystem = Environment.Is64BitOperatingSystem;
            environmentModel.Is64BitProcess = Environment.Is64BitProcess;
            environmentModel.MachineName = Environment.MachineName;
            environmentModel.OSVersion = Environment.OSVersion.VersionString;
            environmentModel.TickCount = Environment.TickCount;
            environmentModel.ProcessorCount = Environment.ProcessorCount;
            environmentModel.ClrVersion = Environment.Version.ToString();
            environmentModel.WorkingSet = Environment.WorkingSet;

            return Task.FromResult(environmentModel);
        }

        private static Type EnvTypeDescriptor()
        {
            if (environmentModelType == null)
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

                typeBuilder
                    .AddProperty("Is64BitOperatingSystem", typeof(bool))
                    .AddProperty("Is64BitProcess", typeof(bool))
                    .AddProperty("MachineName", typeof(string))
                    .AddProperty("OSVersion", typeof(string))
                    .AddProperty("TickCount", typeof(int))
                    .AddProperty("ProcessorCount", typeof(int))
                    .AddProperty("ClrVersion", typeof(string))
                    .AddProperty("WorkingSet", typeof(long));

                environmentModelType = typeBuilder.CreateType();
            }

            return environmentModelType;
        }
    }
}
