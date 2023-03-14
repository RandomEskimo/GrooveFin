using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrooveFin.Helpers
{
	internal static class Helpers
	{
		public static R? Let<T, R>(this T? This, Func<T?, R?> Body) => Body(This);
		public static T Also<T>(this T This, Action<T> Body)
		{
			Body(This);
			return This;
		}
		public static void UpdateChildren<T>(
			this VerticalStackLayout Layout, 
			List<T> NewData,
			Func<IView,T?> GetDataFromChild,
			Action<IView, T> SetChildData,
			Func<T, IView> GenerateNewChild,
			Func<T, T, bool> CompareData
			)
		{
			int i = 0;
			for(;i < Layout.Children.Count && i < NewData.Count;++i)
			{
				var child = Layout.Children[i];
				var childData = GetDataFromChild(child);
				var newData = NewData[i];
				if(childData == null || !CompareData(childData, newData))
				{
					SetChildData(child, newData);
				}
			}
			if(NewData.Count > i)
			{
				for (; i < NewData.Count; ++i)
				{
					Layout.Children.Add(GenerateNewChild(NewData[i]));
				}
			}
			if(Layout.Children.Count > i)
			{
				for(int n = Layout.Children.Count - 1;n >= i;--n)
				{
					Layout.Children.RemoveAt(n);
				}
			}
		}
	}
}
