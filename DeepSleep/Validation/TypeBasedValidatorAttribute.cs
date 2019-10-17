namespace DeepSleep.Validation
{
    using System;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class TypeBasedValidatorAttribute : Attribute
    {
        #region Constructors & Initialization

        /// <summary>Initializes a new instance of the <see cref="TypeBasedValidatorAttribute"/> class.</summary>
        /// <param name="validator">The validator.</param>
        public TypeBasedValidatorAttribute(Type validator)
        {
            ValidatorType = validator;
        }

        #endregion

        #region Public Properties

        /// <summary>Gets the type of the validator.</summary>
        /// <value>The type of the validator.</value>
        public Type ValidatorType
        {
            get;
            private set;
        }

        /// <summary>Gets or sets the order.</summary>
        /// <value>The order.</value>
        public int Order { get; set; }

        #endregion
    }
}
