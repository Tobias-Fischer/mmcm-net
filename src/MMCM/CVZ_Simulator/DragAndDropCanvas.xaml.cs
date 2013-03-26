/* 
 * Copyright (C) Stephane Lallee
 * Authors: Stephane Lallee
 * email:   stephane.lallee@gmail.com
 * website: http://ghostlessshell.blogspot.com.es/
 * Permission is granted to copy, distribute, and/or modify this program
 * under the terms of the GNU General Public License, version 2 or any
 * later version published by the Free Software Foundation.
 *
 * A copy of the license can be found in gpl.txt
 *
 * This program is distributed in the hope that it will be useful, but
 * WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General
 * Public License for more details
 */

/*
 * Code inspired by Marcelo Lopez Ruiz, based on his post at http://blogs.msdn.com/b/marcelolr/archive/2006/03/02/542641.aspx
 * 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DragDropCanvas
{
    /// <summary>
    /// Interaction logic for DragAndDropCanvas.xaml
    /// </summary>
    public partial class DragAndDropCanvas : Canvas
    {
        private Point ddStartPoint = new Point();
        private double ddOriginalLeft;
        private double ddOriginalTop;
        private bool ddIsDown = false;
        private bool ddIsDragging = false;
        private UIElement ddOriginalElement = null;
        private Rectangle ddOverlayRectangle = null;

        public DragAndDropCanvas()
        {
            InitializeComponent();
            this.PreviewMouseLeftButtonDown += ModelCanvas_PreviewMouseLeftButtonDown;
            this.PreviewMouseLeftButtonUp += ModelCanvas_PreviewMouseLeftButtonUp;
            this.PreviewMouseMove += ModelCanvas_PreviewMouseMove;
            this.PreviewKeyDown += ModelCanvas_PreviewKeyDown;
        }

        #region Drag and Drop
        private void ModelCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this == e.Source)
                return;
            ddIsDown = true;
            ddStartPoint = e.GetPosition(this);
            ddOriginalElement = e.Source as UIElement;
            this.CaptureMouse();
            e.Handled = true;
        }

        private void ModelCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (ddIsDown)
            {
                if (!ddIsDragging &&
                    Math.Abs(e.GetPosition(this).X - ddStartPoint.X) > SystemParameters.MinimumHorizontalDragDistance &&
                    Math.Abs(e.GetPosition(this).Y - ddStartPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    DragStarted();
                }
                if (ddIsDragging)
                    DragMoved();
            }
        }

        private void DragStarted()
        {
            ddIsDragging = true;

            ddOriginalLeft = Canvas.GetLeft(ddOriginalElement);
            ddOriginalTop = Canvas.GetTop(ddOriginalElement);

            // ' Add a rectangle to indicate where we'll end up.
            // ' We'll just use an alpha-blended visual brush.
            VisualBrush brush = new VisualBrush(ddOriginalElement);
            brush.Opacity = 0.5;

            ddOverlayRectangle = new Rectangle();
            ddOverlayRectangle.Width = ddOriginalElement.RenderSize.Width;
            ddOverlayRectangle.Height = ddOriginalElement.RenderSize.Height;
            ddOverlayRectangle.Fill = brush;

            this.Children.Add(ddOverlayRectangle);
        }

        private void DragMoved()
        {
            Point currentPosition = System.Windows.Input.Mouse.GetPosition(this);
            Double elementLeft = (currentPosition.X - ddStartPoint.X) + ddOriginalLeft;
            Double elementTop = (currentPosition.Y - ddStartPoint.Y) + ddOriginalTop;
            Canvas.SetLeft(ddOverlayRectangle, elementLeft);
            Canvas.SetTop(ddOverlayRectangle, elementTop);
        }

        private void ModelCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ddIsDown)
            {
                DragFinished(false);
                e.Handled = true;
            }
        }

        private void ModelCanvas_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && ddIsDragging)
                DragFinished(true);
        }

        private void DragFinished(bool canceled)
        {
            System.Windows.Input.Mouse.Capture(null);
            if (ddIsDragging)
            {
                this.Children.Remove(ddOverlayRectangle);
                if (!canceled)
                {
                    Canvas.SetLeft(ddOriginalElement, Canvas.GetLeft(ddOverlayRectangle));
                    Canvas.SetTop(ddOriginalElement, Canvas.GetTop(ddOverlayRectangle));
                }
                ddOverlayRectangle = null;
            }
            ddIsDragging = false;
            ddIsDown = false;
        }

        #endregion

    }
}
