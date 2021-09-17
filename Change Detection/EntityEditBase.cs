using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using ChangeDetection.Properties;

namespace ChangeDetection
{
    public abstract class EntityEditBase<TDto, KEntityEditBase>
    {
        public event EventHandler OnChange;

        public TDto ExtendedDto { get; protected set; }
        public bool IsSavable { get => (IsDirty || IsNew) && IsValid; }
        public bool IsDirty { get; protected set; }
        public bool IsValid { get; protected set; } = false;
        public bool IsNew { get; protected set; } = false;

        public EntityEditBase(Guid companyId)
        {
            SetupProperties();
            IsNew = true;
        }

        public EntityEditBase(TDto existingModel)
        {
            ExtendedDto = existingModel;
            SetupProperties();
            IsNew = false;
        }

        #region RequiredImplementations
        protected abstract void SetupProperties();
        #endregion RequiredImplementations

        #region ProtectedHelpers
        protected void EmitChange(object sender, EventArgs args)
        {
            IsDirty = true;
            OnChange?.Invoke(sender, args);
        }
        #endregion ProtectedHelpers

        #region PropertyClasses
        public class EditProperty<TFieldTest> : HelperEditProperty<TDto, TFieldTest>
        {
            public EditProperty(Expression<Func<TDto, TFieldTest>> expression, TDto editModel) : base(expression, editModel)
            {

            }
        }
        #endregion PropertyClasses 

        #region PropertyHelpers
        protected EditProperty<TAnyFieldType> AssignProp<TAnyFieldType>(Expression<Func<TDto, TAnyFieldType>> expression)
        {
            var property = new EditProperty<TAnyFieldType>(expression, ExtendedDto);

            property.ValueChanged += (sender, args) =>
            {
                EmitChange(sender, null);
            };

            return property;
        }
        #endregion PropertyHelpers
    }
}
