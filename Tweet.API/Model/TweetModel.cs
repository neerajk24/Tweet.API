using System;
using System.Collections.Generic;

namespace Tweet.API.Model
{
    public class TweetModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime? Timestamp { get; set; }
        public int? Likes { get; set; }
        public List<string>? Comments { get; set; }
        public int? Shares { get; set; }
        public List<string>? Images { get; set; }
        public List<string>? Videos { get; set; }

        public TweetModel? ReTweetModel { get; set; }
    }
}


