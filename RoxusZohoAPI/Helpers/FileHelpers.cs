using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoxusZohoAPI.Helpers
{
    public class FileHelpers
    {

        public static string GenerateFileName(string fileName)
        {
            string newFileName = string.Empty;
            int idx = fileName.LastIndexOf('.');

            if (idx != -1)
            {
                string name = fileName.Substring(0, idx);
                string extension = fileName.Substring(idx + 1);

                newFileName = Guid.NewGuid() + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + extension;
            }
            return newFileName;
        }

    }

}
