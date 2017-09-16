using System;
using Tugberk.Domain;

namespace Tugberk.Persistance.SqlServer
{
    public enum PostFormatEntity 
    {
        PlainText = 1,
        Html = 2,
        Markdown = 3
    }

    public static class PostFormatEntityExtensions 
    {
        public static PostFormatEntity ToEntityModel(this PostFormat postFormat) 
        {
            switch (postFormat)
            {
                case PostFormat.Html:
                    return PostFormatEntity.Html;

                case PostFormat.Markdown:
                    return PostFormatEntity.Markdown;

                case PostFormat.PlainText:
                    return PostFormatEntity.PlainText;

                default:
                    throw new NotSupportedException();
            }
        }

        public static PostFormat ToDomainModel(this PostFormatEntity postFormatEntity) 
        {
            switch (postFormatEntity)
            {
                case PostFormatEntity.Html:
                    return PostFormat.Html;

                case PostFormatEntity.Markdown:
                    return PostFormat.Markdown;

                case PostFormatEntity.PlainText:
                    return PostFormat.PlainText;

                default:
                    throw new NotSupportedException();
            }
        }
    }
}
