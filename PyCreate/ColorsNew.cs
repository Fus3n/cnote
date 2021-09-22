using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ColorsNew
{

       public class ToolBarTheme_Light : ToolStripProfessionalRenderer
        {
            public ToolBarTheme_Light() : base(new MyColors()) { }
        }

        public class MyColors : ProfessionalColorTable
        { 
            public override Color MenuItemPressedGradientBegin
            {
                get { return Color.Transparent; }
            }

            public override Color MenuItemPressedGradientEnd
            {
                get { return Color.Transparent; }
            }

            public override Color MenuItemSelected
            {
                get { return Color.LightGray; }
            }

            public override Color MenuItemSelectedGradientBegin
            {
                get { return Color.Transparent; }
            }
            public override Color MenuItemSelectedGradientEnd
            {
                get { return Color.Transparent; }
            }
        }


    public class ToolBarTheme_Dark : ToolStripProfessionalRenderer
    {
        public ToolBarTheme_Dark() : base(new MyColorsDark()) { }
    }


    public class MyColorsDark : ProfessionalColorTable
    {
        public override Color MenuItemPressedGradientBegin
        {
            get { return Color.Transparent; }
        }

        public override Color MenuItemPressedGradientEnd
        {
            get { return Color.Transparent; }
        }

        public override Color MenuItemSelected
        {
            get { return Color.DimGray; }
        }

        public override Color MenuItemSelectedGradientBegin
        {
            get { return Color.Transparent; }
        }
        public override Color MenuItemSelectedGradientEnd
        {
            get { return Color.Transparent; }
        }
    }

}

