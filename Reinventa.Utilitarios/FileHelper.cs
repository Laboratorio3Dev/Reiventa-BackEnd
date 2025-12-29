using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reinventa.Utilitarios
{
    public static class FileHelper
    {
        public static string DetectContentType(byte[] file)
        {
            if (file.Length < 4) return "application/octet-stream";

            // PDF
            if (file[0] == 0x25 && file[1] == 0x50)
                return "application/pdf";

            // JPG
            if (file[0] == 0xFF && file[1] == 0xD8)
                return "image/jpeg";

            // PNG
            if (file[0] == 0x89 && file[1] == 0x50)
                return "image/png";

            // DOCX (ZIP)
            if (file[0] == 0x50 && file[1] == 0x4B)
                return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

            return "application/octet-stream";
        }

        public static string GetExtension(string contentType) =>
            contentType switch
            {
                "application/pdf" => ".pdf",
                "image/jpeg" => ".jpg",
                "image/png" => ".png",
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => ".docx",
                _ => ".bin"
            };
    }

}
