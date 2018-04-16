using System.Windows.Controls;

namespace SmartBiz.MDM.Presentation.CustomControls
{
    internal class CustomCombo : ComboBox
    {
        public CustomCombo()
            : base()
        {
            this.IsEditable = true;
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
        }
    }
}