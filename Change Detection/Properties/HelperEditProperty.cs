using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ChangeDetection.Properties
{
    public class HelperEditProperty<TParentModel, KFieldType>
    {
        public event EventHandler<KFieldType> ValueChanged;

        private Expression<Func<TParentModel, KFieldType>> _expression;
        private TParentModel _editModel;

        public KFieldType Value
        {
            get
            {
                return GetValue();
            }

            set
            {
                SetValueOfField(value);
            }
        }

        public HelperEditProperty(Expression<Func<TParentModel, KFieldType>> expression,
            TParentModel editModel)
        {
            _expression = expression;
            _editModel = editModel;
        }

        private void SetValueOfField(KFieldType value)
        {
            if (_editModel != null)
            {
                var propertyPath =  _expression.GetPropertyPath();

                _editModel.SetPropertyValue(propertyPath, value);
                ValueChanged.Invoke(this, value);
            }
        }

        private KFieldType GetValue()
        {
            if (_editModel != null)
            {
                var propertyPath = _expression.GetPropertyPath();
                var path = string.Join(".", propertyPath);
             
                return (KFieldType) _editModel.GetPropertyValue(path);
            }

            return default(KFieldType);
        }

        public override string ToString()
        {
            return Value?.ToString();
        }
    }
}
