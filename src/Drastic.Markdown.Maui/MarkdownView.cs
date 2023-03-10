// <copyright file="MarkdownView.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

#nullable disable

namespace Drastic.Markdown
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text.RegularExpressions;
    using Extensions;
    using Markdig;
    using Markdig.Syntax;
    using Markdig.Syntax.Inlines;

    public class MarkdownView : ContentView
    {
        public Action<string> NavigateToLink { get; set; } = (s) => { };

        public static MarkdownTheme Global = new DarkMarkdownTheme();

        static readonly WebClient webClient = new WebClient();

        public string Markdown
        {
            get { return (string)GetValue(MarkdownProperty); }
            set { SetValue(MarkdownProperty, value); }
        }

        public static readonly BindableProperty MarkdownProperty = BindableProperty.Create(nameof(Markdown), typeof(string), typeof(MarkdownView), null, propertyChanged: OnMarkdownChanged);

        public string RelativeUrlHost
        {
            get { return (string)GetValue(RelativeUrlHostProperty); }
            set { SetValue(RelativeUrlHostProperty, value); }
        }

        public static readonly BindableProperty RelativeUrlHostProperty = BindableProperty.Create(nameof(RelativeUrlHost), typeof(string), typeof(MarkdownView), null, propertyChanged: OnMarkdownChanged);

        public MarkdownTheme Theme
        {
            get { return (MarkdownTheme)GetValue(ThemeProperty); }
            set { SetValue(ThemeProperty, value); }
        }

        public static readonly BindableProperty ThemeProperty = BindableProperty.Create(nameof(Theme), typeof(MarkdownTheme), typeof(MarkdownView), Global, propertyChanged: OnMarkdownChanged);

        bool isQuoted;

        List<View> queuedViews = new List<View>();

        static void OnMarkdownChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var view = bindable as MarkdownView;
            view.RenderMarkdown();
        }

        StackLayout stack;

        List<KeyValuePair<string, string>> links = new List<KeyValuePair<string, string>>();

        void RenderMarkdown()
        {
            stack = new StackLayout()
            {
                Spacing = Theme.VerticalSpacing,
            };

            Padding = Theme.Margin;

            BackgroundColor = Theme.BackgroundColor;

            if (!string.IsNullOrEmpty(Markdown))
            {
                var pipeline = new MarkdownPipelineBuilder();

                if (Theme.Link.UseAutolinksExtension)
                {
                    pipeline = pipeline.UseAutoLinks();
                }

                if (Theme.UseEmojiAndSmileyExtension)
                {
                    pipeline = pipeline.UseEmojiAndSmiley();
                }

                var parsed = Markdig.Markdown.Parse(Markdown, pipeline.Build());
                Render(parsed.AsEnumerable());
            }

            Content = stack;
        }

        void Render(IEnumerable<Block> blocks)
        {
            foreach (var block in blocks)
            {
                Render(block);
            }
        }

        void AttachLinks(View view)
        {
            if (links.Any())
            {
                var blockLinks = links.Select(o => new LinkData { Text = o.Key, Link = o.Value }).ToList();
                view.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async () =>
                    {
                        try
                        {
                            if (Theme.Link.CustomTapHandler != null)
                            {
                                Theme.Link.CustomTapHandler.Invoke(blockLinks);
                            }
                            else
                            {
                                if (blockLinks.Count > 1)
                                {
                                    var result = await Application.Current.MainPage.DisplayActionSheet(Theme.Link.OpenLinkSheetTitle, Theme.Link.OpenLinkSheetCancel, null, blockLinks.Select(x => x.Text).ToArray());
                                    var link = blockLinks.FirstOrDefault(x => x.Text == result);
                                    NavigateToLink(link.Link);
                                }
                                else
                                {
                                    NavigateToLink(blockLinks.First().Link);
                                }
                            }

                        }
                        catch (Exception) { }
                    }),
                });

                links = new List<KeyValuePair<string, string>>();
            }
        }

        #region Rendering blocks

        void Render(Block block)
        {
            switch (block)
            {
                case HeadingBlock heading:
                    Render(heading);
                    break;

                case ParagraphBlock paragraph:
                    Render(paragraph);
                    break;

                case QuoteBlock quote:
                    Render(quote);
                    break;

                case CodeBlock code:
                    Render(code);
                    break;

                case ListBlock list:
                    Render(list);
                    break;

                case ThematicBreakBlock thematicBreak:
                    Render(thematicBreak);
                    break;

                case HtmlBlock html:
                    Render(html);
                    break;

                default:
                    Debug.WriteLine($"Can't render {block.GetType()} blocks.");
                    break;
            }

            if (queuedViews.Any())
            {
                foreach (var view in queuedViews)
                {
                    stack.Children.Add(view);
                }
                queuedViews.Clear();
            }
        }

        void Render(ThematicBreakBlock block)
        {
            var style = Theme.Separator;

            if (style.BorderSize > 0)
            {
                stack.Children.Add(new BoxView
                {
                    HeightRequest = style.BorderSize,
                    BackgroundColor = style.BorderColor,
                });
            }
        }

        void Render(ListBlock block)
        {
            var listTheme = block.IsOrdered ? Theme.OrderedList : Theme.UnorderedList;

            var initialStack = stack;

            stack = new StackLayout()
            {
                Spacing = listTheme.ItemsVerticalSpacing,
                Margin = listTheme.ListMargin,
            };

            var itemsCount = block.Count();
            for (var i = 0; i < itemsCount; i++)
            {
                var item = block.ElementAt(i);

                if (item is ListItemBlock itemBlock)
                {
                    Render(block, listTheme, i + 1, itemBlock);
                }
            }

            initialStack.Children.Add(stack);

            stack = initialStack;

        }

        void Render(ListBlock parent, ListStyle listTheme, int index, ListItemBlock block)
        {
            var initialStack = stack;

            stack = new StackLayout()
            {
                Spacing = 0,
                VerticalOptions = listTheme.ItemVerticalOptions,
            };

            Render(block.AsEnumerable());
            Grid.SetColumn(stack, 1);

            var horizontalStack = new Grid
            {
                ColumnDefinitions = new ColumnDefinitionCollection {
                    new ColumnDefinition() { Width = GridLength.Auto },
                    new ColumnDefinition() { Width = GridLength.Star },
                },
                ColumnSpacing = listTheme.Spacing ?? Theme.Margin,
                RowSpacing = 0,
                Margin = new Thickness(block.Column * listTheme.Indentation, 0, 0, 0),
            };

            if (listTheme.BulletStyleType == ListStyleType.None)
            {
                horizontalStack.ColumnSpacing = 0;
            }

            var bullet = GetListBullet(listTheme, index, parent, block);

            if (bullet != null)
            {
                Grid.SetColumn(bullet, 0);
                horizontalStack.Children.Add(bullet);
            }

            horizontalStack.Children.Add(stack);
            initialStack.Children.Add(horizontalStack);

            stack = initialStack;
        }

        View GetListBullet(ListStyle listTheme, int index, ListBlock parent, ListItemBlock block)
        {

            if (listTheme.BulletStyleType == ListStyleType.None)
            {
                return null;
            }

            if (listTheme.BulletStyleType == ListStyleType.Custom)
            {
                return listTheme.CustomCallback?.Invoke(index, parent, block);
            }

            if (listTheme.BulletStyleType == ListStyleType.Decimal || listTheme.BulletStyleType == ListStyleType.Symbol)
            {
                return new Label
                {
                    Text = listTheme.BulletStyleType == ListStyleType.Symbol ? listTheme.Symbol : $"{index}.",
                    FontSize = listTheme.BulletFontSize ?? Theme.Paragraph.FontSize,
                    TextColor = listTheme.BulletColor ?? Theme.Paragraph.ForegroundColor,
                    LineHeight = listTheme.BulletLineHeight ?? Theme.Paragraph.LineHeight,
                    FontAttributes = listTheme.BulletFontAttributes,
                    VerticalOptions = listTheme.BulletVerticalOptions,
                };
            }
            else if (listTheme.BulletStyleType == ListStyleType.Square || listTheme.BulletStyleType == ListStyleType.Circle)
            {
                var bullet = new Frame
                {
                    WidthRequest = listTheme.BulletSize,
                    HeightRequest = listTheme.BulletSize,
                    BackgroundColor = listTheme.BulletColor ?? Theme.Paragraph.ForegroundColor,
                    Padding = 0,
                    HasShadow = false,
                    VerticalOptions = listTheme.BulletVerticalOptions,
                    CornerRadius = 0,
                };

                if (listTheme.BulletStyleType == ListStyleType.Circle)
                {
                    bullet.CornerRadius = listTheme.BulletSize / 2;
                }

                return bullet;
            }

            return null;
        }

        void Render(HeadingBlock block)
        {
            MarkdownStyle style;

            switch (block.Level)
            {
                case 1:
                    style = Theme.Heading1;
                    break;
                case 2:
                    style = Theme.Heading2;
                    break;
                case 3:
                    style = Theme.Heading3;
                    break;
                case 4:
                    style = Theme.Heading4;
                    break;
                case 5:
                    style = Theme.Heading5;
                    break;
                default:
                    style = Theme.Heading6;
                    break;
            }

            var foregroundColor = isQuoted ? Theme.Quote.ForegroundColor : style.ForegroundColor;

            var label = new Label
            {
                FormattedText = CreateFormatted(block.Inline, style.FontFamily, style.Attributes, style.TextDecorations, foregroundColor, style.BackgroundColor, style.FontSize, style.LineHeight),
                HorizontalTextAlignment = style.HorizontalTextAlignment,
                VerticalTextAlignment = style.VerticalTextAlignment,
            };

            AttachLinks(label);

            if (style.BorderSize > 0)
            {
                var headingStack = new StackLayout();
                headingStack.Children.Add(label);
                headingStack.Children.Add(new BoxView
                {
                    HeightRequest = style.BorderSize,
                    BackgroundColor = style.BorderColor,
                });
                stack.Children.Add(headingStack);
            }
            else
            {
                stack.Children.Add(label);
            }
        }

        void Render(ParagraphBlock block)
        {
            var style = Theme.Paragraph;
            var foregroundColor = isQuoted ? Theme.Quote.ForegroundColor : style.ForegroundColor;
            var label = new Label
            {
                FormattedText = CreateFormatted(block.Inline, style.FontFamily, style.Attributes, style.TextDecorations, foregroundColor, style.BackgroundColor, style.FontSize, style.LineHeight),
                HorizontalTextAlignment = style.HorizontalTextAlignment,
                VerticalTextAlignment = style.VerticalTextAlignment,
            };
            AttachLinks(label);
            stack.Children.Add(label);
        }

        void Render(HtmlBlock block)
        {
            // ?
        }

        void Render(QuoteBlock block)
        {
            var initialIsQuoted = isQuoted;
            var initialStack = stack;

            isQuoted = true;
            stack = new StackLayout()
            {
                Spacing = Theme.VerticalSpacing,
            };

            var style = Theme.Quote;

            if (style.BorderSize > 0)
            {
                var horizontalStack = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    BackgroundColor = Theme.Quote.BackgroundColor,
                };

                horizontalStack.Children.Add(new BoxView()
                {
                    WidthRequest = style.BorderSize,
                    BackgroundColor = style.BorderColor,
                });

                horizontalStack.Children.Add(stack);
                initialStack.Children.Add(horizontalStack);
            }
            else
            {
                stack.BackgroundColor = Theme.Quote.BackgroundColor;
                initialStack.Children.Add(stack);
            }

            Render(block.AsEnumerable());

            isQuoted = initialIsQuoted;
            stack = initialStack;
        }

        void Render(CodeBlock block)
        {
            var style = Theme.Code;
            var label = new Label
            {
                TextColor = style.ForegroundColor,
                FontAttributes = style.Attributes,
                TextDecorations = style.TextDecorations,
                FontFamily = style.FontFamily,
                FontSize = style.FontSize,
                Text = string.Join(Environment.NewLine, block.Lines),
                LineHeight = style.LineHeight,
                HorizontalTextAlignment = style.HorizontalTextAlignment,
                VerticalTextAlignment = style.VerticalTextAlignment,
            };
            stack.Children.Add(new Frame()
            {
                CornerRadius = 3,
                HasShadow = false,
                Padding = Theme.Margin,
                BackgroundColor = style.BackgroundColor,
                Content = label
            });
        }

        FormattedString CreateFormatted(ContainerInline inlines, string family, FontAttributes attributes, TextDecorations textDecorations, Color foregroundColor, Color backgroundColor, float size, float lineHeight)
        {
            var fs = new FormattedString();

            foreach (var inline in inlines)
            {
                var spans = CreateSpans(inline, family, attributes, textDecorations, foregroundColor, backgroundColor, size, lineHeight);
                foreach (var span in spans)
                {
                    fs.Spans.Add(span);
                }
            }

            return fs;
        }

        Span[] CreateSpans(Inline inline, string family, FontAttributes attributes, TextDecorations textDecorations, Color foregroundColor, Color backgroundColor, float size, float lineHeight)
        {
            switch (inline)
            {
                case LiteralInline literal:
                    return new[]
                    {
                        new Span
                        {
                            Text = literal.Content.Text.Substring(literal.Content.Start, literal.Content.Length),
                            FontAttributes = attributes,
                            TextColor = foregroundColor,
                            BackgroundColor = backgroundColor,
                            FontSize = size,
                            FontFamily = family,
                            LineHeight = lineHeight,
                            TextDecorations = textDecorations,
                        }
                    };

                case EmphasisInline emphasis:
                    var childAttributes = attributes | (emphasis.IsDouble ? FontAttributes.Bold : FontAttributes.Italic);
                    return emphasis.SelectMany(x => CreateSpans(x, family, childAttributes, textDecorations, foregroundColor, backgroundColor, size, lineHeight)).ToArray();

                case LineBreakInline breakline:
                    return new[] { new Span { Text = "\n" } };

                case LinkInline link:

                    var url = link.Url;

                    if (!Theme.Link.ExternalProtocols.Any(o => url.StartsWith(o)))
                    {
                        url = $"{RelativeUrlHost?.TrimEnd('/')}/{url.TrimStart('/')}";
                    }

                    if (TryLoadYouTubePreview(link.Url, out var youtubePreview))
                    {
                        queuedViews.Add(youtubePreview);
                        return new Span[0];
                    }
                    else if (link.IsImage)
                    {
                        var image = new Image();

                        if (Path.GetExtension(url) == ".svg")
                        {
                            image.RenderSvg(url);
                        }
                        else
                        {
                            image.Source = url;
                        }

                        queuedViews.Add(image);
                        return new Span[0];
                    }
                    else
                    {
                        var spans = link.SelectMany(x => CreateSpans(x, Theme.Link.FontFamily ?? family, Theme.Link.Attributes, Theme.Link.TextDecorations, Theme.Link.ForegroundColor, Theme.Link.BackgroundColor, size, lineHeight)).ToArray();
                        links.Add(new KeyValuePair<string, string>(string.Join("", spans.Select(x => x.Text)), url));
                        return spans;
                    }

                case AutolinkInline autolink:

                    return RenderAutolink(autolink, size, lineHeight, family);

                case CodeInline code:
                    return new[]
                    {
                        new Span()
                        {
                            Text="\u2002",
                            FontSize = size,
                            FontFamily = Theme.Code.FontFamily,
                            TextColor = Theme.Code.ForegroundColor,
                            BackgroundColor = Theme.Code.BackgroundColor
                        },
                        new Span
                        {
                            Text = code.Content,
                            FontAttributes = Theme.Code.Attributes,
                            TextDecorations = Theme.Code.TextDecorations,
                            FontSize = size,
                            FontFamily = Theme.Code.FontFamily,
                            TextColor = Theme.Code.ForegroundColor,
                            BackgroundColor = Theme.Code.BackgroundColor
                        },
                        new Span()
                        {
                            Text="\u2002",
                            FontSize = size,
                            FontFamily = Theme.Code.FontFamily,
                            TextColor = Theme.Code.ForegroundColor,
                            BackgroundColor = Theme.Code.BackgroundColor
                        },
                    };

                default:
                    Debug.WriteLine($"Can't render {inline.GetType()} inlines.");
                    return new Span[0];
            }
        }

        Span[] RenderAutolink(AutolinkInline autolink, float fontSize, float lineHeight, string fontFamily)
        {
            var url = autolink.Url;

            if (TryLoadYouTubePreview(url, out var youtubePreview))
            {
                queuedViews.Add(youtubePreview);
                return new Span[0];
            }

            if (autolink.IsEmail && !url.ToLower().StartsWith("mailto:"))
            {
                url = $"mailto:{url}";
            }

            links.Add(new KeyValuePair<string, string>(autolink.Url, url));

            var styles = Theme.Link;

            return new[] {
                new Span
                {
                    Text = autolink.Url,
                    FontAttributes = styles.Attributes,
                    TextDecorations = styles.TextDecorations,
                    TextColor = styles.ForegroundColor,
                    BackgroundColor = styles.BackgroundColor,
                    FontSize = fontSize,
                    FontFamily = Theme.Link.FontFamily ?? fontFamily,
                    LineHeight = lineHeight,
                }
            };
        }

        bool TryLoadYouTubePreview(string url, out View image)
        {
            image = null;

            if (!Theme.Link.LoadYouTubePreview)
            {
                return false;
            }

            var reg = new List<Regex>
            {
                new Regex(@"https?:\/\/www\.youtube\.com\/watch\?v=([^&]+)&?"),
                new Regex(@"https?:\/\/youtu\.be/([^&]+)&?"),
                new Regex(@"https?:\/\/www\.youtube-nocookie\.com\/embed\/([^&]+)&?"),
            };

            var match = reg.Select(o => o.Match(url)).FirstOrDefault(o => o.Success);

            if (match == null)
            {
                return false;
            }

            var code = match.Groups[1].Value;

            var videoPreviewDescriptor = new VideoPreviewDescriptor
            {
                Code = code,
                VideoUrl = url,
            };

            var theme = Theme.Link.YouTubePreview;

            ImageSource imageSource;

            if (theme?.CustomLoadImage != null)
            {
                imageSource = theme.CustomLoadImage(videoPreviewDescriptor);
            }
            else
            {
                imageSource = DownloadImage(theme?.GenerateLoadImageUrl?.Invoke(videoPreviewDescriptor) ?? $"https://img.youtube.com/vi/{code}/hqdefault.jpg");
            }

            if (imageSource == null)
            {
                return false;
            }

            image = new Image
            {
                Source = imageSource,
            };

            image.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    if (Theme.Link.CustomTapHandler != null)
                    {
                        Theme.Link.CustomTapHandler.Invoke(new List<LinkData> { new LinkData { Link = url, Text = url } });
                    }
                    else
                    {
                        NavigateToLink(url);
                    }

                })
            });

            if (theme?.TransformView != null)
            {
                image = theme.TransformView(image as Image, videoPreviewDescriptor);
            }

            return true;
        }

        ImageSource DownloadImage(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return null;
            }

            try
            {
                var byteArray = webClient.DownloadData(url);

                if (byteArray == null || !byteArray.Any())
                {
                    return null;
                }

                return ImageSource.FromStream(() => new MemoryStream(byteArray));
            }
            catch (Exception) { }

            return null;
        }

        #endregion
    }
}