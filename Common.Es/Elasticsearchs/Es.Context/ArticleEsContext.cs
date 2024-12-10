using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Nest;

namespace Common.Es
{
    public class ArticleEsContext : EsContext<ArticleDto>
    {
        public ArticleEsContext(EsConfig esConfig) : base(esConfig)
        {
        }

        public override string IndexName => "article_index";

        public async Task<EsData<ArticleDto>> GetArticles(EsPageParameter parameter)
        {
            var query = new BoolQuery
            {
                Should = new List<QueryContainer>
            {
                new MatchQuery { Field = "ArticleTitle", Query = parameter.KeyWords, Fuzziness = Fuzziness.Auto },
                new MatchQuery { Field = "ArticleContent", Query = parameter.KeyWords, Fuzziness = Fuzziness.Auto }
            },
                MinimumShouldMatch = 1
            };

            return await SearchAsync(query,"PublishTime", true,parameter.PageNumber, parameter.PageSize);
        }
    }

    public class ArticleDto
    {
        [Text(Name = "ArticleID")]
        public long ArticleId { get; set; }
        [Text(Name = "UserID")]
        public long UserId { get; set; }
        [Text(Name = "ArticleTitle")]
        public string ArticleTitle { get; set; }
        [Text(Name = "ArticleContent")]
        public string ArticleContent { get; set; }
        [Text(Name = "ImageAddress")]
        public string? ImageAddress { get; set; }
        [Text(Name = "StandPoint")]
        public int? StandPoint { get; set; }
        [Text(Name = "PublishTime")]
        public DateTime PublishTime { get; set; }
        [Text(Name = "Status")]
        public int Status { get; set; }
        [Text(Name = "Likes")]
        public int Likes { get; set; }
        [Text(Name = "Shares")]
        public int Shares { get; set; }
        [Text(Name = "Comments")]
        public int Comments { get; set; }
        [Text(Name = "Reports")]
        public int Reports { get; set; }
        [Text(Name = "Sort")]
        public int Sort { get; set; }
        [Text(Name = "PublishingMode")]
        public int PublishingMode { get; set; }
        [Text(Name = "SourceType")]
        public int SourceType { get; set; }
        [Text(Name = "Reply")]
        public string? Reply { get; set; }
        [Text(Name = "IsTop")]
        public bool IsTop { get; set; }
        [Text(Name = "TopStartTime")]
        public long? TopEndTime { get; set; }
        [Text(Name = "Hot")]
        public decimal Hot { get; set; }
        [Text(Name = "EditUserId")]
        public long EditUserId { get; set; }
        [Text(Name = "UserType")]
        public int UserType { get; set; }
        [Text(Name = "UserNickname")]
        public string UserNickname { get; set; }
        [Text(Name = "ForbiddenState")]
        public int ForbiddenState { get; set; }
        [Text(Name = "PublishDateTime")]
        public long PublishDateTime { get; set; }
        [Text(Name = "TopArea")]
        public int TopArea { get; set; }
        [Text(Name = "SubscribeType")]
        public int SubscribeType { get; set; }
        [Text(Name = "CollectionCount")]
        public int CollectionCount { get; set; }
        [Text(Name = "Articletype")]
        public int Articletype { get; set; }
        [Text(Name = "NewsID")]
        public int NewsID { get; set; }
        [Text(Name = "CommentUserCount")]
        public int CommentUserCount { get; set; }
        [Text(Name = "TopStartTime")]
        public long? TopStartTime { get; set; }
        [Text(Name = "View")]
        public int View { get; set; }
        [Text(Name = "ViewDuration")]
        public int? ViewDuration { get; set; }
        [Text(Name = "Forwardings")]
        public int Forwardings { get; set; }
        [Text(Name = "ForwardingFId")]
        public long? ForwardingFId { get; set; }
        [Text(Name = "Freshness")]
        public decimal? Freshness { get; set; }
        [Text(Name = "ShelfReason")]
        public string? ShelfReason { get; set; }
        [Text(Name = "AuditTime")]
        public DateTime? AuditTime { get; set; }
        [Text(Name = "CreatedTime")]
        public long? CreatedTime { get; set; }
        [Text(Name = "EditTime")]
        public long? EditTime { get; set; }
    }

}
