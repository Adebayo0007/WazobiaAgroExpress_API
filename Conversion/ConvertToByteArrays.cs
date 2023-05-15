namespace AgroExpressAPI.Conversion
{
    public class ConvertToByteArrays
    {
        public static byte[] ToBytearray(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }

        public static IFormFile ConverToIFormFile(byte[] bytes, string  fileName)
        {
            MemoryStream stream = new MemoryStream(bytes);
            IFormFile file = new FormFile(stream,0,bytes.Length,null,fileName);
            return file;
        }
    }
}