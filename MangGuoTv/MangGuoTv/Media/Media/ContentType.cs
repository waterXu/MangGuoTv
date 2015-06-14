
using System;
using System.Collections.Generic;

namespace SM.Media.Content
{
    public class ContentType : IEquatable<ContentType>
    {
        public ContentType(string name, ContentKind kind, string mimeType, IEnumerable<string> fileExts, IEnumerable<string> alternateMimeTypes = null);
        public ContentType(string name, ContentKind kind, string fileExt, string mimeType, IEnumerable<string> alternateMimeTypes = null);

        public ICollection<string> AlternateMimeTypes { get; }
        public ICollection<string> FileExts { get; }
        public ContentKind Kind { get; }
        public string MimeType { get; }
        public string Name { get; }

        public bool Equals(ContentType other);
        public override bool Equals(object obj);
        public override int GetHashCode();
        public override string ToString();
    }
}
