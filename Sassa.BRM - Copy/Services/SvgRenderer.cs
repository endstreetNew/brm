using Barcoder;
using SvgLib;
using System;
using System.IO;
using System.Linq;

namespace Sassa.Renderer.Svg
{
    public interface IRenderer
    {
        void Render(IBarcode barcode, Stream outputStream);
    }
    public sealed class SvgRenderer : IRenderer
    {
        private static readonly int[] Ean8LongerBars = new[] { 0, 2, 32, 34, 64, 66 };
        private static readonly int[] Ean13LongerBars = new[] { 0, 2, 46, 48, 92, 94 };
        private static readonly int[] Ean14LongerBars = new int[0];
        private readonly bool _includeEanContentAsText;

        public SvgRenderer(bool includeEanContentAsText = false)
        {
            _includeEanContentAsText = includeEanContentAsText;
        }

        private bool IncludeEanContent(IBarcode barcode) => _includeEanContentAsText && (barcode.Metadata.CodeKind == BarcodeType.EAN13 || barcode.Metadata.CodeKind == BarcodeType.EAN8 || barcode.Metadata.CodeKind == BarcodeType.EAN14);

        public void Render(IBarcode barcode, Stream outputStream)
        {
            barcode = barcode ?? throw new ArgumentNullException(nameof(barcode));
            outputStream = outputStream ?? throw new ArgumentNullException(nameof(outputStream));
            if (barcode.Bounds.Y == 1)
                Render1D(barcode, outputStream);
            else if (barcode.Bounds.Y > 1)
                Render2D(barcode, outputStream);
            else
                throw new NotSupportedException($"Y value of {barcode.Bounds.Y} is invalid");
        }

        private void Render1D(IBarcode barcode, Stream outputStream)
        {
            var document = SvgDocument.Create();
            int height = IncludeEanContent(barcode) ? 55 : 50;
            //viewBox = <min-x> <min-y> <width> <height>
            document.ViewBox = new SvgViewBox
            {
                Left = 0,
                Top = 0,
                Width = 160,
                Height = 60
            };
            document.Fill = "#FFFFFF";
            document.Stroke = "#000000";
            document.AddClass("shape-rendering=\"crispEdges\"");
            //document.StrokeWidth = 1;
            //document.StrokeLineCap = SvgStrokeLineCap.Butt;

            var prevBar = false;
            for (var x = 0; x < barcode.Bounds.X; x++)
            {
                if (!barcode.At(x, 0))
                {
                    prevBar = false;
                    continue;
                }

                SvgLine line;
                int lineHeight = height;
                if (IncludeEanContent(barcode))
                {
                    switch (barcode.Metadata.CodeKind)
                    {
                        case BarcodeType.EAN13:
                            if (!Ean13LongerBars.Contains(x))
                            {
                                lineHeight = 48;
                            }
                            break;
                        case BarcodeType.EAN8:
                            if (!Ean8LongerBars.Contains(x))
                            {
                                lineHeight = 48;
                            }
                            break;
                    }
                }

                if (prevBar)
                {
                    line = document.AddLine();
                    line.StrokeWidth = 1.5;
                    line.X1 = line.X2 = x + barcode.Margin - 0.25;
                    line.Y1 = 0;
                    line.Y2 = lineHeight;
                }
                else
                {
                    line = document.AddLine();
                    line.X1 = line.X2 = x + barcode.Margin;
                    line.Y1 = 0;
                    line.Y2 = lineHeight;
                }

                prevBar = true;
            }

            if (IncludeEanContent(barcode))
            {
                if (barcode.Metadata.CodeKind == BarcodeType.EAN13)
                {
                    AddText(document, 4, 54.5D, barcode.Content.Substring(0, 1));
                    AddText(document, 21, 54.5D, barcode.Content.Substring(1, 6));
                    AddText(document, 67, 54.5D, barcode.Content.Substring(7));
                }
                else
                {
                    AddText(document, 18, 54.5D, barcode.Content.Substring(0, 4));
                    AddText(document, 50, 54.5D, barcode.Content.Substring(4));
                }
            }

            document.Save(outputStream);
        }

        private void AddText(SvgDocument doc, double x, double y, string t)
        {
            SvgText text = doc.AddText();
            text.FontFamily = "arial";
            text.Text = t;
            text.X = x;
            text.Y = y;
            text.StrokeWidth = 0;
            text.Fill = "#000000";
            text.FontSize = 8D;
        }

        private void Render2D(IBarcode barcode, Stream outputStream)
        {
            var document = SvgDocument.Create();

            document.ViewBox = new SvgViewBox
            {
                Left = 0,
                Top = 0,
                Width = 100,// barcode.Bounds.X + 2 * barcode.Margin,
                Height = 100// barcode.Bounds.Y + 2 * barcode.Margin
            };
            document.Fill = "#FFFFFF";
            document.Stroke = "#000000";
            document.StrokeWidth = 0.01;
            //document.StrokeLineCap = SvgStrokeLineCap.Butt;
            //document.AddClass("height:\"100\";");
            //document.AddClass("width:\"100\";");
            document.AddClass("shape-rendering:\"crispEdges\";");


            SvgGroup group = document.AddGroup();
            group.Fill = "#000000";
            for (int y = 0; y < barcode.Bounds.Y; y++)
            {
                for (int x = 0; x < barcode.Bounds.X; x++)
                {
                    if (barcode.At(x, y))
                    {
                        SvgRect rect = group.AddRect();
                        rect.X = x + barcode.Margin;
                        rect.Y = y + barcode.Margin;
                        rect.Width = 1;
                        rect.Height = 1;
                    }
                }
            }

            document.Save(outputStream);
        }
    }
}
