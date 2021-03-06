#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports.Net											}
{																	}
{	Copyright (C) 2003-2016 Stimulsoft     							}
{	ALL RIGHTS RESERVED												}
{																	}
{	The entire contents of this file is protected by U.S. and		}
{	International Copyright Laws. Unauthorized reproduction,		}
{	reverse-engineering, and distribution of all or any portion of	}
{	the code contained in this file is strictly prohibited and may	}
{	result in severe civil and criminal penalties and will be		}
{	prosecuted to the maximum extent possible under the law.		}
{																	}
{	RESTRICTIONS													}
{																	}
{	THIS SOURCE CODE AND ALL RESULTING INTERMEDIATE FILES			}
{	ARE CONFIDENTIAL AND PROPRIETARY								}
{	TRADE SECRETS OF Stimulsoft										}
{																	}
{	CONSULT THE END USER LICENSE AGREEMENT FOR INFORMATION ON		}
{	ADDITIONAL RESTRICTIONS.										}
{																	}
{*******************************************************************}
*/
#endregion Copyright (C) 2003-2016 Stimulsoft

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using System.Drawing.Drawing2D;
using Stimulsoft.Base.Gauge;

namespace Stimulsoft.Base.Gauge.GaugeGeoms
{
    public class StiGraphicsArcGeometryGaugeGeom : StiGaugeGeom
    {
        #region Fields
        public readonly RectangleF rect;
        public readonly StiBrush background;
        public readonly StiBrush borderBrush;
        public readonly float borderWidth;
        public readonly float startAngle;
        public readonly float sweepAngle;
        public readonly float startWidth;
        public readonly float endWidth;
        #endregion

        #region Properties
        public override StiGaugeGeomType Type
        {
            get 
            {
                return StiGaugeGeomType.GraphicsArcGeometry;
            }
        }
        #endregion

        public StiGraphicsArcGeometryGaugeGeom(RectangleF rect, StiBrush background, StiBrush borderBrush, 
            float borderWidth, float startAngle, float sweepAngle, float startWidth, float endWidth)
        {
            this.rect = rect;
            this.background = background;
            this.borderBrush = borderBrush;
            this.borderWidth = borderWidth;
            this.startAngle = startAngle;
            this.sweepAngle = sweepAngle;
            this.startWidth = startWidth;
            this.endWidth = endWidth;
        }
    }
}