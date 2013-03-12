/* SharpKML port for Windows Phone
 * by Pierre BELIN <pierre@ree7.fr>, and project contributors (2013).
 *
 * Distributed under the Microsoft Public License (Ms-PL)
 * with attribution requirement. If you use this library in your projects,
 * please display an attribution in your application's credits. */

using SharpKml.Dom;
using System.Collections.Generic;
using System.Diagnostics;

namespace ree7.Helpers
{
	public static class KMLHelper
	{
		private static void ExtractPlacemarks(Feature feature, List<Placemark> placemarks)
		{
			// Is the passed in value a Placemark?
			Placemark placemark = feature as Placemark;
			if (placemark != null)
			{
				placemarks.Add(placemark);
			}
			else
			{
				// Is it a Container, as the Container might have a child Placemark?
				Container container = feature as Container;
				if (container != null)
				{
					// Check each Feature to see if it's a Placemark or another Container
					foreach (var f in container.Features)
					{
						ExtractPlacemarks(f, placemarks);
					}
				}
			}
		}

		public static List<Placemark> ParsePlacemarks(string kmlfile)
		{
			List<Placemark> placemarks = new List<Placemark>();

			// Parse the KML file
			SharpKml.Base.Parser kmlparser = new SharpKml.Base.Parser();
			kmlparser.ParseString(kmlfile, false);

			// It's good practice for the root element of the file to be a Kml element
			Kml kml = kmlparser.Root as Kml;
			if (kml != null)
			{
				ExtractPlacemarks(kml.Feature, placemarks);

				// Sort using their names
				placemarks.Sort((a, b) => string.Compare(a.Name, b.Name));

				// Display the results
				foreach (var placemark in placemarks)
				{
					Debug.WriteLine("KMLHelper : found placemark " + placemark.Name);
				}
			}

			return placemarks;
		}
	}
}
