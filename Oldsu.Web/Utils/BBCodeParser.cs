using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Oldsu.Web.Utils
{
        /// <summary>
    /// BBCode Helper allows formatting of text
    /// without the need to use html
    /// </summary>
    public class BBCodeHelper
    {
        #region Helper Classes
        interface IHtmlFormatter
        {
            string Format(string data);
        }
        
        protected class RegexFormatter : IHtmlFormatter
        {
            private string _replace;
            private Regex _regex;

            public RegexFormatter(string pattern, string replace)
                : this(pattern, replace, true)
            {

            }

            public RegexFormatter(string pattern, string replace, bool ignoreCase)
            {
                RegexOptions options = RegexOptions.Compiled;

                if (ignoreCase)
                {
                    options |= RegexOptions.IgnoreCase;
                }

                _replace = replace;
                _regex = new Regex(pattern, options);
            }

            public string Format(string data)
            {
                return _regex.Replace(data, _replace);
            }
        }
        
        protected class SearchReplaceFormatter : IHtmlFormatter
        {
            private string _pattern;
            private string _replace;

            public SearchReplaceFormatter(string pattern, string replace)
            {
                _pattern = pattern;
                _replace = replace;
            }

            public string Format(string data)
            {
                return data.Replace(_pattern, _replace);
            }
        }
        #endregion

        #region BBCode
        static List<IHtmlFormatter> _formatters;

        static BBCodeHelper()
        {
            _formatters = new List<IHtmlFormatter>();

            _formatters.Add(new RegexFormatter(@"<(.|\n)*?>", string.Empty));

            _formatters.Add(new SearchReplaceFormatter("\r", ""));
            _formatters.Add(new SearchReplaceFormatter("\n", "<br />"));

            _formatters.Add(new RegexFormatter(@"\[b(?:\s*)\]((.|\n)*?)\[/b(?:\s*)\]", "<b>$1</b>"));
            _formatters.Add(new RegexFormatter(@"\[i(?:\s*)\]((.|\n)*?)\[/i(?:\s*)\]", "<i>$1</i>"));
            _formatters.Add(new RegexFormatter(@"\[s(?:\s*)\]((.|\n)*?)\[/s(?:\s*)\]", "<strike>$1</strike>"));

            _formatters.Add(new RegexFormatter(@"\[left(?:\s*)\]((.|\n)*?)\[/left(?:\s*)]", "<div style=\"text-align:left\">$1</div>"));
            _formatters.Add(new RegexFormatter(@"\[center(?:\s*)\]((.|\n)*?)\[/center(?:\s*)]", "<div style=\"text-align:center\">$1</div>"));
            _formatters.Add(new RegexFormatter(@"\[right(?:\s*)\]((.|\n)*?)\[/right(?:\s*)]", "<div style=\"text-align:right\">$1</div>"));

            string quoteStart = "<blockquote><b>$1 said:</b></p><p>";
            string quoteEmptyStart = "<blockquote>";
            string quoteEnd = "</blockquote>";

            _formatters.Add(new RegexFormatter(@"\[quote=((.|\n)*?)(?:\s*)\]", quoteStart));
            _formatters.Add(new RegexFormatter(@"\[quote(?:\s*)\]", quoteEmptyStart));
            _formatters.Add(new RegexFormatter(@"\[/quote(?:\s*)\]", quoteEnd));

            _formatters.Add(new RegexFormatter(@"\[url(?:\s*)\]((?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+)\[/url(?:\s*)\]", "<a class=\"bbcode-link\" href=\"$1\" target=\"_blank\" title=\"$1\">$1</a>"));
            _formatters.Add(new RegexFormatter(@"\[url=""((?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+)""\]((.|\n)*?)\[/url(?:\s*)\]", "<a class=\"bbcode-link\" href=\"$1\" target=\"_blank\" title=\"$1\">$3</a>"));
            _formatters.Add(new RegexFormatter(@"\[url=((?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+)(?:\s*)\]((.|\n)*?)\[/url(?:\s*)\]", "<a class=\"bbcode-link\" href=\"$1\" target=\"_blank\" title=\"$1\">$3</a>"));
            _formatters.Add(new RegexFormatter(@"\[link(?:\s*)\]((?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+)\[/link(?:\s*)\]", "<a class=\"bbcode-link\" href=\"$1\" target=\"_blank\" title=\"$1\">$1</a>"));
            _formatters.Add(new RegexFormatter(@"\[link=((?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+)(?:\s*)\]((.|\n)*?)\[/link(?:\s*)\]", "<a class=\"bbcode-link\" href=\"$1\" target=\"_blank\" title=\"$1\">$3</a>"));

            _formatters.Add(new RegexFormatter(@"\[img(?:\s*)\]((?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+)\[/img(?:\s*)\]", "<img src=\"$1\" border=\"0\" alt=\"\" />"));
            _formatters.Add(new RegexFormatter(@"\[img align=((?:\w*?))(?:\s*)\]((?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+)\[/img(?:\s*)\]", "<img src=\"$3\" border=\"0\" align=\"$1\" alt=\"\" />"));
            _formatters.Add(new RegexFormatter(@"\[img=((?:\d)*?)x((?:\d)*?)(?:\s*)\]((?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+)\[/img(?:\s*)\]", "<img width=\"$1\" height=\"$3\" src=\"$5\" border=\"0\" alt=\"\" />"));

            _formatters.Add(new RegexFormatter(@"\[color=(#?(?:\w*?))(?:\s*)\]((.|\n)*?)\[/color(?:\s*)\]", "<span style=\"color:$1;\">$3</span>"));

            _formatters.Add(new RegexFormatter(@"\[hr(?:\s*)\]", "<hr />"));

            _formatters.Add(new RegexFormatter(@"\[size=((?:\w*?))(?:\s*)\]((.|\n)*?)\[/size(?:\s*)\]", "<span style=\"font-size:$1\">$3</span>"));
            _formatters.Add(new RegexFormatter(@"\[font=((?:\w*?))(?:\s*)\]((.|\n)*?)\[/font(?:\s*)\]", "<span style=\"font-family:$1;\">$3</span>"));
            _formatters.Add(new RegexFormatter(@"\[align=((?:\w*?))(?:\s*)\]((.|\n)*?)\[/align(?:\s*)\]", "<span style=\"text-align:$1;\">$3</span>"));
            _formatters.Add(new RegexFormatter(@"\[float=((?:\w*?))(?:\s*)\]((.|\n)*?)\[/float(?:\s*)\]", "<span style=\"float:$1;\">$3</div>"));

            string sListFormat = "<ol class=\"bbcode-list\" style=\"list-style:{0};\">$1</ol>";

            _formatters.Add(new RegexFormatter(@"\[\*(?:\s*)]\s*([^\[]*)", "<li>$1</li>"));
            _formatters.Add(new RegexFormatter(@"\[list(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", "<ul class=\"bbcode-list\">$1</ul>"));
            _formatters.Add(new RegexFormatter(@"\[list=1(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", string.Format(sListFormat, "decimal"), false));
            _formatters.Add(new RegexFormatter(@"\[list=i(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", string.Format(sListFormat, "lower-roman"), false));
            _formatters.Add(new RegexFormatter(@"\[list=I(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", string.Format(sListFormat, "upper-roman"), false));
            _formatters.Add(new RegexFormatter(@"\[list=a(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", string.Format(sListFormat, "lower-alpha"), false));
            _formatters.Add(new RegexFormatter(@"\[list=A(?:\s*)\]((.|\n)*?)\[/list(?:\s*)\]", string.Format(sListFormat, "upper-alpha"), false));
        }
        #endregion

        #region Format
        public static string Format(string data)
        {
            data = HttpUtility.HtmlEncode(data);
            
            foreach (IHtmlFormatter formatter in _formatters)
            {
                data = formatter.Format(data);
            }
            
            return data;
        }
        #endregion
    }
}