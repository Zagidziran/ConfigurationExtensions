namespace Zagidziran.ConfigurationExtensions.Exceptions
{
    using System;
    using Zagidziran.ConfigurationExtensions.Transformations;

    public class TransformationResultNotSupportedException : Exception
    {
        public TransformationResultNotSupportedException(Type resultType)
            : base($"{resultType.Name} cannot be used to build configuration.")
        { 
        }

        public TransformationResultNotSupportedException(ITransformationResult transformationResult)
            : base($"{GetResultTypeName(transformationResult)} cannot be interpolated.")
        {
        }

        private static string GetResultTypeName(ITransformationResult transformationResult)
        {
            return transformationResult switch
            {
                NullTransformationResult _ => "Null",
                DictionaryTransformationResult _ => "Dictionary",
                _ => transformationResult.GetType().Name,
            };
        }
    }
}
