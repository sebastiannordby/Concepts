using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ChangeDetection.Properties
{
    public class EditListProperty<TDto, KEditModel> : BindingList<KEditModel>
        where KEditModel : EntityEditBase<TDto, KEditModel>
    {
        public event EventHandler ChildChanged;

        public EditListProperty(List<TDto> values)
        {
            SetupSubscriptionToProperties(values);
        }

        public void SetupSubscriptionToProperties(List<TDto> values)
        {
            if(values != null)
            {
                foreach (var value in values)
                {
                    var editModel = (KEditModel) Activator.CreateInstance(typeof(KEditModel), value);

                    if (editModel != null)
                    {
                        editModel.OnChange += (sender, args) =>
                        {
                            ChildChanged.Invoke(sender, args);
                        };

                        Add(editModel);
                    }
                }
            }
        }

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                var item = this[e.NewIndex];

                item.OnChange += (sender, args) => 
                {
                    ChildChanged.Invoke(sender, args);
                };
            }
        }
    }
}
