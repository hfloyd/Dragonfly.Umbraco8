namespace Dragonfly.UmbracoModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dragonfly.UmbracoHelpers;
    using Umbraco.Core;
    using Umbraco.Core.Models.PublishedContent;
    using Umbraco.Web;

    public class SimpleMediaInfo
    {
        public SimpleMediaInfo()
        {

        }

        public SimpleMediaInfo(IPublishedContent Node)
        {
            this.Id = Node.Id;
            this.Guid = Node.Key;
            this.Udi = Node.ToUdi();
            this.Name = Node.Name;
            this.Path = Node.Path;
            this.CreateDate = Node.CreateDate;
            this.CreatorId = Node.CreatorId;
            this.CreatorName = Node.CreatorName;
            this.UpdateDate = Node.UpdateDate;
            this.DocumentTypeAlias = Node.ContentType.Alias;
            this.FilePath =!string.IsNullOrEmpty( Node.Value<string>("umbracoFile"))? Node.Value<string>("umbracoFile"):"";
            this.Filename = GetFileNameOnly(FilePath);
            this.Url = Node.Url;
        }

        public Udi Udi { get; set; }

        public string Name { get; set; }
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string DocumentTypeAlias { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
        public string FilePath { get; set; }
        public string Filename { get; set; }
        public string CreatorName { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        private string GetFileNameOnly(string Path)
        {
            if (!string.IsNullOrEmpty(Path))
            {
                var structure = Path.Split('/');
                var name = structure.Last();
                return name;
            }
            else
            {
                return "";
            }
        }
    }
}
