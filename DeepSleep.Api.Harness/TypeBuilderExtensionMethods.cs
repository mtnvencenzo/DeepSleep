namespace DeepSleep.NetCore
{
    using System;
    using System.Reflection;
    using System.Reflection.Emit;

    /// <summary>
    /// 
    /// </summary>
    public static class TypeBuilderExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeBuilder"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        internal static TypeBuilder AddProperty(this TypeBuilder typeBuilder, string propertyName, Type propertyType)
        {
            var getSetAttr = MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig;

            var prop = typeBuilder.DefineProperty(propertyName, PropertyAttributes.None, propertyType, new Type[] { });
            var underlyingField = typeBuilder.DefineField($"_{propertyName}", propertyType, FieldAttributes.Private);

            var prop_Get_Method = typeBuilder.DefineMethod($"get_{propertyName}", getSetAttr, propertyType, new Type[] { });
            var prop_Get_MethodIl = prop_Get_Method.GetILGenerator();
            prop_Get_MethodIl.Emit(OpCodes.Ldarg_0);
            prop_Get_MethodIl.Emit(OpCodes.Ldfld, underlyingField);
            prop_Get_MethodIl.Emit(OpCodes.Ret);
            prop.SetGetMethod(prop_Get_Method);

            var prop_Set_Method = typeBuilder.DefineMethod($"set_{propertyName}", getSetAttr, null, new Type[] { propertyType });
            var prop_Set_MethodIl = prop_Set_Method.GetILGenerator();
            prop_Set_MethodIl.Emit(OpCodes.Ldarg_0);
            prop_Set_MethodIl.Emit(OpCodes.Ldarg_1);
            prop_Set_MethodIl.Emit(OpCodes.Stfld, underlyingField);
            prop_Set_MethodIl.Emit(OpCodes.Ret);
            prop.SetSetMethod(prop_Set_Method);

            return typeBuilder;
        }
    }
}
