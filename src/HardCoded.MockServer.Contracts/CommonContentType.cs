using System.Net.Mime;

namespace HardCoded.MockServer.Contracts
{
    public class CommonContentType : ContentType
    {
        //public string ContentType { get; }
        //  public static implicit operator ContentType(CommonContentType contentType) => new ContentType(contentType.ContentType);
        public int Id { get; }

        /// <summary>
        /// Add charset information to the content type.
        /// </summary>
        /// <remarks>
        /// text/xml; --> text/xml; charset="utf-8"
        /// </remarks>
        /// <param name="charset">The charset of the encoding used.</param>
        public CommonContentType WithCharset(string charset)
        {
            CharSet = charset;
            return this;
        }
        
        private CommonContentType(string contentType, int id)
        {
            Name = contentType;
            //  ContentType = contentType;
            Id = id;
        }
        
        public static CommonContentType Json = new CommonContentType("application/json", 0);
        public static CommonContentType Xml = new CommonContentType("text/xml", 1);
        public static CommonContentType Soap12 = new CommonContentType("application/soap+xml", 2);
        public static CommonContentType Plain = new CommonContentType("text/plain", 3);
        public static CommonContentType Html = new CommonContentType("text/html", 4);
        
        
    }
}