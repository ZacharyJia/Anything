using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Reflection;

namespace MyRun
{
    class Image
    {
        public static Bitmap getImage(string name)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var names = asm.GetManifestResourceNames();
            var stream = asm.GetManifestResourceStream(name);
            Bitmap bmp = new Bitmap(stream);
            return bmp;
        }

    }
}
