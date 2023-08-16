using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Infrastructure.Extensions
{
    // Custom convention-based mapping
    public class CustomPropertyTypeMapper : SqlMapper.ITypeMap
    {
        private readonly Type _type;

        public CustomPropertyTypeMapper(Type type)
        {
            _type = type;
        }

        public ConstructorInfo FindConstructor(string[] names, Type[] types)
        {
            return _type.GetConstructor(Type.EmptyTypes);
        }

        public ConstructorInfo FindExplicitConstructor()
        {
            return null;
        }

        public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
        {
            return null;
        }

        public SqlMapper.IMemberMap GetMember(string columnName)
        {
            var property = _type.GetProperties().FirstOrDefault(prop =>
                prop.GetCustomAttributes(false)
                    .OfType<ColumnAttribute>()
                    .Any(attr => attr.Name == columnName)
            );

            return property != null ? new CustomPropertyMap(property) : null;
        }

        public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, int index)
        {
            return null;
        }
    }

    public class CustomPropertyMap : SqlMapper.IMemberMap
    {
        private readonly PropertyInfo _propertyInfo;

        public CustomPropertyMap(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        public object GetMember(object obj)
        {
            return _propertyInfo.GetValue(obj);
        }

        public void SetMember(object obj, object value)
        {
            _propertyInfo.SetValue(obj, value);
        }

        public string ColumnName => _propertyInfo.Name;

        public Type MemberType => _propertyInfo.PropertyType;

        public PropertyInfo Property => _propertyInfo;

        public FieldInfo Field => null;

        public ParameterInfo Parameter => null;
    }
}
