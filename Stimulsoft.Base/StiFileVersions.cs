#region Copyright (C) 2003-2016 Stimulsoft
/*
{*******************************************************************}
{																	}
{	Stimulsoft Reports												}
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
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace Stimulsoft.Base
{
	/// <summary>
	/// Contains report files version constants.
	/// </summary>
	public sealed class StiFileVersions
	{
        //1.02-��������� �������� FilterEngine �� ���� �������� ����������� IStiFilter.
        //1.01-��������� �������� Topmost � StiBorder �����. ���� �������� Topmost �� ����� False, �� ��� ����������� ������ � StiBorder. ���� �����, �� �� �����������.
        //1.00- ��������� ������ �������
        public const string ReportFile = "1.02";
	}
}
