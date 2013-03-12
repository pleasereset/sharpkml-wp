/* SharpKML port for Windows Phone
 * by Pierre BELIN <pierre@ree7.fr>, and project contributors (2013).
 *
 * Distributed under the Microsoft Public License (Ms-PL)
 * with attribution requirement. If you use this library in your projects,
 * please display an attribution in your application's credits. */

using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using ree7.Helpers;
using SharpKml.Dom;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;

namespace Sample
{
	public partial class MainPage : PhoneApplicationPage
	{
		// Constructor
		public MainPage()
		{
			InitializeComponent();

			// Sample code to localize the ApplicationBar
			//BuildLocalizedApplicationBar();
		}

		// Sample code for building a localized ApplicationBar
		//private void BuildLocalizedApplicationBar()
		//{
		//    // Set the page's ApplicationBar to a new instance of ApplicationBar.
		//    ApplicationBar = new ApplicationBar();

		//    // Create a new button and set the text value to the localized string from AppResources.
		//    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
		//    appBarButton.Text = AppResources.AppBarButtonText;
		//    ApplicationBar.Buttons.Add(appBarButton);

		//    // Create a new menu item with the localized string from AppResources.
		//    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
		//    ApplicationBar.MenuItems.Add(appBarMenuItem);
		//}

		public void GetKML(Action<List<Placemark>> callback)
		{
			string kmlfile = "";
			var si = Application.GetResourceStream(new Uri("Resources/69.kml", UriKind.Relative));
			using (var sr = new StreamReader(si.Stream))
			{
				kmlfile = sr.ReadToEnd();
			}

			if (kmlfile != null)
			{
				// Parse the KML file
				callback(KMLHelper.ParsePlacemarks(kmlfile));
			}
			else
			{
				Debug.WriteLine("KML file was empty !");
				callback(null);
			}
		}

		private void BeginDrawKMLOverlay()
		{
			GetKML(placemarks =>
			{
				if (placemarks != null)
				{
					foreach (var p in placemarks)
					{
						try
						{
							Color fillColor = Color.FromArgb(128, 255, 0, 0);
							Color strokeColor = Colors.Black;

							MapPolygon shape = new MapPolygon()
							{
								FillColor = fillColor,
								StrokeColor = strokeColor,
								StrokeThickness = 3
							};

							foreach (var coord in ((SharpKml.Dom.Polygon)p.Geometry).OuterBoundary.LinearRing.Coordinates)
							{
								shape.Path.Add(new GeoCoordinate(coord.Latitude, coord.Longitude));
							}

							Map.MapElements.Add(shape);
						}
						catch (Exception)
						{
						}
					}
				}
			});
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			// Center the map of the right location
			Map.SetView(new GeoCoordinate(45.880356, 4.702795), 9, MapAnimationKind.Parabolic);

			BeginDrawKMLOverlay();
		}

	}
}