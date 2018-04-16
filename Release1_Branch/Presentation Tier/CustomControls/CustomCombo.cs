using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SmartBiz.MDM.Presentation.CustomControls
{
    class CustomCombo:ComboBox
    {
       // public Object CustomSelectedItem { get; set; }
      
        public CustomCombo():base()
        {
            this.IsEditable = true;
           
            
        }
        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {         
           
        
        }
      
       
      
    }
}
